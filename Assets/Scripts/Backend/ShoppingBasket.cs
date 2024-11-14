using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingBasket : MonoBehaviour
{
    [SerializeField] private Character_Controller player;
    public Item_List item_List;


    private void Start()
    {
        item_List = FindObjectOfType<Item_List>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Character_Controller>();
        if (player != null && player.heldItem != null)
        {
            CheckItemList();
        }
    }

    private void CheckItemList()
    {
        Item_Product heldItem = player.heldItem.GetComponent<Item_Product>();
        if (heldItem != null)
        {
            Debug.Log(heldItem.selectedProduct);
            if (heldItem.selectedProduct != null)
            {
                foreach (Product product in item_List.products)
                {
                    if (product.id == heldItem.id)
                    {
                        Debug.Log("Producto encontrado en la lista de compras.");
                        DropItemInBasket();
                    }
                    else
                    {
                        Debug.LogWarning("Producto no encontrado: " + heldItem.id);
                    }
                }
            }
        }
    }

    public void DropItemInBasket()
    {
        if (player.heldItem != null)
        {
            Item_Product itemProduct = player.heldItem.GetComponent<Item_Product>();
            if (itemProduct != null)
            {
                // Llamar al método RemoveFromShoppingList en Item_Product
                itemProduct.RemoveFromShoppingList(itemProduct.selectedProduct);

                // Destruir el objeto en la escena
                Destroy(player.heldItem.gameObject);

                // Limpiar el ítem que el jugador sostiene
                player.heldItem = null;
            }
        }
    }
}
