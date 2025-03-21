using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GokuHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    public Animator animator; // Référence à l'Animator
    public bool isDead = false; // Empêche plusieurs appels à Die()
    public float timeBeforeRestart = 2f; // Temps avant le redémarrage du niveau après la mort

    [Header("Sound Effect")]
    public AudioClip Audio_Death; 
    private AudioSource audioSource;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Accéder à l'Animator du parent "goku"
        Transform parentGoku = transform.parent; // Récupère le parent immédiat (goku)
        if (parentGoku != null)
        {
            animator = parentGoku.GetComponent<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError("Animator non trouvé sur le parent 'goku' !");
        }
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

    public void Die()
    {
        if (isDead) return; // Sécurité pour ne pas exécuter plusieurs fois
        isDead = true;

        Debug.Log("Goku est mort !");
        if (animator != null)
        {
            animator.SetTrigger("Die");
            PlaySound(Audio_Death); 
        }
        
        // Désactiver le mouvement physique
        if (rb != null)
        {
            rb.isKinematic = true; // Rend le Rigidbody statique
            rb.velocity = Vector3.zero; // Stoppe tout mouvement
            // rb.angularVelocity = Vector3.zero; // Stoppe toute rotation
        }
        // Lancer le redémarrage du niveau après l'animation
        StartCoroutine(RestartLevelAfterDeath());
    }

    public IEnumerator RestartLevelAfterDeath()
    {
        // Attendre que l'animation en cours ne soit plus "Goku-death"
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("GokuDeath"))
        {
            yield return null; // Attend une frame avant de revérifier
        }

        // Attendre un peu avant de relancer le niveau
        yield return new WaitForSeconds(timeBeforeRestart);

        RestartLevel();
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Joue le son une seule fois
        }
    }
}
