using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Düşman Prefab'ları")]
    public GameObject zigzagEnemyPrefab;
    public GameObject directEnemyPrefab;
    
    [Header("Havuz Ayarları")]
    public int poolSizePerType = 5;
    public float spawnInterval = 3f;

    private List<GameObject> zigzagPool;
    private List<GameObject> directPool;
    private float spawnTimer;

    void Start()
    {
        zigzagPool = CreatePool(zigzagEnemyPrefab, poolSizePerType);
        directPool = CreatePool(directEnemyPrefab, poolSizePerType);
    }

    private List<GameObject> CreatePool(GameObject prefab, int size)
    {
        List<GameObject> pool = new List<GameObject>();
        if (prefab == null) return pool;
        
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
        return pool;
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
    List<GameObject> chosenPool = Random.value < 0.5f ? zigzagPool : directPool;
    
    foreach (GameObject enemy in chosenPool)
    {
        if (!enemy.activeInHierarchy)
        {
            Vector3 randomSpawnPoint = new Vector3(
                Random.Range(-10f, 10f),
                1f,
                Random.Range(-10f, 10f)
            );
            enemy.transform.position = randomSpawnPoint;
            enemy.SetActive(true);
            return;
        }
    }
}
}