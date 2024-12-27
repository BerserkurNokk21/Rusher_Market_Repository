using Unity.Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameTimer : NetworkBehaviour
{
    [Header("Timer Settings")]
    public int initialTimeLeft = 300; // Tiempo inicial en segundos
    public TextMeshProUGUI timerText;
    private string sceneName = "Endgame_Test";

    // NetworkVariables para sincronización
    private NetworkVariable<int> networkTimeLeft = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private NetworkVariable<bool> networkIsTimerRunning = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        // Suscribir eventos para reflejar cambios en clientes
        networkTimeLeft.OnValueChanged += OnTimeChanged;
        networkIsTimerRunning.OnValueChanged += OnTimerStateChanged;

        // Actualizar la UI al iniciar
        OnTimeChanged(0, networkTimeLeft.Value);

        // Solo inicializamos la lógica si aún no está en ejecución
        if (!networkIsTimerRunning.Value && networkTimeLeft.Value == 0)
        {
            InitializeTimer();
        }
    }

    private void InitializeTimer()
    {
        networkTimeLeft.Value = initialTimeLeft;
        networkIsTimerRunning.Value = true;

        Debug.Log("Timer initialized and running.");
        StartCoroutine(TimerLoop());
    }

    private IEnumerator TimerLoop()
    {
        while (networkIsTimerRunning.Value && networkTimeLeft.Value > 0)
        {
            yield return new WaitForSeconds(1f); // Reduce el tiempo cada segundo
            networkTimeLeft.Value--;
        }

        if (networkTimeLeft.Value <= 0)
        {
            networkIsTimerRunning.Value = false;
            LoadGameOverSceneClientRpc();
        }
    }

    private void OnTimeChanged(int previousValue, int newValue)
    {
        // Actualizar la UI en todos los clientes
        if (timerText == null) return;

        int minutes = newValue / 60;
        int seconds = newValue % 60;
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnTimerStateChanged(bool previousState, bool newState)
    {
        Debug.Log($"Timer state changed: {newState}");
    }

    [ClientRpc]
    private void LoadGameOverSceneClientRpc()
    {
        Debug.Log("Loading endgame scene...");
        SceneManager.LoadScene(sceneName);
    }

    public void RestartTimer()
    {
        networkTimeLeft.Value = initialTimeLeft;
        networkIsTimerRunning.Value = true;

        Debug.Log($"Timer restarted with {initialTimeLeft} seconds");
        StartCoroutine(TimerLoop());
    }

    private void OnDestroy()
    {
        // Desuscribir eventos
        networkTimeLeft.OnValueChanged -= OnTimeChanged;
        networkIsTimerRunning.OnValueChanged -= OnTimerStateChanged;
    }
}
