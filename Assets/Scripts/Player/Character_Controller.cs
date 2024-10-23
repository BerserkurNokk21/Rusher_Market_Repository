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

    [SerializeField] private Item_List nearbyItem;
    [SerializeField] private bool isNearItem = false;

    [SerializeField] public Item_List heldItem; // El ítem que el jugador está sosteniendo

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
        Move();
        if (heldItem != null)
        {
            heldItem.transform.position = transform.position + itemCarryOffset;
        }

        if(!IsOwner){
            return;
        }
    }

    void Move()
    {
        moveDir = _playerInputs.PlayerActions.Mover.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

    void PickUpItem()
    {
        if (isNearItem && nearbyItem != null && heldItem == null)
        {
            heldItem = nearbyItem;
            Debug.Log("Picked up item: " + heldItem.itemName);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item_List item = collision.GetComponent<Item_List>();
        if (item != null)
        {
            nearbyItem = item;
            isNearItem = true;
        }

        if(collision.CompareTag("Enemy")&& _playerInputs.PlayerActions.Attack.triggered){
            enemy.stunned = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Item_List item = collision.GetComponent<Item_List>();
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
}
