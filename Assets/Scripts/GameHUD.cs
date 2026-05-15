using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    void OnEnable()
    {
        WaveManager.OnWaveStarted += UpdateWaveText;
        WaveManager.OnEnemyCountChanged += UpdateEnemyCount;
    }

    void OnDisable()
    {
        WaveManager.OnWaveStarted -= UpdateWaveText;
        WaveManager.OnEnemyCountChanged -= UpdateEnemyCount;
    }

    void Start()
    {
        if (waveText != null) waveText.text = "Wave 0 / 0";
        if (enemyCountText != null) enemyCountText.text = "Kalan: 0";
    }

    private void UpdateWaveText(int currentWave, int totalWaves)
    {
        if (waveText != null)
            waveText.text = "Wave " + currentWave + " / " + totalWaves;
    }

    private void UpdateEnemyCount(int count)
    {
        if (enemyCountText != null)
            enemyCountText.text = "Count: " + count;
    }
}