using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_List : MonoBehaviour
{
    // Nombre del �tem (asignado en el Inspector)
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

    // M�todo que se llamar� cuando el jugador interact�e con el �tem
    public void AddItemToShoppingList()
    {
        itemSelected = itemName[Random.Range(0, itemName.Count)];

        shoppingListManager.AddItemToShoppingList(itemSelected);

    }

    // M�todo para eliminar este �tem de la lista de la compra
    public void RemoveFromShoppingList()
    {
        shoppingListManager.RemoveItemFromShoppingList(itemSelected);
    }
}
