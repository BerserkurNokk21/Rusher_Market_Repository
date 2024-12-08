using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerDataList : NetworkBehaviour
{
    public string playerID;
    public string playerName;
    public TextMeshProUGUI playerUsername;

    //Dejar por el momento, posible uso futuro
    public float playerPoints;
    private void Start()
    {
        playerID = JsonHelper.PlayerData.playerID;
        playerName = JsonHelper.PlayerData.playerUsername;
        playerUsername.text = playerName;
    }


    public void AddPoints(float points)
    {
        playerPoints += points;
    }
}
