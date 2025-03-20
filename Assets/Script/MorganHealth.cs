using UnityEngine;
using System.Collections;

public class MorganHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    private Animator animator; // Référence à l'Animator
    public bool isDead = false; // Empêche plusieurs appels à Die()

    public LayerMask layerMask;
    private morgan morgan;
    

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        morgan = GetComponent<morgan>();
        

        // Accéder à l'Animator du parent "morgan"
        Transform parentmorgan = transform.parent; 
        if (parentmorgan != null)
        {
            animator = parentmorgan.GetComponent<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError("Animator non trouvé sur le parent 'morgan' !");
        }
    }

    // Gestion des degats
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

    // gestion de la mort
    public void Die()
    {
        if (isDead) return; // Sécurité pour ne pas exécuter plusieurs fois
        isDead = true;
        Debug.Log("Morgan est mort !");
        if (animator != null)
        {
            animator.Play("Morgan-death"); 
        }

        // Lancer la destruction après l'animation
        StartCoroutine(DestroyAfterDeath());
    }

    public IEnumerator DestroyAfterDeath()
    {
        // Attendre que l'animation en cours ne soit plus "Morgan-death"
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Morgan-death"))
        {
            yield return null; // Attend une frame avant de revérifier
        }

        yield return new WaitForSeconds(1f); // Attendre encore un peu avant destruction

        Destroy(transform.parent.gameObject);
    }

}
