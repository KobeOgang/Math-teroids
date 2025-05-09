using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject asteroidPrefab;
    public float padding = 0.1f;
    public float baseSpawnRate = 2f; 
    public float minSpawnRate = 0.5f; 
    public float scalingFactor = 0.95f; 
    public float difficultyIncreaseInterval = 30f; 

    private Camera mainCamera;
    private float currentSpawnRate;

    private void Start()
    {
        mainCamera = Camera.main;
        currentSpawnRate = baseSpawnRate;
        StartCoroutine(SpawnAsteroids());
        StartCoroutine(ScaleSpawnRate());
        SpawnAsteroid();
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnRate);
            SpawnAsteroid();
        }
    }

    private void SpawnAsteroid()
    {
        if (asteroidPrefab == null)
        {
            Debug.LogError("Asteroid prefab is missing! Ensure it is properly assigned in the Inspector.");
            return;
        }

        Vector2 spawnPosition = GetRandomSpawnPosition();
        Vector2 direction = (Vector2.zero - spawnPosition).normalized;
        float speed = Random.Range(GameController.Instance.asteroidMinSpeed, GameController.Instance.asteroidMaxSpeed);

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.GetComponent<Rigidbody2D>().velocity = direction * speed;

        
    }

    private IEnumerator ScaleSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            
            currentSpawnRate = Mathf.Max(currentSpawnRate * scalingFactor, minSpawnRate);

        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float x = 0f;
        float y = 0f;

        
        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0: 
                x = Random.Range(0f, 1f);
                y = 1f + padding; 
                break;
            case 1: 
                x = Random.Range(0f, 1f);
                y = 0f - padding;
                break;
            case 2: 
                x = 0f - padding;
                y = Random.Range(0f, 1f);
                break;
            case 3: 
                x = 1f + padding;
                y = Random.Range(0f, 1f);
                break;
        }

        return mainCamera.ViewportToWorldPoint(new Vector3(x, y, 0f));
    }

}
