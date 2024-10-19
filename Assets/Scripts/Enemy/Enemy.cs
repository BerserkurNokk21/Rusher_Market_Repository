using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer sp;
    public bool stunned;
    // Start is called before the first frame update
    void Start()
    {
        sp= GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if(stunned) sp.color = Color.blue;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Item Dropped");
            sp.color = Color.blue;
            Debug.Log("Stunned");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sp.color = Color.white;
        }
    }
}
