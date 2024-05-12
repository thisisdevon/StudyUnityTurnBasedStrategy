using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler OnDamage;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth = 0;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public bool TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        OnDamage?.Invoke(this, null);

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (currentHealth <= 0)
        {
            Die();
            return false; //is dead
        }
        return true; //still alive
    }

    private void Die()
    {
        OnDeath?.Invoke(this, null);
    }

    public float GetHealthNormalized()
    {
        return (float)currentHealth / maxHealth;
    }
}
