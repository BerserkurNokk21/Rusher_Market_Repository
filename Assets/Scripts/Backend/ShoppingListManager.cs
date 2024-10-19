using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShoppingListManager : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI donde se mostrará la lista
    public TextMeshProUGUI shoppingListText;

    // Lista dinámica de la compra
    private List<string> shoppingList = new List<string>();

    // Método para agregar un ítem a la lista
    public void AddItemToShoppingList(string item)
    {
        shoppingList.Add(item);
        UpdateShoppingListUI();
    }

    // Método para eliminar un ítem de la lista (opcional, si lo necesitas en el futuro)
    public void RemoveItemFromShoppingList(string item)
    {
        if (shoppingList.Contains(item))
        {
            shoppingList.Remove(item);
            UpdateShoppingListUI();
        }
    }

    // Método para actualizar el texto de la UI con la lista actualizada
    private void UpdateShoppingListUI()
    {
        shoppingListText.text = "";  // Limpia el texto previo
        foreach (string item in shoppingList)
        {
            shoppingListText.text += "- " + item + "\n";  // Muestra cada ítem
        }
    }
}
