using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShoppingListManager : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI donde se mostrar� la lista
    public TextMeshProUGUI shoppingListText;

    // Lista din�mica de la compra (almacena pares ID-Nombre)
    private List<string> shoppingListIds = new List<string>();
    private List<string> shoppingListNames = new List<string>();

    // M�todo para agregar un �tem a la lista
    public void AddItemToShoppingList(string id, string name)
    {
        shoppingListIds.Add(id);
        shoppingListNames.Add(name);
        UpdateShoppingListUI();
    }

    // M�todo para verificar si un �tem ya est� en la lista de compras por su ID
    public bool ShoppingListContains(string id)
    {
        return shoppingListIds.Contains(id);
    }

    // M�todo para eliminar un �tem de la lista de compras
    public void RemoveItemFromShoppingList(string id)
    {
        int index = shoppingListIds.IndexOf(id);
        if (index != -1)
        {
            shoppingListIds.RemoveAt(index);
            shoppingListNames.RemoveAt(index);
            UpdateShoppingListUI();
        }
    }

    // M�todo para actualizar el texto de la UI con la lista actualizada
    private void UpdateShoppingListUI()
    {
        shoppingListText.text = "";  // Limpia el texto previo
        for (int i = 0; i < shoppingListNames.Count; i++)
        {
            shoppingListText.text += "- " + shoppingListNames[i] + "\n";  // Muestra cada �tem
        }
    }
}
