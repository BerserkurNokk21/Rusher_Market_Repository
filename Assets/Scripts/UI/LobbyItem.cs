using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Services.Lobbies.Models;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private TMP_Text lobbyPlayersText;
    private LobbysList lobbiesList;
    private Lobby lobby;
    public void Initialise(LobbysList lobbiesList, Lobby lobby)
    {
        this.lobbiesList = lobbiesList;
        this.lobby = lobby;
        lobbyNameText.text = lobby.Name;
        lobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    public void JoinLobby()
    {
        lobbiesList.JoinAsync(lobby);
    }
}
