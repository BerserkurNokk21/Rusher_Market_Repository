using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] PlayerInputs _playerInputs;  
    public float moveSpeed;
    public Rigidbody2D rb;

    Vector2 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        _playerInputs = new();
        _playerInputs.Enable();
        
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        moveDir = _playerInputs.PlayerActions.Mover.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveDir.x*moveSpeed, moveDir.y*moveSpeed);
    }
}
