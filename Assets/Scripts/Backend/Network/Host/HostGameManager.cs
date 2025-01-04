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
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private const int MaxConnections = 4;
    private string joinCode;

	private Allocation allocation;
    private string lobbyId;
    private const string GameScene = "ListManager_Tests";

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
