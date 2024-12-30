using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class GameManagerEndGame : NetworkBehaviour
{
    public PodiumManager podiumManager;

    [ServerRpc(RequireOwnership = false)]
    public void EndGameServerRpc()
    {
        List<PlayerScoreData> playerScores = new List<PlayerScoreData>();

        foreach (var playerObject in NetworkManager.Singleton.ConnectedClientsList)
        {
            PlayerScore playerScore = playerObject.PlayerObject.GetComponent<PlayerScore>();
            if (playerScore != null)
            {
                var data = new PlayerScoreData
                {
                    playerName = new FixedString128Bytes(playerScore.playerName.Value),
                    score = playerScore.score.Value
                };
                playerScores.Add(data);
            }
        }

        playerScores.Sort((a, b) => b.score.CompareTo(a.score));

        //SendScoresToPodiumClientRpc(playerScores.ToArray());
    }

    //[ClientRpc]
    //private void SendScoresToPodiumClientRpc(PlayerScoreData[] scores)
    //{
    //    if (IsClient && podiumManager != null)
    //    {
    //        // Convertir el arreglo a una lista para manipulación (opcional)
    //        List<PlayerScoreData> playerScores = new List<PlayerScoreData>(scores);

    //        // Pasar la lista convertida a arreglo
    //        podiumManager.SetupPodium(playerScores.ToArray());
    //    }
    //}


}
