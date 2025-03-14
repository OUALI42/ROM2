using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrolyHealth : MonoBehaviour
{
    public int maxHealth = 200;
    public int currentHealth;
    private Animator animator;
    public HealthBar healthBar; // Référence à la barre de vie
    private BrolyBoss Broly;
    public GameObject superAttackCanvas; // Référence au Canvas de l'animation
    public Animator canvasAnimator;
    public float freezeDuration = 3f; // Durée du freeze en secondes

    

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        Broly = GetComponent<BrolyBoss>();
        // Assigner l'Animator du Canvas correctement
        if (superAttackCanvas != null)
        {
            canvasAnimator = superAttackCanvas.GetComponent<Animator>();
        }
        superAttackCanvas.SetActive(false); // Désactive le Canvas au début
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if(currentHealth == 60)
        {
            Broly.isFrozen =true;
            animator.Play("BrolyAnimSuperAttack");
            Broly.isFrozen =false;
        }

        if(currentHealth == 40)
        {
            Broly.isFrozen =true;
            animator.Play("BrolySuperAttacks2");
            Broly.isFrozen =false;
        }

        if(currentHealth == 20)
        {
            StartCoroutine(SpecialAttackSequence("BrolyUltraLazer"));
        }


   IEnumerator SpecialAttackSequence(string animationName)
    {
        Time.timeScale = 0; // Arrête le temps
        superAttackCanvas.SetActive(true); // Active le Canvas

        // Assure que l'Animator du Canvas fonctionne même avec Time.timeScale = 0
        if (canvasAnimator != null)
        {
            canvasAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            canvasAnimator.Play("CanvaAnimLazer", 0, 0);
        }

        yield return null; // Force la mise à jour de l'animation avant l'attente
        yield return new WaitForSecondsRealtime(freezeDuration); // Attend X secondes en temps réel

        superAttackCanvas.SetActive(false); // Désactive l'animation
        Time.timeScale = 1; // Reprend le temps
        animator.Play("BrolyUltraLazer"); // Joue l'attaque spéciale
    }


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



