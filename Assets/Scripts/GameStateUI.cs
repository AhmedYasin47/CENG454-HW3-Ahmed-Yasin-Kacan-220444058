using UnityEngine;

public class GameStateUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    void OnEnable()
    {
        CoreHealth.OnCoreDestroyed += ShowGameOver;
        WaveManager.OnAllWavesCompleted += ShowVictory;
    }

    void OnDisable()
    {
        CoreHealth.OnCoreDestroyed -= ShowGameOver;
        WaveManager.OnAllWavesCompleted -= ShowVictory;
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    private void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ShowVictory()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}