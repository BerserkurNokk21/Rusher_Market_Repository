using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShoppingListManager : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI donde se mostrar� la lista
    public TextMeshProUGUI shoppingListText;

    // Lista din�mica de la compra
    private List<string> shoppingList = new List<string>();

    // M�todo para agregar un �tem a la lista
    public void AddItemToShoppingList(string item)
    {
        shoppingList.Add(item);
        UpdateShoppingListUI();
    }

    // M�todo para eliminar un �tem de la lista (opcional, si lo necesitas en el futuro)
    public void RemoveItemFromShoppingList(string item)
    {
        if (shoppingList.Contains(item))
        {
            shoppingList.Remove(item);
            UpdateShoppingListUI();
        }
    }

    // M�todo para actualizar el texto de la UI con la lista actualizada
    private void UpdateShoppingListUI()
    {
        shoppingListText.text = "";  // Limpia el texto previo
        foreach (string item in shoppingList)
        {
            shoppingListText.text += "- " + item + "\n";  // Muestra cada �tem
        }
    }
}
