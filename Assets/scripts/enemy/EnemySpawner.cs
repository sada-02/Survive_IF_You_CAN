using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemyPrefab; 
        public float spawnProbability; 
    }

    // define enemies and their spawn probability
    public EnemyData[] enemies; 

    // define spawn area
    public Vector2 spawnAreaMin; 
    public Vector2 spawnAreaMax; 

    // define spawn interval
    public float minSpawnInterval = 15f; 
    public float maxSpawnInterval = 30f; 
    public int maxSpawnLocations = 3; 

    [Header("Collision Checking")]
    public LayerMask collisionLayer; 
    public float collisionRadius = 0.5f;

    private bool isSpawning = true;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (isSpawning)
        {
            // Determine a random spawn interval.
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            SpawnEnemiesAtRandomLocations();
        }
    }

    private void SpawnEnemiesAtRandomLocations()
    {
        List<Vector2> spawnPositions = new List<Vector2>();

        // Generate multiple valid spawn positions.
        for (int i = 0; i < maxSpawnLocations; i++)
        {
            Vector2 spawnPosition;
            if (TryGetValidSpawnPosition(out spawnPosition))
            {
                spawnPositions.Add(spawnPosition);
            }
        }

        foreach (Vector2 position in spawnPositions)
        {
            GameObject enemyToSpawn = SelectEnemyBasedOnProbability();
            if (enemyToSpawn != null)
            {
                Instantiate(enemyToSpawn, position, Quaternion.identity);
            }
        }
    }

    // Select an enemy based on their spawn probability.
    private GameObject SelectEnemyBasedOnProbability()
    {
        float totalProbability = 0f;

        foreach (var enemy in enemies)
        {
            totalProbability += enemy.spawnProbability;
        }

        float randomValue = Random.Range(0, totalProbability);

        foreach (var enemy in enemies)
        {
            if (randomValue < enemy.spawnProbability)
            {
                return enemy.enemyPrefab;
            }
            randomValue -= enemy.spawnProbability;
        }

        return null; 
    }

    // Check if the spawn position is free of colliders.
    private bool TryGetValidSpawnPosition(out Vector2 position)
    {
        const int maxAttempts = 10; 

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            position = new Vector2(x, y);

            // Check if the position is free of colliders.
            if (!Physics2D.OverlapCircle(position, collisionRadius, collisionLayer))
            {
                return true; 
            }
        }

        position = Vector2.zero;
        return false; 
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }

    public void ResumeSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnEnemies());
        }
    }
}