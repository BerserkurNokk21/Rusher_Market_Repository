using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class LobbyData : NetworkBehaviour
{

    [Header("Data Strings")]
    public static string lobbyId;
    public static string lobby_joinCode;
    public string lobbyIdMostrar;

    public NetworkVariable<FixedString64Bytes> networkLobbyjoinCode = new NetworkVariable<FixedString64Bytes>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public NetworkVariable<FixedString64Bytes> networkLobbyDB_ID = new NetworkVariable<FixedString64Bytes>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        SetLobbyData(lobby_joinCode, lobbyId);
        lobbyIdMostrar = lobbyId;
    }

    public void SetLobbyData(string _lobby_joinCode, string _lobbyDB_ID)
    {
        networkLobbyDB_ID.Value = new FixedString64Bytes(_lobbyDB_ID);
        networkLobbyjoinCode.Value = new FixedString64Bytes(_lobby_joinCode);
        Debug.Log($"LobbyData sincronizado - LobbyId: {_lobby_joinCode}, DB_ID: {_lobbyDB_ID}");
    }

    // Métodos para obtener los valores
    public string GetLobbyId()
    {
        return networkLobbyjoinCode.Value.ToString();
    }

    public string GetLobbyDB_ID()
    {
        return networkLobbyDB_ID.Value.ToString();
    }
}