using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    private Animator animator;
    public int Give_Ki;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.Play("BuuDeath"); // Animation de mort
        
        // Trouver Goku dans la scène et lui donner du Ki
        GokuAnimAttack goku = FindObjectOfType<GokuAnimAttack>();
        Ki_Barre kiBar = FindObjectOfType<Ki_Barre>();
        if (goku != null)
        {
            goku.Ki += Give_Ki;
            kiBar.Ki_gestion();
        }
        
        Destroy(gameObject, 1f); // Détruit le mob après 1 seconde
    }
}
