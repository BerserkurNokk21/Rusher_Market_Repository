using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDataList : NetworkBehaviour
{
    public string playerID;
    public string playerName;

    private void Start()
    {
        playerID = JsonHelper.PlayerData.playerID;
        playerName = JsonHelper.PlayerData.playerUsername;
    }
}
