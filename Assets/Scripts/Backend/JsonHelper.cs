using System.Collections.Generic;
using UnityEngine;
// Ayuda para convertir el JSON a array
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
    [System.Serializable]
    public class LoginResponse
    {
        public string status;
        public string player_id;
        public string username;
    }
    // Clase para el JSON de la lista de jugadores
    public static class PlayerData
    {
        public static string playerID;
        public static string playerUsername;
    }

    public static class Lobbydata
    {
        public static string lobbyId;
        public static string lobbyDB_ID;
    }
}
