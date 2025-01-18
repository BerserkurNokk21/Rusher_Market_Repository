using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private const int MaxConnections = 4;
    private string joinCode;

	private Allocation allocation;
    private string lobbyId;
    private const string GameScene = "ListManager_Tests";

    [System.Serializable]
    private class GameRegistrationResponse
    {
        public string status;
        public string message;
        public GameData data;
    }

    [System.Serializable]
    private class GameData
    {
        public string game_id;
    }

    public async Task StartHostAsync()
    {
		try
		{
			allocation = await Relay.Instance.CreateAllocationAsync(MaxConnections);

		}
		catch (Exception ex)
		{

			Debug.LogWarning(ex);
			return;
		}

        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Join code: " + joinCode);


        }
        catch (Exception ex)
        {

            Debug.LogWarning(ex);
            return;
        }
        

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        try
        {
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode
                        )
                }
            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync("Sample Lobby name", MaxConnections, lobbyOptions);

            lobbyId = lobby.Id;

            await RegisterLobby(lobbyId);


            HostSingleton.Instance.StartCoroutine(HeartbeatLobby(15));
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
            return;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            await Task.Delay(10);
        }

        NetworkManager.Singleton.StartHost();
    }

    private async Task RegisterLobby(string lobbyId)
    {
        string uri = "http://localhost/unity_api/game_registration.php";

        WWWForm form = new WWWForm();
        Debug.Log($"Enviando lobby_id: {lobbyId}"); // Debug del ID que enviamos
        form.AddField("lobby_id", lobbyId);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            try
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();


                
                string jsonResponse = www.downloadHandler.text;
                Debug.Log($"Respuesta completa del servidor: {jsonResponse}");

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error HTTP: {www.error}");
                    Debug.LogError($"Respuesta del servidor: {jsonResponse}");
                    return;
                }

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    Debug.LogError("La respuesta del servidor está vacía");
                    return;
                }

                try
                {
                    GameRegistrationResponse response = JsonUtility.FromJson<GameRegistrationResponse>(jsonResponse);

                    if (response != null && response.status == "success")
                    {
                        //LobbyData.SetLobbyData(lobbyId, response.data?.game_id);
                        LobbyData.lobby_joinCode = lobbyId;
                        LobbyData.lobbyId = response.data?.game_id;
                        Debug.Log($"Partida registrada exitosamente");
                        Debug.Log($"ID de partida: {response.data?.game_id}");
                        Debug.Log($"Mensaje: {response.message}");
                    }
                    else
                    {
                        Debug.LogError($"Error en la respuesta del servidor: {response?.message}");
                    }
                }
                catch (Exception jsonEx)
                {
                    Debug.LogError($"Error al parsear JSON: {jsonEx.Message}");
                    Debug.LogError($"JSON recibido: {jsonResponse}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error en el registro de la partida: {ex.Message}");
                throw;
            }
        }
    }

    private IEnumerator HeartbeatLobby(float waitTimeSeconds)
    {
        WaitForSecondsRealtime delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}
