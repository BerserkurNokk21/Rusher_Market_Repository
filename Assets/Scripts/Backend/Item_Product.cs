using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Product : MonoBehaviour
{
    //Asignamos el id del producto que queremos
    public string id;
    [SerializeField] private string productName;
    public Item_List item_List;
    public Product selectedProduct;
    public float points;

    private ShoppingListManager shoppingListManager;

    void Start()
    {
        shoppingListManager = FindObjectOfType<ShoppingListManager>();
        item_List = FindObjectOfType<Item_List>();

        StartCoroutine(WaitForProductsAndAssignName());
    }

    IEnumerator WaitForProductsAndAssignName()
    {
        while (!item_List.AreProductsLoaded())
        {
            yield return null;
        }

        AssignProductNameById();
    }
    public void AssignProductNameById()
    {
        if (item_List != null && item_List.products.Count > 0)
        {
            foreach (Product product in item_List.products)
            {
                if (product.id == id)
                {
                    selectedProduct = product;
                    productName = product.name;
                    return;
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
    //public void RemoveFromShoppingList(Product product)
    //{
    //    if (product != null)
    //    {
    //        ShoppingListManager shoppingListManager = FindObjectOfType<ShoppingListManager>();
    //        shoppingListManager.RemoveItemFromShoppingList(product.id);
    //        Debug.Log("Producto eliminado de la lista de compras: " + product.name);
    //    }
    //}

}
