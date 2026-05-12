using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoreHealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI  healthText;
    [SerializeField] private Image fillImage;
    
    [Header("Renk Ayarları")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color midHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;

    private int maxHealth;

    void Start()
    {
        CoreHealth core = FindFirstObjectByType<CoreHealth>();
        if (core != null)
        {
            maxHealth = core.maxHealth;
            
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = maxHealth;
            }
            
            UpdateUI(maxHealth);
        }
    }

    void OnEnable()
    {
        CoreHealth.OnCoreDamaged += UpdateUI;
        CoreHealth.OnCoreDestroyed += OnCoreDestroyed;
    }

    void OnDisable()
    {
        CoreHealth.OnCoreDamaged -= UpdateUI;
        CoreHealth.OnCoreDestroyed -= OnCoreDestroyed;
    }

    private void UpdateUI(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }

        if (fillImage != null && maxHealth > 0)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            
            if (healthPercent > 0.5f)
            {
                fillImage.color = Color.Lerp(midHealthColor, fullHealthColor, (healthPercent - 0.5f) * 2f);
            }
            else
            {
                fillImage.color = Color.Lerp(lowHealthColor, midHealthColor, healthPercent * 2f);
            }
        }
    }

    private void OnCoreDestroyed()
    {
        if (healthSlider != null) healthSlider.value = 0;
        if (healthText != null) healthText.text = "0 / " + maxHealth;
        if (fillImage != null) fillImage.color = lowHealthColor;
    }
}