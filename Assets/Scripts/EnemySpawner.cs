using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int poolSize = 10;   
    public float spawnInterval = 3f; 

    private List<GameObject> enemyPool;
    private float spawnTimer;

    void Start()
    {
        enemyPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            enemyPool.Add(obj);
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                Vector3 randomSpawnPoint = new Vector3(Random.Range(-10f, 10f), 1f, Random.Range(-10f, 10f));
                enemy.transform.position = randomSpawnPoint;
                
                enemy.SetActive(true);
                return;
            }
        }
        
        Debug.LogWarning("Havuzda müsait düşman yok!");
    }
}