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
    [SerializeField] private bool stunned;
    [SerializeField] private float stunTime;
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
        if (stunned)
        {
            StartCoroutine("Stun");
        }

        

    }

    void Move()
    {
        moveDir = _playerInputs.PlayerActions.Mover.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
        if (moveDir.x >= 0.1f || moveDir.y >= 0.1 || moveDir.x <= -0.1 || moveDir.y <= -0.1)
        {
            anim.SetBool("Move", true);
            anim.SetBool("Idle", false);
            if (moveDir.x <= -0.1 || moveDir.y <= -0.1)
            {
                sp.flipX = true;
                // Girar la posición del punto de ataque
                attackPos.localPosition = new Vector3(-Mathf.Abs(attackPos.localPosition.x), attackPos.localPosition.y, attackPos.localPosition.z);
            }
            else
            {
                sp.flipX = false;
                // Restaurar la posición original del punto de ataque
                attackPos.localPosition = new Vector3(Mathf.Abs(attackPos.localPosition.x), attackPos.localPosition.y, attackPos.localPosition.z);
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
                Character_Controller player = enemigo.GetComponent<Character_Controller>();
                HitEnemy(player.gameObject, stunned);
            }
            StartCoroutine("FinishAttack");

        }
    }
    IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Hit", true);
        anim.SetBool("Idle", false);
    }

    IEnumerator Stun()
    {
        attackCol.enabled = false;
        anim.SetBool("Stun", true);
        anim.SetBool("Idle", false);
        yield return new WaitForSeconds(stunTime);
        anim.SetBool("Stun",false);
        anim.SetBool("Idle", true);
        attackCol.enabled = true;
        StopCoroutine("Stun");
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
        target.GetComponent<Character_Controller>().stunned = true;
        target.GetComponent<Character_Controller>().anim.SetBool("Stun", true);
        target.GetComponent<Character_Controller>().SetHitFalse();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }

    public void SetHitFalse()
    {
        anim.SetBool("Hit",false);
    }
}
