using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int poolSize = 30;
    
    private List<GameObject> pool;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        
        GameObject newBullet = Instantiate(bulletPrefab);
        pool.Add(newBullet);
        return newBullet;
    }
}