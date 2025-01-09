using System.Collections;
using TMPro;
using UnityEngine;

public class UpdatePlayerCanvasData : MonoBehaviour
{
    public PlayerDataList playerDataList;
    public TextMeshProUGUI playerPointsText;
    [SerializeField] private bool isPlayerInitialized = false;
    private float currentPoints = 0f;

    void Start()
    {
        StartCoroutine(WaitForPlayerInitialization());
    }

    void Update()
    {
        if (isPlayerInitialized && playerDataList != null && playerPointsText != null)
        {
            // Actualizamos solo si hay un cambio en los puntos
            float newPoints = playerDataList.playerPointsnetwork.Value;
            if (currentPoints != newPoints)
            {
                currentPoints = newPoints;
                UpdatePointsDisplay();
            }
        }
    }

    private void UpdatePointsDisplay()
    {
        if (playerPointsText != null)
        {
            playerPointsText.text = currentPoints.ToString("F0"); // F0 para mostrar sin decimales
        }
    }

    private IEnumerator WaitForPlayerInitialization()
    {
        while (playerDataList == null)
        {
            playerDataList = FindObjectOfType<PlayerDataList>();

            if (playerDataList != null)
            {
                isPlayerInitialized = true;
                // Suscribimos al evento de cambio de puntos
                playerDataList.playerPointsnetwork.OnValueChanged += OnPointsChanged;
            }

            yield return null;
        }
    }

    private void OnPointsChanged(float oldValue, float newValue)
    {
        currentPoints = newValue;
        UpdatePointsDisplay();
    }

    private void OnDestroy()
    {
        if (playerDataList != null && playerDataList.playerPointsnetwork != null)
        {
            playerDataList.playerPointsnetwork.OnValueChanged -= OnPointsChanged;
        }
    }
}