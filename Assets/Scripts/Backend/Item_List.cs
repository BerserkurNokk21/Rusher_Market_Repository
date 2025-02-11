using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Networking;
using static JsonHelper;

public class Item_List : NetworkBehaviour
{
    public List<Product> products = new List<Product>();
    public List<Product> playerShoppingList = new List<Product>();
    public string playerId;
    public string gameID;
    public LobbyData _lobbyData;
    [SerializeField] private int listProductsLimit = 5;
    private bool productsLoaded = false;

    // Referencia al ShoppingListManager para a�adir el producto a la lista de compras
    private ShoppingListManager shoppingListManager;

    void Start()
    {
        shoppingListManager = FindObjectOfType<ShoppingListManager>();
        StartCoroutine(InitializeShoppingList());
    }

    // Coroutine para inicializar la lista de compras
    IEnumerator InitializeShoppingList()
    {
        // Primero obtener los productos de la base de datos
        yield return StartCoroutine(GetItemsFromDatabase());

        // Una vez que los productos se han cargado, a�adirlos a la lista de compras
        if (productsLoaded)
        {
            AddProductsToShoppingList();
            //yield return StartCoroutine(RegisterPlayerShoppingListInDatabase());
        }
    }

    // M�todo para obtener los productos desde la base de datos usando PHP
    IEnumerator GetItemsFromDatabase()
    {
        string uri = "http://localhost/unity_api/get_products.php";  // URL del script PHP local

        UnityWebRequest www = UnityWebRequest.Get(uri);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al obtener los productos: " + www.error);
        }
        else
        {
            // Procesar los datos recibidos en formato JSON
            string jsonResult = www.downloadHandler.text;
            Product[] productArray = JsonHelper.FromJson<Product>(jsonResult);

            // A�adir los productos a la lista
            products.AddRange(productArray);

            // Marcar como cargados
            productsLoaded = true;
        }
    }

    // M�todo para a�adir productos a la lista de la compra del jugador
    void AddProductsToShoppingList()
    {
        // Asegurarse de que no se agreguen m�s de los productos permitidos (listProductsLimit)
        int itemsAdded = 0;

        foreach (Product product in products)
        {
            // Obtener un producto aleatorio de la lista de productos
            Product randomProduct = products[Random.Range(0, products.Count)];

            if (itemsAdded >= listProductsLimit)
                break;

            // A�adir el producto aleatorio a la lista de compras si no est� ya en ella
            if (!playerShoppingList.Contains(randomProduct))
            {
                playerShoppingList.Add(randomProduct);
                shoppingListManager.AddItemToShoppingList(randomProduct.id, randomProduct.name);
                itemsAdded++;
            }
        }

        StartCoroutine(RegisterPlayerShoppingListInDatabase());

        // Vaciar la lista de compras del jugador para el siguiente jugador
        //playerShoppingList.Clear();

        if (itemsAdded == 0)
        {
            Debug.LogWarning("No se a�adieron productos a la lista de compras.");
        }
    }

    // M�todo para registrar la 'Shopping_List' en la base de datos
    public IEnumerator RegisterPlayerShoppingListInDatabase()
    {
        string uri = "http://localhost/unity_api/register_shopping_list.php";
        WWWForm form = new WWWForm();
        gameID = _lobbyData.networkLobbyDB_ID.Value.ToString();
        form.AddField("player_id", playerId);
        form.AddField("game_id", gameID);

        List<string> productIds = new List<string>();
        foreach (Product product in playerShoppingList)
        {
            productIds.Add(product.id);
        }

        string jsonProducts = JsonUtility.ToJson(new ProductListWrapper { products = productIds });
        form.AddField("products", jsonProducts);

        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al registrar la lista de compras: " + www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;

            try
            {
                JSONResponse response = JsonUtility.FromJson<JSONResponse>(jsonResponse);
                if (response.status == "success")
                {
                    foreach (var mapping in response.data)
                    {
                        StartCoroutine(RegisterTaskAction(mapping.shopping_list_id, mapping.product_id));
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error al parsear la respuesta JSON: " + e.Message);
            }
        }

    }


    // Coroutine para registrar en `Task_Actions`
    IEnumerator RegisterTaskAction(string shoppingListId, string productId)
    {
        string uri = "http://localhost/unity_api/register_task_actions.php";

        WWWForm form = new WWWForm();
        form.AddField("shopping_list_id", shoppingListId);
        form.AddField("product_id", productId);
        form.AddField("item_picked", "FALSE");

        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al registrar la acci�n de tarea: " + www.error);
        }
    }


    public bool AreProductsLoaded()
    {
        return productsLoaded;
    }
}
#region JSON Classes

// Clase para envolver la lista de productos
[System.Serializable]
public class ProductListWrapper
{
    public List<string> products;
}
// Clase para manejar la respuesta JSON del servidor
[System.Serializable]
public class ServerResponse
{
    public string message;
    public string error;
}

// Clases para manejar la respuesta JSON del servidor
[System.Serializable]
public class JSONResponse
{
    public string status;
    public string message;
    public List<ShoppingListMapping> data;
}

[System.Serializable]
public class ShoppingListMapping
{
    public string product_id;
    public string shopping_list_id;
}


#endregion