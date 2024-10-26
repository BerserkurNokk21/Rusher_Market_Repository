using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Item_List : MonoBehaviour
{
    public List<Product> products = new List<Product>(); // Lista de productos obtenidos de la base de datos
    public List<Product> playerShoppingList = new List<Product>(); // Lista de productos que el jugador ha añadido
    public string playerId = "14db527a-44ff-4e43-bb3c-08997fb001ee"; // ID del jugador
    [SerializeField] private int listProductsLimit = 5; // Límite de productos
    private bool productsLoaded = false; // ¿Los productos han sido cargados?

    // Referencia al ShoppingListManager para añadir el producto a la lista de compras
    private ShoppingListManager shoppingListManager;

    void Start()
    {
        shoppingListManager = FindObjectOfType<ShoppingListManager>();
        StartCoroutine(InitializeShoppingList()); // Inicializar la lista de compras
    }

    // Coroutine para inicializar la lista de compras
    IEnumerator InitializeShoppingList()
    {
        // Primero obtener los productos de la base de datos
        yield return StartCoroutine(GetItemsFromDatabase());

        // Una vez que los productos se han cargado, añadirlos a la lista de compras
        if (productsLoaded)
        {
            AddProductsToShoppingList();
            yield return StartCoroutine(RegisterPlayerShoppingListInDatabase()); // Registrar la lista de compras en la base de datos
        }
        else
        {
            Debug.LogError("Los productos no pudieron ser cargados.");
        }
    }

    // Método para obtener los productos desde la base de datos usando PHP
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

            // Añadir los productos a la lista
            products.AddRange(productArray);

            // Marcar como cargados
            productsLoaded = true;

            Debug.Log("Productos obtenidos de la base de datos.");
        }
    }

    // Método para añadir productos a la lista de compras del jugador
    void AddProductsToShoppingList()
    {
        // Asegurarse de que no se agreguen más de los productos permitidos (listProductsLimit)
        int itemsAdded = 0;
        foreach (Product product in products)
        {
            if (itemsAdded >= listProductsLimit)
                break;

            if (!playerShoppingList.Contains(product))
            {
                playerShoppingList.Add(product);
                shoppingListManager.AddItemToShoppingList(product.id, product.name);
                itemsAdded++;
            }
        }

        if (itemsAdded == 0)
        {
            Debug.LogWarning("No se añadieron productos a la lista de compras.");
        }
    }

    // Método para registrar la lista de compras en la base de datos
    IEnumerator RegisterPlayerShoppingListInDatabase()
    {
        string uri = "http://localhost/unity_api/register_shopping_list.php";

        // Crear el formulario con el ID del jugador
        WWWForm form = new WWWForm();
        form.AddField("player_id", playerId);

        // Crear una lista de IDs de productos y convertirla a JSON
        List<string> productIds = new List<string>();
        foreach (Product product in playerShoppingList)
        {
            productIds.Add(product.id);
        }

        // Convertir la lista de IDs a JSON
        string jsonProducts = JsonUtility.ToJson(new ProductListWrapper { products = productIds });

        // Añadir el JSON al formulario
        form.AddField("products", jsonProducts);

        // Mostrar el JSON para verificar que está correcto
        Debug.Log("Enviando JSON de productos: " + jsonProducts);

        // Enviar la solicitud POST al servidor
        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al registrar la lista de compras: " + www.error);
        }
        else
        {
            Debug.Log("Lista de compras registrada correctamente para el jugador.");
            Debug.Log("Respuesta del servidor: " + www.downloadHandler.text); // Mostrar respuesta del servidor
        }
    }
    public bool AreProductsLoaded()
    {
        return productsLoaded;
    }
}

// Clase para envolver la lista de productos
[System.Serializable]
public class ProductListWrapper
{
    public List<string> products;
}
