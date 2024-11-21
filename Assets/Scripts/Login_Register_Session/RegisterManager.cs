using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Para TextMeshPro

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField usernameField; // Campo de entrada de nombre de usuario
    public TMP_InputField passwordField; // Campo de entrada de contraseña
    public TextMeshProUGUI resultText; // Texto de resultado para mostrar mensajes

    public void StartRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", usernameField.text); // Añade nombre de usuario
        form.AddField("password", passwordField.text); // Añade contraseña

        // Cambia la URL para apuntar a tu script PHP de registro con PostgreSQL
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/unity_api/register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                resultText.text = "Error: " + www.error; // Manejo de errores de conexión
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log(responseText);

                if (responseText.Contains("success"))
                {
                    resultText.text = "Register successful!";
                }
                else if (responseText.Contains("duplicate"))
                {
                    resultText.text = "Register failed! Duplicated value";
                }
                else
                {
                    resultText.text = "Register failed!";
                }
            }
        }
    }
}
