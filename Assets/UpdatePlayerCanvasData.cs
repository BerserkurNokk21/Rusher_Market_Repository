using System.Collections;
using TMPro;
using UnityEngine;

public class UpdatePlayerCanvasData : MonoBehaviour
{
    public PlayerDataList playerDataList; // Referencia al script PlayerDataList
    public TextMeshProUGUI playerPointsText; // Referencia al TextMeshProUGUI para mostrar puntos
    [SerializeField] private bool isPlayerInitialized = false; // Indica si el jugador ha sido encontrado e inicializado

    void Start()
    {
        // Comienza un bucle para buscar al jugador hasta que sea inicializado
        StartCoroutine(WaitForPlayerInitialization());
    }

    void Update()
    {
        if (isPlayerInitialized && playerDataList != null && playerPointsText != null)
        {
            // Actualiza el texto con el valor de puntos del jugador
            playerPointsText.text = playerDataList.playerPoints.ToString();
        }
    }

    private IEnumerator WaitForPlayerInitialization()
    {
        while (playerDataList == null)
        {
            // Intenta encontrar el objeto PlayerDataList
            playerDataList = FindObjectOfType<PlayerDataList>();

            // Si se encuentra, marca como inicializado y rompe el bucle
            if (playerDataList != null)
            {
                Debug.Log("PlayerDataList encontrado e inicializado.");
                isPlayerInitialized = true;
            }

            // Espera un frame antes de volver a intentarlo
            yield return null;
        }
    }
}
