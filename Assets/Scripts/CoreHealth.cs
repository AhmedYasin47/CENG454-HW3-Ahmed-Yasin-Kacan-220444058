using System;
using UnityEngine;

public class CoreHealth : MonoBehaviour, Damage_Interface
{
    public int maxHealth = 100;
    private int currentHealth;

    public static event Action<int> OnCoreDamaged;
    public static event Action OnCoreDestroyed;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log("Çekirdek Hasar Aldı! Kalan Can: " + currentHealth);

        OnCoreDamaged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("OYUN BİTTİ! Çekirdek Parçalandı.");
            OnCoreDestroyed?.Invoke();
        }
    }
}