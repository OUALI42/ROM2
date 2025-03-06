using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneAttack : MonoBehaviour
{
    public int damage = 10;
    public float damageInterval = 1f;
    private GokuHealth GokuLife;
    private mob mobScript;
    private Coroutine damageCoroutine;
	[SerializeField]
	private Animator animator;
    private bool isPlayerInZone = false;


    private void Start()
    {
        mobScript = GetComponentInParent<mob>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsAttacking", true);
            GokuLife = other.GetComponent<GokuHealth>();
            
            if (GokuLife != null)
            {
                isPlayerInZone = true;  // Le joueur est dans la zone
                
                mobScript.StopMovement();
                mobScript.StopChase();
                
                damageCoroutine = StartCoroutine(DealDamageContinuously());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("IsAttacking", false);
            isPlayerInZone = false;  // Le joueur est sorti de la zone
            
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            if (mobScript != null)
            {
                mobScript.ResumeMovement(mobScript.patrolSpeed);

                // Vérifier si le joueur est toujours dans la zone de détection principale
                Collider2D detectionZone = mobScript.GetComponentInChildren<detectionMob1Script>().GetComponent<Collider2D>();

                if (detectionZone.bounds.Contains(other.transform.position))
                {
                    mobScript.StartChase(other.transform);
                }
            }
        }
    }

    
  private IEnumerator DealDamageContinuously()
    {
        while (isPlayerInZone && GokuLife != null && GokuLife.currentHealth > 0)
        {
            GokuLife.TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }


}