using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int numberOfEnemiesToSpawn = 10;
    [SerializeField] private float spawnInterval = 1f;

    private GameObject[] a_spawnedEnemies;
    private int numEnemiesSpawned = 0;
    private bool isCoroutineRunning = false;

    void Start()
    {
        a_spawnedEnemies = new GameObject[numberOfEnemiesToSpawn];
        StartSpawnCoroutine();
    }

    void StartSpawnCoroutine()
    {
        if (!isCoroutineRunning)
        {
            StartCoroutine(SpawnEnemiesCoroutine());
        }
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        isCoroutineRunning = true;

        while (numEnemiesSpawned < numberOfEnemiesToSpawn)
        {
            int emptySlot = System.Array.FindIndex(a_spawnedEnemies, enemy => enemy == null);

            if (emptySlot != -1)
            {
                RespawnEnemy(emptySlot);
              
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                // If all slots are filled
                break;
            }
        }

        isCoroutineRunning = false;
    }

    void RespawnEnemy(int index)
    {
        Vector2 randomViewportPosition = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        Vector3 randomWorldPosition = Camera.main.ViewportToWorldPoint(new Vector3(randomViewportPosition.x, randomViewportPosition.y, 10f));

        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        a_spawnedEnemies[index] = Instantiate(enemyPrefabs[enemyIndex], randomWorldPosition, Quaternion.identity);

        numEnemiesSpawned++;
    }

    public void EnemyDestroyed()
    {
        numEnemiesSpawned--;

        if (numEnemiesSpawned < numberOfEnemiesToSpawn)
        {
            // Check if coroutine is not running, if not start it
            if (!isCoroutineRunning)
            {
                StartSpawnCoroutine();
            }
            else
            {
                // Update numEnemiesSpawned if coroutine is still running
                numEnemiesSpawned = System.Array.FindAll(a_spawnedEnemies, enemy => enemy != null).Length;
            }
        }
    }
}
