using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPointConfig
    {
        public Transform spawnPoint; // Punto de spawn
        public GameObject itemPrefab; // �tem que aparecer� en este punto
    }

    [SerializeField] private List<SpawnPointConfig> spawnPointConfigs; // Configuraci�n de puntos de spawn
    public float spawnInterval = 5.0f; // Intervalo entre spawns
    public float initialSpawnDelay = 2.0f; // Retraso inicial antes del primer spawn

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    // Corrutina para instanciar los �tems en sus puntos asignados
    IEnumerator SpawnItems()
    {
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            foreach (SpawnPointConfig config in spawnPointConfigs)
            {
                if (config.itemPrefab != null && config.spawnPoint != null)
                {
                    // Instanciar el �tem asignado en el punto correspondiente
                    Instantiate(config.itemPrefab, config.spawnPoint.position, config.spawnPoint.rotation);
                }
                else
                {
                    Debug.LogWarning("Un punto de spawn o un prefab no est�n asignados en la configuraci�n.");
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
