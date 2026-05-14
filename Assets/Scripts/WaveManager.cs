using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    public string waveName = "Dalga 1";
    public int enemyCount = 5;
    public float timeBetweenSpawns = 1.5f;
}

public class WaveManager : MonoBehaviour
{
    [Header("Dalgalar")]
    [SerializeField] private List<WaveData> waves = new List<WaveData>();
    
    [Header("Dalgalar Arası Bekleme")]
    [SerializeField] private float timeBetweenWaves = 3f;

    [Header("Referanslar")]
    [SerializeField] private EnemySpawner enemySpawner;

    private int currentWaveIndex = 0;
    private int aliveEnemiesCount = 0;
    private bool isWaveActive = false;
    private bool gameWon = false;

    public static event Action<int, int> OnWaveStarted; 
    public static event Action<int> OnWaveCompleted;  
    public static event Action OnAllWavesCompleted;  
    public static event Action<int> OnEnemyCountChanged;  

    void OnEnable()
    {
        EnemyHealth.OnEnemyDied += HandleEnemyDied;
        CoreHealth.OnCoreDestroyed += HandleGameOver;
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= HandleEnemyDied;
        CoreHealth.OnCoreDestroyed -= HandleGameOver;
    }

    void Start()
    {
        StartCoroutine(StartNextWaveAfterDelay(2f));
    }

    private IEnumerator StartNextWaveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (gameWon) return;
        
        if (currentWaveIndex >= waves.Count)
        {
            gameWon = true;
            Debug.Log("All wawes are clear!");
            OnAllWavesCompleted?.Invoke();
            return;
        }

        WaveData wave = waves[currentWaveIndex];
        Debug.Log("Dalga başladı");
        
        OnWaveStarted?.Invoke(currentWaveIndex + 1, waves.Count);
        
        isWaveActive = true;
        aliveEnemiesCount = wave.enemyCount;
        OnEnemyCountChanged?.Invoke(aliveEnemiesCount);
        
        StartCoroutine(SpawnWaveCoroutine(wave));
    }

    private IEnumerator SpawnWaveCoroutine(WaveData wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            if (gameWon) yield break;
            
            enemySpawner.SpawnOne();
            yield return new WaitForSeconds(wave.timeBetweenSpawns);
        }
    }

    private void HandleEnemyDied(EnemyHealth enemy)
    {
        if (!isWaveActive) return;
        
        aliveEnemiesCount--;
        OnEnemyCountChanged?.Invoke(aliveEnemiesCount);
        
        Debug.Log("Kalan düşman: " + aliveEnemiesCount);

        if (aliveEnemiesCount <= 0)
        {
            isWaveActive = false;
            Debug.Log("✅ DALGA " + (currentWaveIndex + 1) + " TEMİZLENDİ!");
            OnWaveCompleted?.Invoke(currentWaveIndex);
            
            currentWaveIndex++;
            StartCoroutine(StartNextWaveAfterDelay(timeBetweenWaves));
        }
    }

    private void HandleGameOver()
    {
        isWaveActive = false;
        gameWon = false;
        StopAllCoroutines();
    }

    public int GetCurrentWave() => currentWaveIndex + 1;
    public int GetTotalWaves() => waves.Count;
    public int GetAliveEnemiesCount() => aliveEnemiesCount;
}