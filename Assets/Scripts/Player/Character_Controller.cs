using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class Character_Controller : NetworkBehaviour
{
    [SerializeField] PlayerInputs _playerInputs;
    public float moveSpeed;
    public Rigidbody2D rb;

    [SerializeField] private bool stunned;
    [SerializeField] private float stunTime;
    [SerializeField] private Collider2D attackCol;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadius;

    [SerializeField] private Item_Product nearbyItem;
    [SerializeField] private bool isNearItem = false;

    [SerializeField] public Item_Product heldItem; // El ítem que el jugador está sosteniendo

    public Vector3 itemCarryOffset = new Vector3(0.5f, 0.5f, 0); // Offset del ítem respecto al jugador

    Vector2 moveDir;


    Enemy enemy;

    void Start()
    {
        _playerInputs = new PlayerInputs();
        _playerInputs.Enable();

        // Acción para coger el ítem
        _playerInputs.PlayerActions.PickItem.performed += ctx => PickUpItem();

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if(!IsOwner){
            return;
        }
        Hit();
        Move();
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
    }
    private void Hit()
    {
        if (_playerInputs.PlayerActions.Attack.triggered && heldItem==null)
        {
            Debug.Log("Attack");
            Collider2D[] enemigos = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, LayerMask.GetMask("Enemigos"));

            foreach(Collider2D enemigo in enemigos) {
                Character_Controller player = enemigo.GetComponent<Character_Controller>();

                if(player.NetworkObjectId != this.NetworkObjectId)
                {
                    HitEnemy(player.gameObject,stunned);
                }
            }
        }
    }

    IEnumerator Stun()
    {
        attackCol.enabled = false;
        yield return new WaitForSeconds(stunTime);
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
}
