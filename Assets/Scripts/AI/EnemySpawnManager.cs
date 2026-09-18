using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform spawnParent;

    [SerializeField]
    private float startSpawnInterval = 5f;

    [SerializeField]
    private float minimumSpawnInterval = 1f;

    [SerializeField]
    private int startMaxEnemies = 3;

    [SerializeField]
    private int maximumEnemies = 10;

    [SerializeField]
    private float difficultyIncreaseTime = 60f;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private float spawnTimer = 0f;
    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Awake()
    {
        

        for (int i = 0; i < spawnParent.childCount; i++)
        {
            spawnPoints.Add(spawnParent.GetChild(i));
        }

        isRunning = true;

        GameManager.Instance.OnGameEnd += OnEndGame;
    }

    private void OnEndGame()
    {
        isRunning = false;

        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] != null)
            {
                Destroy(spawnedEnemies[i]);
            }
        }

    }

    private void Update()
    {
        if(!isRunning) return;
        elapsedTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        RemoveDeadEnemies();

        float difficulty =
            Mathf.Clamp01(elapsedTime / difficultyIncreaseTime);

        float currentSpawnInterval =
            Mathf.Lerp(
                startSpawnInterval,
                minimumSpawnInterval,
                difficulty
            );

        int currentMaxEnemies =
            Mathf.RoundToInt(
                Mathf.Lerp(
                    startMaxEnemies,
                    maximumEnemies,
                    difficulty
                )
            );

        if (spawnTimer >= currentSpawnInterval)
        {
            spawnTimer = 0f;

            if (spawnedEnemies.Count < currentMaxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Count == 0)
        {
            return;
        }

        Transform spawnPoint =
            spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject enemy =
            Instantiate(
                enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

        spawnedEnemies.Add(enemy);


    }

    private void RemoveDeadEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }
    }

}