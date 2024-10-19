using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_List : MonoBehaviour
{
    // Nombre del ítem (asignado en el Inspector)
    public List<string> itemName;
    private string itemSelected;
    // Referencia al ShoppingListManager
    private ShoppingListManager shoppingListManager;

    void Start()
    {
        // Busca el objeto ShoppingListManager en la escena y lo guarda en la variable
        shoppingListManager = FindObjectOfType<ShoppingListManager>();
        AddItemToShoppingList();
    }

    // Método que se llamará cuando el jugador interactúe con el ítem
    public void AddItemToShoppingList()
    {
        itemSelected = itemName[Random.Range(0, itemName.Count)];

        shoppingListManager.AddItemToShoppingList(itemSelected);

    }

    // Método para eliminar este ítem de la lista de la compra
    public void RemoveFromShoppingList()
    {
        shoppingListManager.RemoveItemFromShoppingList(itemSelected);
    }
}
