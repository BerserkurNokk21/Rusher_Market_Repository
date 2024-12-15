using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using static JsonHelper;

public class MainMenu : NetworkBehaviour
{
    public NetworkVariable<string> playerName = new NetworkVariable<string>();
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private TextMeshProUGUI playerUsername;

    public void Start()
    {
        playerName.Value = PlayerData.playerUsername;
        playerUsername.text = playerName.Value;
    }

    public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();
    }

    public async void StartClient()
    {
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeInput.text);
    }
}
