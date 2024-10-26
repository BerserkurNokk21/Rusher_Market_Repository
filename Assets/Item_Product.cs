using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Product : MonoBehaviour
{
    public string id = "17d9839d-cb75-41ac-8ebc-69bc31dc1546"; // ID del producto asignado en el editor
    [SerializeField] private string productName; // Nombre del producto que será asignado automáticamente
    public Item_List item_List; // Referencia al script Item_List
    public Product selectedProduct; // Producto seleccionado

    // Referencia al ShoppingListManager para añadir el producto a la lista de compras
    private ShoppingListManager shoppingListManager;

    void Start()
    {
        shoppingListManager = FindObjectOfType<ShoppingListManager>();
        item_List = FindObjectOfType<Item_List>();

        // Intentar asignar el nombre del producto basándonos en el ID
        StartCoroutine(WaitForProductsAndAssignName());
    }

    // Coroutine para esperar hasta que los productos estén cargados
    IEnumerator WaitForProductsAndAssignName()
    {
        // Esperar hasta que los productos estén cargados
        while (!item_List.AreProductsLoaded())
        {
            yield return null; // Espera hasta el siguiente frame
        }

        // Ahora que los productos están cargados, asigna el nombre
        AssignProductNameById();
    }

    // Método para asignar el nombre del producto basado en el ID
    public void AssignProductNameById()
    {
        if (item_List != null && item_List.products.Count > 0)
        {
            foreach (Product product in item_List.products)
            {
                // Verifica si el ID del producto coincide con el ID asignado en el editor
                if (product.id == id)
                {
                    selectedProduct = product; // Asigna el producto seleccionado
                    productName = product.name; // Asigna el nombre del producto
                    return; // Sale del loop una vez encontrado
                }
                else
                {
                    Debug.LogWarning("Producto no encontrado: " + id);
                }
            }
        }
        else
        {
            Debug.LogWarning("No hay productos en la lista.");
        }
    }
    public void RemoveFromShoppingList(Product product)
    {
        if (product != null)
        {
            // Remover el producto de la lista de compras en el ShoppingListManager
            ShoppingListManager shoppingListManager = FindObjectOfType<ShoppingListManager>();
            shoppingListManager.RemoveItemFromShoppingList(product.id);  // Usar el ID del producto
            Debug.Log("Producto eliminado de la lista de compras: " + product.name);
        }
    }

}
