using System.Collections;
using TMPro;
using UnityEngine;

public class UpdatePlayerCanvasData : MonoBehaviour
{
    public PlayerDataList playerDataList;
    public TextMeshProUGUI playerPointsText;
    [SerializeField] private bool isPlayerInitialized = false;

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
                isPlayerInitialized = true;
            }

            // Espera un frame antes de volver a intentarlo
            yield return null;
        }
    }
}
