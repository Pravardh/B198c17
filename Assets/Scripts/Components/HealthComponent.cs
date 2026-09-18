using System;
using UnityEngine;

public abstract class HealthComponent : MonoBehaviour, IHealthComponent
{
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float currentHealth;

    public event Action OnTakeDamage;
    public event Action OnHeal;
    public event Action OnDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0.0f, maxHealth);
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0.0f, maxHealth);

        if(currentHealth <= 0.0f)
        {
            Die();
            OnDeath?.Invoke();    
        }
    }

    protected virtual void Die()
    {
        
    }

}
