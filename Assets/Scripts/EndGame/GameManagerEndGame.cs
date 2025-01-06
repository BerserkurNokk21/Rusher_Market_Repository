using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManagerEndGame : NetworkBehaviour
{
    public PodiumManager podiumManager;
    private string idChampion;
    private string sceneName = "Endgame_Test";

    

    public string GetChampionId()
    {
        return idChampion;
    }
}