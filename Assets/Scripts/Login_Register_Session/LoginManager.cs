using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;
using static JsonHelper;
using Unity.Netcode;

public class LoginManager : NetworkBehaviour
{
    private string mainMenuScene = "NetBootstrap";
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI resultText;

    public void StartLogin()
    {
        StartCoroutine(Login());
    }

    IEnumerator Login()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text); // Añade nombre de usuario
        form.AddField("password", passwordField.text); // Añade contraseña

        // Cambia la URL para apuntar a tu script PHP de login con PostgreSQL
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                resultText.text = "Error: " + www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;

                try
                {
                    GetPlayerData(responseText);
                }
                catch (System.Exception e)
                {

                    resultText.text = "Error al procesar la respuesta: " + e.Message;
                }
            }
        }
    }

    private void GetPlayerData(string responseText)
    {
        LoginResponse response = JsonUtility.FromJson<LoginResponse>(responseText);

        if (response.status == "success")
        {
            PlayerData.playerID = response.player_id;
            PlayerData.playerUsername = response.username;

            SceneManager.LoadScene(mainMenuScene);
        }
        else
        {
            resultText.text = "Login failed!";
        }
    }
}
