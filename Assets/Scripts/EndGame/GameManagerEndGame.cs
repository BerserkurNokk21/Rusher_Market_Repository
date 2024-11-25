using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEndGame : MonoBehaviour
{
    public PodiumManager podiumManager;


    void EndGame()
    {
        List<PlayerScore> players = new List<PlayerScore>();
        
            foreach (var player in players) 
        {
            players.Add(new PlayerScore(player.icon, player.name, player.score));
        }


        podiumManager.SetupPodium(players);
    }
    
}
