using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingBasket : MonoBehaviour
{
    [SerializeField] private Character_Controller player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Character_Controller>();
        if (player != null && player.heldItem != null)
        {
            DropItemInBasket();
        }
    }
    public void DropItemInBasket()
    {
        if (player.heldItem != null)
        {
            Debug.Log("Dropped item in basket: " + player.heldItem.itemName);
            player.heldItem.RemoveFromShoppingList();
            Destroy(player.heldItem.gameObject);
            player.heldItem = null;
        }
    }
}
