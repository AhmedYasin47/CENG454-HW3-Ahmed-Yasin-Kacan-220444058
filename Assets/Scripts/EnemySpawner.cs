using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Düşman Prefab'ları")]
    public GameObject zigzagEnemyPrefab;
    public GameObject directEnemyPrefab;
    
    [Header("Havuz Ayarları")]
    public int poolSizePerType = 10;
    
    [Header("Spawn Alanı")]
    public float spawnRadius = 10f;

    private List<GameObject> zigzagPool;
    private List<GameObject> directPool;

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

    public void SpawnOne()
    {
        List<GameObject> chosenPool = Random.value < 0.5f ? zigzagPool : directPool;
        
        foreach (GameObject enemy in chosenPool)
        {
            if (!enemy.activeInHierarchy)
            {
                Vector3 randomSpawnPoint = new Vector3(
                    Random.Range(-spawnRadius, spawnRadius),
                    1f,
                    Random.Range(-spawnRadius, spawnRadius)
                );
                enemy.transform.position = randomSpawnPoint;
                enemy.SetActive(true);
                return;
            }
        }
    }
}