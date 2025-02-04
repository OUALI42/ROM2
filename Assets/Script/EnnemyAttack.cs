using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackCooldown = 1f; // Délai entre les attaques
    private float lastAttackTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Si le délai d'attaque est terminé, l'ennemi peut attaquer
            if (ShouldAttack()) 
            {
                Attack();
            }
        }
    }

    bool ShouldAttack()
    {
        // Implémenter une condition pour attaquer, comme la distance au joueur ou une entrée spécifique
        return true; // Ceci est un exemple simple, vous pouvez ajouter une logique plus complexe
    }

    void Attack()
    {
        // Déclenche l'animation d'attaque
        animator.SetTrigger("Attack");
        lastAttackTime = Time.time;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Gérer le dommage ou autre logique de collision lors de l'attaque
            Debug.Log("Le joueur a été attaqué !");
        }
    }
}