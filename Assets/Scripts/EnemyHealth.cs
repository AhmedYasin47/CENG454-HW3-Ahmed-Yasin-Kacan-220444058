using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Damage_Interface
{
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    public static event Action<EnemyHealth> OnEnemyDied;
    public event Action<int> OnHealthChanged;

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnEnemyDied?.Invoke(this);
        
        gameObject.SetActive(false);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}