using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrolyHealth : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth;
    private Animator animator;
    public HealthBar healthBar; // Référence à la barre de vie

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.Play("BrolyDeath"); // Animation de mort
        Destroy(gameObject, 1f); // Détruit le mob après 1 seconde
    }
}
