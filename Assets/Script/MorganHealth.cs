using UnityEngine;

public class MorganHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private Animator animator; // Référence à l'Animator
    private bool isDead = false; // Empêche plusieurs appels à Die()

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Ne prend pas de dégâts si déjà mort

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Sécurité pour ne pas exécuter plusieurs fois
        isDead = true;

        Debug.Log("Morgan est mort !");
        animator.SetTrigger("Die"); // Joue l'animation de mort

        // Attend la fin de l'animation avant de détruire l'objet
        StartCoroutine(DestroyAfterDeath());
    }

    private System.Collections.IEnumerator DestroyAfterDeath()
    {
        // Attend la durée de l'animation de mort
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject); // Détruit l'objet après l'animation
    }
}

