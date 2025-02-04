using UnityEngine;

public class EnnemyDeath : MonoBehaviour
{
    private Animator animator;
    private bool isDead = false;

    // Variable qui indique si l'attaque spéciale du joueur est en cours
    public bool isSpecialAttack = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("Die");
            // Vous pouvez ajouter des effets de mort, des sons, ou des points de score ici.
            Destroy(gameObject, 2f); // Détruire l'ennemi après l'animation de mort.
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérification si le joueur est en collision avec l'ennemi et si l'attaque spéciale est activée
        if (collision.collider.CompareTag("Player") && isSpecialAttack)
        {
            Die(); // Appeler la méthode Die quand l'ennemi reçoit l'attaque spéciale du joueur.
        }
    }
}