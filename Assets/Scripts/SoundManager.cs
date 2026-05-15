using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [Header("Ses Klipleri")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip gameOverSound;
    
    [Header("Ses Ayarları")]
    [SerializeField] [Range(0f, 1f)] private float shootVolume = 0.5f;
    [SerializeField] [Range(0f, 1f)] private float enemyDeathVolume = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float victoryVolume = 1f;
    [SerializeField] [Range(0f, 1f)] private float gameOverVolume = 1f;
    
    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        EnemyHealth.OnEnemyDied += HandleEnemyDied;
        WaveManager.OnAllWavesCompleted += PlayVictory;
        CoreHealth.OnCoreDestroyed += PlayGameOver;
    }

    void OnDisable()
    {
        EnemyHealth.OnEnemyDied -= HandleEnemyDied;
        WaveManager.OnAllWavesCompleted -= PlayVictory;
        CoreHealth.OnCoreDestroyed -= PlayGameOver;
    }

    public void PlayShoot()
    {
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound, shootVolume);
    }

    private void HandleEnemyDied(EnemyHealth enemy)
    {
        if (enemyDeathSound != null)
            audioSource.PlayOneShot(enemyDeathSound, enemyDeathVolume);
    }

    private void PlayVictory()
    {
        if (victorySound != null)
            audioSource.PlayOneShot(victorySound, victoryVolume);
    }

    private void PlayGameOver()
    {
        if (gameOverSound != null)
            audioSource.PlayOneShot(gameOverSound, gameOverVolume);
    }
}