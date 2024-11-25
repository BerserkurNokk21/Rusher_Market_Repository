using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PodiumManager : MonoBehaviour
{
    public List<Image> podiumIcons; 
    public List<Text> podiumNames; 
    public List<Text> podiumScores; 

    public Image fourthIcon; 
    public Text fourthName; 
    public Text fourthScore; 

    
    public void SetupPodium(List<PlayerScore> playerScores)
    {
        
        playerScores.Sort((x, y) => y.score.CompareTo(x.score));

        
        for (int i = 0; i < 3; i++)
        {
            podiumIcons[i].sprite = playerScores[i].icon;
            podiumNames[i].text = playerScores[i].name;
            podiumScores[i].text = playerScores[i].score.ToString();
        }

        
        fourthIcon.sprite = playerScores[3].icon;
        fourthName.text = playerScores[3].name;
        fourthScore.text = playerScores[3].score.ToString();
    }
    
}
