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
    [SerializeField] public Item_Product heldItem; // El �tem que el jugador est� sosteniendo
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

    [SerializeField] private bool stunned;
    [SerializeField] private float stunTime = 2f;
    [SerializeField] private float attackRadius;
    [SerializeField] private bool isNearItem = false;
    public float moveSpeed;
    public Vector3 itemCarryOffset = new Vector3(0.5f, 0.5f, 0); // Offset del �tem respecto al jugador
    public Vector2 moveDir;





    void Start()
    {
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();
        // Acci�n para coger el �tem
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



    void Update()
    {
        if (!IsOwner){
            return;
        }
        Move();
        Hit();
        if (heldItem != null)
        {
            heldItem.transform.position = transform.position + itemCarryOffset;
        }
    }
    void OnFlipChanged(bool previousValue, bool newValue)
    {
        sp.flipX = newValue;
        // Actualizar la posici�n del punto de ataque
        attackPos.localPosition = new Vector3(newValue ? -Mathf.Abs(attackPos.localPosition.x) : Mathf.Abs(attackPos.localPosition.x),
            attackPos.localPosition.y,
            attackPos.localPosition.z);
    }
    void Move()
    {
        if (!IsOwner) return;

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
    void PickUpItem()
    {
        if (isNearItem && nearbyItem != null && heldItem == null)
        {
            heldItem = nearbyItem;
        }
    }

    void DropItem()
    {
        if (heldItem!=null)
        {
            Destroy(heldItem.gameObject);
        }
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
}
