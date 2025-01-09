using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;
using static JsonHelper;

public class PlayerDataList : NetworkBehaviour
{
    public NetworkVariable<FixedString128Bytes> playerID = new NetworkVariable<FixedString128Bytes>();
    public NetworkVariable<FixedString128Bytes> playerNetworkName = new NetworkVariable<FixedString128Bytes>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server // Solo el servidor puede escribir
    );
    public NetworkVariable<float> playerPointsnetwork = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );


    public TextMeshProUGUI playerUsername;
    public float playerPoints;
    [SerializeField] public string id;
    [SerializeField] private string playerName;
    private Item_List itemListComponent;
    private PlayerHistoricalManager playerHistoricalManager;

    public override void OnNetworkSpawn()
    {
        SyncUpdatePlayerNameServerRpc(PlayerData.playerUsername, PlayerData.playerID, PlayerData.playerPoints);
        id = PlayerData.playerID;
        playerName = PlayerData.playerUsername;
        playerPoints = PlayerData.playerPoints;

        // Escuchar cambios en el nombre del jugador
        playerNetworkName.OnValueChanged += OnPlayerNameChanged;

        // Escuchar cambios en los puntos del jugador
        playerPointsnetwork.OnValueChanged += OnPlayerPointsChanged;

        OnPlayerPointsChanged(0, playerPointsnetwork.Value);

        // Actualizar la UI inicial si el nombre ya está sincronizado
        OnPlayerNameChanged(default, playerNetworkName.Value);

        // Llamar a la función para registrar la lista del jugador
        InitializePlayerShoppingList();

        // Llamar al script PlayerHistoricalManager y pasarle el id del jugador
        playerHistoricalManager = FindObjectOfType<PlayerHistoricalManager>();
        if (playerHistoricalManager != null)
        {
            playerHistoricalManager.RegisterPlayerHistory(id);
        }
        else
        {
            Debug.LogError("PlayerHistoricalManager script not found in the scene!");
        }
    }

    private void OnPlayerNameChanged(FixedString128Bytes oldValue, FixedString128Bytes newValue)
    {
        // Actualizar la UI con el nuevo nombre
        if (playerUsername != null)
        {
            playerUsername.text = newValue.ToString();
        }
    }

    [ServerRpc]
    private void SyncUpdatePlayerNameServerRpc(string _playerName, string _playerId, float _points)
    {
        playerNetworkName.Value = new FixedString128Bytes(_playerName);
        playerID.Value = new FixedString128Bytes(_playerId);
        id = _playerId;
        playerName = _playerName;
        playerPoints = _points;
    }

    private void InitializePlayerShoppingList()
    {
        // Obtener el componente Item_List en la escena
        if (itemListComponent == null)
        {
            itemListComponent = FindObjectOfType<Item_List>();
        }

        if (itemListComponent != null)
        {
            itemListComponent.playerId = PlayerData.playerID;
        }
        else
        {
            Debug.LogError("Item_List component not found in the scene!");
        }
    }

    public void AddPoints(float points)
    {
        if (IsOwner)
        {
            Debug.Log($"Player {id} adding points: {points}");
            SyncUpdatePointsServerRpc(points);
        }
    }

    [ServerRpc]
    private void SyncUpdatePointsServerRpc(float points)
    {
        Debug.Log($"Server received points update for player {id}: {points}");
        playerPointsnetwork.Value += points;

        // Propagar los puntos a todos los clientes
        UpdatePointsClientRpc(playerPointsnetwork.Value);
    }
    [ClientRpc]
    private void UpdatePointsClientRpc(float newPoints)
    {
        if (!IsOwner) return; // Solo actualizar si es nuestro jugador

        Debug.Log($"Client {id} received points update: {newPoints}");
        playerPoints = newPoints;
    }
    private void OnPlayerPointsChanged(float previousValue, float newValue)
    {
        if (!IsOwner) return; // Solo actualizar si es nuestro jugador

        Debug.Log($"Points changed for player {id}: {previousValue} -> {newValue}");
        playerPoints = newValue;
    }
    private void OnDestroy()
    {
        if (playerPointsnetwork != null)
            playerPointsnetwork.OnValueChanged -= OnPlayerPointsChanged;
        if (playerNetworkName != null)
            playerNetworkName.OnValueChanged -= OnPlayerNameChanged;
    }

    public float GetCurrentPoints()
    {
        return playerPointsnetwork.Value;
    }
}
