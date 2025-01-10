using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;



public class Character_Controller : NetworkBehaviour
{
    [Header("Player References")]
    [SerializeField] private Collider2D attackCol;
    public Rigidbody2D rb;
    [SerializeField] private Transform attackPos;
    [SerializeField] private Item_Product nearbyItem;
    [SerializeField] public Item_Product heldItem; // El ítem que el jugador está sosteniendo
    [SerializeField] PlayerInputs _playerInputs;
    [SerializeField] private CinemachineVirtualCamera _Vcamera;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer sp;
    Enemy enemy;
    PlayerDataList playerData;

    [Header("Player Settings")]
    private NetworkVariable<bool> netFlipX = new NetworkVariable<bool>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner // Solo el servidor puede escribir
    );

    private NetworkVariable<NetworkObjectReference> netHeldItem = new NetworkVariable<NetworkObjectReference>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
)   ;
    [Header("Item Network Configuration")]
    private Vector3 lastItemPosition;
    private Vector3 targetItemPosition;
    private float itemLerpTime = 0f;
    private float itemLerpDuration = 0.1f; // Ajusta este valor según necesites

    [SerializeField] private bool stunned;
    [SerializeField] private float stunTime = 2f;
    [SerializeField] private float attackRadius;
    [SerializeField] private bool isNearItem = false;
    public float moveSpeed;
    public Vector3 itemCarryOffset = new Vector3(0.5f, 0.5f, 0); // Offset del ítem respecto al jugador
    public Vector2 moveDir;


    void Start()
    {
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
        // Acción para coger el ítem
        _playerInputs.PlayerActions.PickItem.performed += ctx => PickUpItem();
        //Soltar el item
        _playerInputs.PlayerActions.DropItem.performed += ctx => DropItem();

        attackCol = GetComponentInChildren<Collider2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerData = GetComponent<PlayerDataList>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn()
    {
        netHeldItem.OnValueChanged += OnNetHeldItemChanged;
        netFlipX.OnValueChanged += OnFlipChanged;
        _Vcamera = transform.GetComponentInChildren<CinemachineVirtualCamera>();
        if (IsOwner && _Vcamera != null)
        {
            _Vcamera.Priority = 10;
            _Vcamera.Follow = transform;
        }
        else if (_Vcamera != null)
        {
            _Vcamera.Priority = 0;
        }
    }

    void FixedUpdate()
    {
        if (!IsSpawned) return;

        // Actualizar posición del ítem en FixedUpdate
        if (netHeldItem.Value.TryGet(out NetworkObject networkObject))
        {
            Item_Product item = networkObject.GetComponent<Item_Product>();
            if (item != null)
            {
                Vector3 newPosition = transform.position + itemCarryOffset;
                // El dueño actualiza directamente
                item.transform.position = newPosition;
                UpdateItemPositionServerRpc(newPosition);
            }
        }
    }

    void Update()
    {
        Move();
        Hit();

        if (netHeldItem.Value.TryGet(out NetworkObject networkObject))
        {
            Item_Product item = networkObject.GetComponent<Item_Product>();
            if (item != null)
            {
                item.transform.position = transform.position + itemCarryOffset;
            }
        }
    }
    void OnFlipChanged(bool previousValue, bool newValue)
    {
        sp.flipX = newValue;
        // Actualizar la posición del punto de ataque
        attackPos.localPosition = new Vector3(newValue ? -Mathf.Abs(attackPos.localPosition.x) : Mathf.Abs(attackPos.localPosition.x),
            attackPos.localPosition.y,
            attackPos.localPosition.z);
    }
    void Move()
    {
        moveDir = _playerInputs.PlayerActions.Mover.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);

        if (moveDir.x >= 0.1f || moveDir.y >= 0.1 || moveDir.x <= -0.1 || moveDir.y <= -0.1)
        {
            anim.SetBool("Move", true);
            anim.SetBool("Idle", false);

            bool shouldFlip = (moveDir.x <= -0.1 || moveDir.y <= -0.1);
            if (netFlipX.Value != shouldFlip)
            {
                netFlipX.Value = shouldFlip;
            }
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Move", false);
        }
    }


    private void Hit()
    {
        if (_playerInputs.PlayerActions.Attack.triggered && heldItem == null)
        {
            Collider2D[] enemigos = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, LayerMask.GetMask("Enemigos"));

            anim.SetBool("Hit", true);
            anim.SetBool("Idle", false);

            foreach (Collider2D enemigo in enemigos)
            {
                Character_Controller player = enemigo.GetComponentInParent<Character_Controller>();

                if (player != null)
                {
                    HitEnemy(player.gameObject, stunned);
                }
            }
            StartCoroutine("FinishAttack");
        }
    }
    IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Hit", false);
        anim.SetBool("Idle", true);
    }
    public void PickUpItem()
    {
        if (isNearItem && nearbyItem != null && heldItem == null)
        {
            Debug.Log("Intentando recoger item");
            NetworkObject networkObject = nearbyItem.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                // El cliente llama al ServerRpc
                PickUpItemServerRpc(new NetworkObjectReference(networkObject));
                heldItem = nearbyItem;  // Actualización local
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]  // Añade RequireOwnership = false
    private void PickUpItemServerRpc(NetworkObjectReference itemRef)
    {
        Debug.Log("Pick up item server rpc");
        netHeldItem.Value = itemRef;

        if (itemRef.TryGet(out NetworkObject networkObject))
        {
            networkObject.transform.SetParent(transform);
        }
    }

    void DropItem()
    {
        if (heldItem != null)
        {
            DropItemServerRpc();
            heldItem = null;
        }
    }

    [ServerRpc(RequireOwnership = false)]  // Añade RequireOwnership = false
    private void DropItemServerRpc()
    {
        if (netHeldItem.Value.TryGet(out NetworkObject networkObject))
        {
            networkObject.Despawn(true);
        }
        netHeldItem.Value = default;
    }
    // Este método se ejecuta en todos los clientes cuando cambia la NetworkVariable
    private void OnNetHeldItemChanged(NetworkObjectReference previousValue, NetworkObjectReference newValue)
    {
        Debug.Log("NetworkVariable changed");
        if (newValue.TryGet(out NetworkObject networkObject))
        {
            // Actualizar visualización del item
            networkObject.transform.position = transform.position + itemCarryOffset;
        }
    }

    [ServerRpc]
    private void UpdateItemPositionServerRpc(Vector3 newPosition)
    {
        UpdateItemPositionClientRpc(newPosition);
    }
    [ClientRpc]
    private void UpdateItemPositionClientRpc(Vector3 newPosition)
    {
        lastItemPosition = targetItemPosition;
        targetItemPosition = newPosition;
        itemLerpTime = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item_Product item = collision.GetComponent<Item_Product>();
        if (item != null)
        {
            nearbyItem = item;
            isNearItem = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Item_Product item = collision.GetComponent<Item_Product>();
        if (item != null && item == nearbyItem)
        {
            nearbyItem = null;
            isNearItem = false;
        }
        if (collision.CompareTag("Enemy") )
        {
            enemy.stunned = false;
        }
    }

    private void HitEnemy(GameObject target, bool stunned)
    {
        Debug.Log("Hit enemy");
        Character_Controller enemy_controller = target.GetComponent<Character_Controller>();

        if (enemy_controller.heldItem != null)
            Destroy(enemy_controller.heldItem);

        enemy_controller.stunned = true;
        enemy_controller.attackCol.enabled = false;
        Debug.Log("Stun enemy");
        enemy_controller.anim.SetBool("Stun", true);
        enemy_controller.anim.SetBool("Idle", false);
        
        enemy_controller.StartCoroutine(enemy_controller.StunCoroutine());
    }
    private IEnumerator StunCoroutine()
    {
        float stunDuration = 1f;

        yield return new WaitForSeconds(stunDuration);

        StunFalse();
    }

    public void StunFalse()
    {
        Debug.Log("Stun false");
        anim.SetBool("Stun", false);
        attackCol.enabled = true;
        anim.SetBool("Idle", true);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento
        netHeldItem.OnValueChanged -= OnNetHeldItemChanged;
    }
}
