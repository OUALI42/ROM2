using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionBroly : MonoBehaviour
{
    private mob parentMob;
    public GameObject healthBarBroly; // Référence à la barre de vie
    private BrolyBoss Broly;
    public GameObject cinematic; // Référence à l'objet de la cinématique
    public Animator animator;
    public float durer_cinematic_broly;
    public float durer_anim_broly;
    private bool hasPlayedCinematic = false; // Booléen pour vérifier si la cinématique a déjà été jouée

    void Start()
    {
        healthBarBroly.SetActive(false);
        cinematic.SetActive(false);
    }

    void Awake()
    {
        parentMob = GetComponentInParent<mob>();
        Broly = GetComponentInParent<BrolyBoss>(); // Correction ici
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasPlayedCinematic)
        {
            hasPlayedCinematic = true;
            StartCoroutine(TriggerCinematicSequence(other.transform));
        }
        else if (other.CompareTag("Player"))
        {
            // Si la cinématique a déjà été jouée, lancer directement le combat
            healthBarBroly.SetActive(true);
            Broly.isFrozen = false; 
            parentMob.StartChase(other.transform);
        }
    }
      private IEnumerator TriggerCinematicSequence(Transform player)
    { 

        // Figer le temps
        Time.timeScale = 0;
        
        // Activer la cinématique
        cinematic.SetActive(true);
        
        // Attendre la durée de la cinématique (exemple : 3 secondes)
        yield return new WaitForSecondsRealtime(durer_cinematic_broly);
        
        // Désactiver la cinématique
        cinematic.SetActive(false);
        
        // Reprendre le temps
        Time.timeScale = 1;

        // Figer Broly pour éviter qu'il ne bouge avant la fin
        Broly.isFrozen = true; 

        animator.Play("Entrer de scene"); // Assure-toi que cette fonction est bien définie dans le script de Broly
        
        // Activer la barre de vie et déclencher l'animation d'entrée de Broly
        healthBarBroly.SetActive(true);

        yield return new WaitForSecondsRealtime(durer_anim_broly);

        Broly.isFrozen = false; 
        
        // Commencer la poursuite
        parentMob.StartChase(player);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            healthBarBroly.SetActive(false);
            Broly.isFrozen = true; // Broly reste figé
            parentMob.StopChase();
        }
    }
}