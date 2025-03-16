using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrolyZoneAttack : MonoBehaviour
{
    public int damage = 10; // Dégâts infligés

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est un ennemi
        {
            GokuHealth enemyHealth = other.GetComponent<GokuHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}

