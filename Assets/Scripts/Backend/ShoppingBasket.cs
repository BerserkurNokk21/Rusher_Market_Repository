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
            // Asegurarse de que el ítem tiene el componente Item_Product
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
