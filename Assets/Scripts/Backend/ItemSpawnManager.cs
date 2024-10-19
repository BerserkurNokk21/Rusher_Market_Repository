using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    // Lista de prefabs de ítems a hacer spawn
    [SerializeField] private List<GameObject> itemPrefabs;

    // Lista de puntos de respawn (se asignan en el editor)
    [SerializeField] private List<Transform> spawnPoints;

    // Tiempo entre spawns
    public float spawnInterval = 5.0f;

    // Tiempo antes del primer spawn
    public float initialSpawnDelay = 2.0f;

    void Start()
    {
        // Inicia el proceso de respawn repetido
        StartCoroutine(SpawnItems());
    }

    // Corrutina que se encarga de hacer spawn de los ítems
    IEnumerator SpawnItems()
    {
        // Espera el tiempo inicial antes de empezar a hacer spawns
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true) // Hacer respawn indefinidamente (puedes añadir una condición si lo necesitas)
        {
            // Selecciona un prefab de ítem aleatorio
            GameObject itemToSpawn = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

            // Selecciona un punto de spawn aleatorio
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Instancia el ítem en el punto de spawn
            Instantiate(itemToSpawn, spawnPoint.position, spawnPoint.rotation);

            // Espera antes de hacer el próximo spawn
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
