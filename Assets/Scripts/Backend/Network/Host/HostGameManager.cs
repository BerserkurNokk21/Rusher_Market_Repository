using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager
{
    private const int MaxConnections = 10;
    private string joinCode;

	private Allocation allocation;
    private const string GameScene = "Game";

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

        RelayServerData relayServerData = new RelayServerData(allocation, "udp");
        transport.SetRelayServerData(relayServerData);

        //NetworkManager.Singleton.StartHost();

        //NetworkManager.Singleton.SceneManager.LoadScene(GameScene, LoadSceneMode.Single);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(GameScene, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            await Task.Delay(10);
        }

        NetworkManager.Singleton.StartHost();
    }
}
