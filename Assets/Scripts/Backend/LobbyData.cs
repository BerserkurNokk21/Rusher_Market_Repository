using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JsonHelper;

public class LobbyData : MonoBehaviour
{
    public static string lobbyId;
    public static string lobbyDB_ID;


    public static void SetLobbyData(string _lobbyId, string _lobbyDB_ID)
    {
        lobbyId = _lobbyId;
        lobbyDB_ID = _lobbyDB_ID;
    }
}
