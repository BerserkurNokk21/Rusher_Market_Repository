using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerScore : NetworkBehaviour
{
    public NetworkVariable<int> score = new NetworkVariable<int>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public NetworkVariable<FixedString128Bytes> playerName = new NetworkVariable<FixedString128Bytes>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public NetworkVariable<FixedString128Bytes> playerId = new NetworkVariable<FixedString128Bytes>(
        default,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public Sprite icon; // Esto puede ser sincronizado por otros medios si es necesario.

    public void SetPlayerData(string name, string id, int scoreValue)
    {
        if (IsServer)
        {
            playerName.Value = name;
            playerId.Value = id;
            score.Value = scoreValue;
        }
    }
}
