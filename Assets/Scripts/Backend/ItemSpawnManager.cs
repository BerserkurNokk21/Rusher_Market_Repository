using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawnManager : NetworkBehaviour
{
    [System.Serializable]
    public class SpawnPointConfig
    {
        public Transform spawnPoint;
        public GameObject itemPrefab;
    }

    [SerializeField] private List<SpawnPointConfig> spawnPointConfigs;
    public float spawnInterval = 5.0f;
    public float initialSpawnDelay = 2.0f;
    void Start()
    {
        StartCoroutine(SpawnItems());
    }
    IEnumerator SpawnItems()
    {
        Debug.Log("Iniciando spawn de items en servidor");
        yield return new WaitForSeconds(initialSpawnDelay);

        while (true)
        {
            foreach (SpawnPointConfig config in spawnPointConfigs)
            {
                if (config.itemPrefab != null && config.spawnPoint != null)
                {
                    Debug.Log($"Intentando spawnear item: {config.itemPrefab.name}");
                    GameObject item = Instantiate(config.itemPrefab, config.spawnPoint.position, config.spawnPoint.rotation);
                    NetworkObject networkObject = item.GetComponent<NetworkObject>();
                    if (networkObject != null)
                    {
                        networkObject.Spawn();
                        Debug.Log($"Item spawneado exitosamente: {item.name}");
                    }
                    else
                    {
                        Debug.LogError($"Error: El prefab {config.itemPrefab.name} no tiene NetworkObject");
                    }
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}