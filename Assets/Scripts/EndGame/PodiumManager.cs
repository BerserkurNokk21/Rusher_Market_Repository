using UnityEngine;
using TMPro;
using static GameTimer;

public class PodiumManager : MonoBehaviour
{
    public TextMeshProUGUI[] podiumTexts;

    private void Start()
    {
        if (GameData.PlayerScores == null || GameData.PlayerScores.Length == 0)
        {
            Debug.LogError("No scores available in GameData.");
            return;
        }

        Debug.Log("Displaying scores on the podium.");
        for (int i = 0; i < podiumTexts.Length && i < GameData.PlayerScores.Length; i++)
        {
            podiumTexts[i].text = $"{GameData.PlayerScores[i].playerName}: {GameData.PlayerScores[i].score}";
        }
    }
}
