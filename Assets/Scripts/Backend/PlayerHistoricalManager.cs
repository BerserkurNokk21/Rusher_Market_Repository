using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHistoricalManager : MonoBehaviour
{
    public void RegisterPlayerHistory(string playerId)
    {
        string uri = "http://localhost/unity_api/register_player_historical.php";
        WWWForm form = new WWWForm();

        form.AddField("player_id", playerId);
        form.AddField("game_id", LobbyData.lobbyDB_ID);

        StartCoroutine(SendPlayerHistoricalData(uri, form));
    }

    private IEnumerator SendPlayerHistoricalData(string uri, WWWForm form)
    {
        UnityWebRequest www = UnityWebRequest.Post(uri, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error registering player historical: " + www.error);
        }
        else
        {
            Debug.Log("Player historical registered successfully");
        }
    }
}