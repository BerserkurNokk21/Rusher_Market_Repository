using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using System;

public class GameTimer : NetworkBehaviour
{
    [Header("Timer Settings")]
    public int initialTimeLeft = 300; // Tiempo inicial en segundos
    public TextMeshProUGUI timerText;
    private string sceneName = "Endgame_Test";
    public Item_List item_list;

    // NetworkVariables para sincronización
    private NetworkVariable<int> networkTimeLeft = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private NetworkVariable<bool> networkIsTimerRunning = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        // Suscribir eventos para reflejar cambios en clientes
        networkTimeLeft.OnValueChanged += OnTimeChanged;
        networkIsTimerRunning.OnValueChanged += OnTimerStateChanged;

        // Actualizar la UI al iniciar
        OnTimeChanged(0, networkTimeLeft.Value);

        // Solo inicializamos la lógica si aún no está en ejecución
        if (!networkIsTimerRunning.Value && networkTimeLeft.Value == 0)
        {
            InitializeTimer();
        }
    }

    private void InitializeTimer()
    {
        networkTimeLeft.Value = initialTimeLeft;
        networkIsTimerRunning.Value = true;

        Debug.Log("Timer initialized and running.");
        StartCoroutine(TimerLoop());
    }

    private IEnumerator TimerLoop()
    {
        while (networkIsTimerRunning.Value && networkTimeLeft.Value > 0)
        {
            yield return new WaitForSeconds(1f);
            networkTimeLeft.Value--;
        }

        if (networkTimeLeft.Value <= 0)
        {
            networkIsTimerRunning.Value = false;
            CollectPlayerScoresServerRpc(); // Recopila puntuaciones al terminar el tiempo
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CollectPlayerScoresServerRpc()
    {
        var scores = new List<PlayerScoreData>();
        string idChampion = "";

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            var playerObject = client.PlayerObject;
            if (playerObject != null && playerObject.TryGetComponent(out PlayerDataList playerData))
            {
                scores.Add(new PlayerScoreData
                {
                    playerName = playerData.playerNetworkName.Value.ToString(),
                    playerId = playerData.playerID.Value.ToString(),
                    score = playerData.playerPoints
                });
                Debug.Log("Player score collected: " + playerData.playerPoints);
            }
        }

        // Ordenar las puntuaciones de mayor a menor
        scores.Sort((a, b) => b.score.CompareTo(a.score));

        idChampion = scores[0].playerId.ToString();

        SendIdChampion(idChampion);
        // Enviar las puntuaciones al cliente
        SendScoresToPodiumClientRpc(scores.ToArray());
    }


    [ClientRpc]
    private void SendScoresToPodiumClientRpc(PlayerScoreData[] scores)
    {
        if (scores == null || scores.Length == 0)
        {
            Debug.LogError("Scores array is null or empty. No scores were sent to the podium.");
            return;
        }
        foreach (var score in scores)
        {
            Debug.Log($"Player: {score.playerName}, Score: {score.score}, Id{score.playerId}");
        }

        // Almacenar los datos en la clase estática
        GameData.PlayerScores = scores;

        // Cargar la escena del podio
        SceneManager.LoadScene(sceneName);
        Debug.Log("Endgame scene loading initiated.");
    }



    private void OnTimeChanged(int previousValue, int newValue)
    {
        // Actualizar la UI en todos los clientes
        if (timerText == null) return;

        int minutes = newValue / 60;
        int seconds = newValue % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnTimerStateChanged(bool previousState, bool newState)
    {
        Debug.Log($"Timer state changed: {newState}");
    }

    public void RestartTimer()
    {
        networkTimeLeft.Value = initialTimeLeft;
        networkIsTimerRunning.Value = true;

        Debug.Log($"Timer restarted with {initialTimeLeft} seconds");
        StartCoroutine(TimerLoop());
    }


    private void SendIdChampion(string idChampion)
    {
        string uri = "http://localhost/unity_api/update_game_champion.php";
        WWWForm form = new WWWForm();

        form.AddField("game_id", LobbyData.lobbyDB_ID);
        //form.AddField("time_played", initialTimeLeft);
        form.AddField("player_champion", idChampion);

        StartCoroutine(SendChampionData(uri, form));
    }

    private IEnumerator SendChampionData(string uri, WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error updating champion: " + www.error);
        }
        else
        {
            Debug.Log("Champion updated successfully");
        }
    }

    private void OnDestroy()
    {
        // Desuscribir eventos
        networkTimeLeft.OnValueChanged -= OnTimeChanged;
        networkIsTimerRunning.OnValueChanged -= OnTimerStateChanged;
    }

    #region
    public static class GameData
    {
        public static PlayerScoreData[] PlayerScores { get; set; }
    }

    #endregion
}
