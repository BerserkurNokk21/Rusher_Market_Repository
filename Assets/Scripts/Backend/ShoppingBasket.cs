using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShoppingBasket : MonoBehaviour
{
    [SerializeField] private Character_Controller player;
    public Item_List item_List;
    public TextMeshProUGUI itemText;


    private void Start()
    {
        item_List = FindObjectOfType<Item_List>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        player = collision.GetComponent<Character_Controller>();
        if (player != null && player.heldItem != null)
        {
            CheckItemList();
        }
    }

    private void CheckItemList()
    {
        Item_Product heldItem = player.heldItem.GetComponent<Item_Product>();
        if (heldItem != null)
        {
            Debug.Log(heldItem.selectedProduct);
            if (heldItem.selectedProduct != null)
            {
                foreach (Product product in item_List.playerShoppingList)
                {
                    if (product.id == heldItem.id)
                    {
                        itemText.text = "Product found in the shopping list";
                        StartCoroutine(ClearItemText());
                        DropItemInBasket();
                    }
                }
            }
        }
    }

    private IEnumerator ClearItemText()
    {
        yield return new WaitForSeconds(1f);
        itemText.text = "";
    }

    public void DropItemInBasket()
    {
        if (player.heldItem != null)
        {
            Item_Product itemProduct = player.heldItem.GetComponent<Item_Product>();
            if (itemProduct != null)
            {
                AddPoints(itemProduct.points);
                // Destruir el objeto en la escena
                Destroy(player.heldItem.gameObject);

                // Limpiar el ítem que el jugador sostiene
                player.heldItem = null;
            }
        }
    }

    public void AddPoints(float points)
    {
        PlayerDataList playerDataList = FindObjectOfType<PlayerDataList>();

        if (playerDataList != null)
        {
            playerDataList.AddPoints(points);
        }
    }
}
