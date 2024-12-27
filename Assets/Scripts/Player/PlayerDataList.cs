using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;
using static JsonHelper;

public class PlayerDataList : NetworkBehaviour
{
    private NetworkVariable<FixedString128Bytes> playerID = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> playerNetworkName = new NetworkVariable<FixedString128Bytes>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server // Solo el servidor puede escribir
    );

    [SerializeField] private string id;
    [SerializeField] private string playerName;
    public TextMeshProUGUI playerUsername;
    public float playerPoints;

    private Item_List itemListComponent;

    public override void OnNetworkSpawn()
    {
        if (IsOwner) // Solo el propietario solicita el nombre al servidor
        {
            SyncUpdatePlayerNameServerRpc(PlayerData.playerUsername, PlayerData.playerID);
            id = PlayerData.playerID;
            playerName = PlayerData.playerUsername;
        }

        // Escuchar cambios en el nombre del jugador
        playerNetworkName.OnValueChanged += OnPlayerNameChanged;

        // Actualizar la UI inicial si el nombre ya está sincronizado
        OnPlayerNameChanged(default, playerNetworkName.Value);

        // Llamar a la función para registrar la lista del jugador
        StartCoroutine(InitializePlayerShoppingList());
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
    private void SyncUpdatePlayerNameServerRpc(string _playerName, string _playerId)
    {
        playerNetworkName.Value = new FixedString128Bytes(_playerName);
        playerID.Value = new FixedString128Bytes(_playerId);
        id = _playerId;
        playerName = _playerName;
    }

    private IEnumerator InitializePlayerShoppingList()
    {
        // Obtener el componente Item_List en la escena
        if (itemListComponent == null)
        {
            itemListComponent = FindObjectOfType<Item_List>();
        }

        if (itemListComponent != null)
        {
            // Llamamos a la función de Item_List para registrar la lista del jugador
            yield return StartCoroutine(itemListComponent.RegisterPlayerShoppingListInDatabase(PlayerData.playerID));
            Debug.Log("Player shopping list initialized for ID: " + PlayerData.playerID);
        }
        else
        {
            Debug.LogError("Item_List component not found in the scene!");
        }
    }

    public void AddPoints(float points)
    {
        if (!IsOwner)
            return;

        playerPoints += points;
    }

    private void OnDestroy()
    {
        // Limpiar el evento para evitar errores de referencia nula
        playerNetworkName.OnValueChanged -= OnPlayerNameChanged;
    }
}
