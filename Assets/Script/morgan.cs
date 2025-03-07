using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class morgan : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;
    public SpriteRenderer Morgan;
    public Animator animator;
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string[] dialogues;
    public KeyCode interactionKey = KeyCode.H; 

    public GameObject attackZone1; // Zone de l'attaque 1
    public GameObject attackZone2; // Zone de l'attaque 2


    private Transform target;
    private int destPoint = 0;
    private bool isTalking = false;
    private bool playerInRange = false;
    private int dialogueIndex = 0;
    private bool hasTalked = false;
    private bool isAttacking = false; // Pour savoir si le boss a commencé son cycle d'attaques

    public float attack1Duration = 1f;
    public float attack2Duration = 1.5f;
    public float attackCooldown = 5f;
    public GameObject healthBar; // Référence à la barre de vie


    void Start()
    {
        target = waypoints[0];
        dialogueBox.SetActive(false);
        attackZone1.SetActive(false);
        attackZone2.SetActive(false);
        healthBar.SetActive(false);
    }

    void Update()
    {
        if (!isTalking && !isAttacking) // Le boss bouge tant qu'il ne parle pas et n'attaque pas
        {
            MoveBoss();
        }

        if (isTalking && Input.GetKeyDown(interactionKey))
        {
            // Le dialogue est terminé, on ferme la boîte et on commence le cycle d'attaques
            dialogueBox.SetActive(false);
            isTalking = false;
            StartCoroutine(AttackCycle()); // Démarre le cycle d'attaques
        }
    }

    void MoveBoss()
    {
        if (playerInRange && !isTalking) return; 

        Vector2 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, target.position) < 0.3f)
        {
            destPoint = (destPoint + 1) % waypoints.Length;
            target = waypoints[destPoint];
            Morgan.flipX = !Morgan.flipX;
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogueBox.SetActive(true);
        dialogueIndex = 0;
        // dialogueText.text = dialogues[dialogueIndex];

        animator.SetTrigger("parle");
        animator.SetBool("isTalking", true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            healthBar.SetActive(true); // Toujours afficher la barre de vie quand le joueur entre

            if (!hasTalked) // Si le dialogue n'a pas encore eu lieu
            {
                hasTalked = true;
                playerInRange = true;
                animator.SetTrigger("parle");
                StartDialogue();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            healthBar.SetActive(false);
        }
    }

    IEnumerator AttackCycle()
    {
        isAttacking = true; // Empêche le boss de bouger une fois qu'il attaque

        while (true)
        {
            // Attaque 1
            animator.SetTrigger("Attack1");
            attackZone1.SetActive(true);
            yield return new WaitForSeconds(attack1Duration);
            attackZone1.SetActive(false);

            yield return new WaitForSeconds(attackCooldown); // Pause

            // Attaque 2
            animator.SetTrigger("Attack2");
            attackZone2.SetActive(true);
            yield return new WaitForSeconds(attack2Duration);
            attackZone2.SetActive(false);

            yield return new WaitForSeconds(attackCooldown); // Pause avant de recommencer
        }
    }
    
}