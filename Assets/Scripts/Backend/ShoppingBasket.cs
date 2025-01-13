using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ShoppingBasket : NetworkBehaviour
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
        if (heldItem != null && heldItem.selectedProduct != null)
        {
            foreach (Product product in item_List.playerShoppingList)
            {
                if (product.id == heldItem.id)
                {
                    itemText.text = "Product found in the shopping list";
                    StartCoroutine(ClearItemText());
                    DropItemInBasket();
                    break;
                }
            }
        }
    }

    private IEnumerator ClearItemText()
    {
        yield return new WaitForSeconds(1f);
        itemText.text = "";
    }

    private void DropItemInBasket()
    {
        if (player.heldItem != null)
        {
            Item_Product itemProduct = player.heldItem.GetComponent<Item_Product>();
            if (itemProduct != null)
            {
                AddPoints(itemProduct.points);
                
                // En lugar de usar Destroy, llamamos al método del jugador para soltar el ítem
                // que ya tiene la lógica de red implementada
                player.DropItem();
            }
        }
    }

    public void AddPoints(float points)
    {
        // Obtener el PlayerDataList asociado al jugador que colisionó
        PlayerDataList playerDataList = player.GetComponent<PlayerDataList>();

        if (playerDataList != null)
        {
            Debug.Log($"Adding points to player {playerDataList.id}: {points}");
            playerDataList.AddPoints(points);
        }
        else
        {
            Debug.LogError("PlayerDataList not found on the player!");
        }
    }
}
