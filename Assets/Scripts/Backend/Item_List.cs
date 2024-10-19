using UnityEngine;

public class Item_List : MonoBehaviour
{
    // Nombre del �tem (asignado en el Inspector)
    public string itemName;

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
        shoppingListManager.AddItemToShoppingList(itemName);
    }

    // M�todo para eliminar este �tem de la lista de la compra
    public void RemoveFromShoppingList()
    {
        shoppingListManager.RemoveItemFromShoppingList(itemName);
    }
}
