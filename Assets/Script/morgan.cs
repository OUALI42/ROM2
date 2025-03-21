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

    public GameObject attackZone1; 
    public GameObject attackZone2; 
    public AudioClip attackSound1; 
    public AudioClip attackSound2; 
    public AudioClip talkSound; 
    private AudioSource audioSource;

    private Transform target;
    private int destPoint = 0;
    private bool isTalking = false;
    private bool playerInRange = false;
    private int dialogueIndex = 0;
    private bool hasTalked = false;
    private bool isAttacking = false; 

    public float attack1Duration = 1f;
    public float attack2Duration = 1.5f;
    public float attackCooldown = 5f;
    public GameObject healthBar; 
    public bool prends_degats = false;

    void Start()
    {
        target = waypoints[0];
        dialogueBox.SetActive(false);
        attackZone1.SetActive(false);
        attackZone2.SetActive(false);
        healthBar.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isTalking && Input.GetKeyDown(interactionKey))
        {
            prends_degats = true;
            // Le dialogue est terminé, on ferme la boîte et on commence le cycle d'attaques
            dialogueBox.SetActive(false);
            isTalking = false;
            StartCoroutine(AttackCycle()); // Démarre le cycle d'attaques
        }
    }

    // Gestion des dialogues
    void StartDialogue()
    {
        isTalking = true;
        dialogueBox.SetActive(true);
        dialogueIndex = 0;
        animator.SetTrigger("parle");
        animator.SetBool("isTalking", true);
    }

    // Si le joueur entre dans la zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("morgancoll"))
        {
            healthBar.SetActive(true); // Toujours afficher la barre de vie quand le joueur entre

            if (!hasTalked) // Si le dialogue n'a pas encore eu lieu
            {
                hasTalked = true;
                playerInRange = true;
                animator.SetTrigger("parle");
                StartDialogue();
                audioSource.PlayOneShot(talkSound);
            }
        }
    }

    // Si le joueur sort de la zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("morgancoll"))
        {
            playerInRange = false;
            healthBar.SetActive(false);
        }
    }

    // Fonction pour activer la zone d'attaque 1
    public void ActivateAttackZone1()
    {
        attackZone1.SetActive(true);
        StartCoroutine(DeactivateAttackZone(attackZone1, attack1Duration));
        PlayAttackSound(1);
    }

    // Fonction pour activer la zone d'attaque 2
    public void ActivateAttackZone2()
    {
        attackZone2.SetActive(true);
        StartCoroutine(DeactivateAttackZone(attackZone2, attack2Duration));
        PlayAttackSound(2);
    }

    // Fonction pour desactiver les zone d'attaque 
    IEnumerator DeactivateAttackZone(GameObject attackZone, float duration)
    {
        yield return new WaitForSeconds(duration);
        attackZone.SetActive(false);
    }

    IEnumerator AttackCycle()
    {
        isAttacking = true; 

        while (true)
        {
            // Attaque 1
            animator.SetTrigger("Attack1");

            yield return new WaitForSeconds(attack1Duration + attackCooldown); // Pause

            // Attaque 2
            animator.SetTrigger("Attack2");

            yield return new WaitForSeconds(attack2Duration + attackCooldown); // Pause avant de recommencer
        }
    }

    // Joue le son du cycle d'attaque
    void PlayAttackSound(int attackNumber)
    {
        if (attackNumber == 1 && attackSound1 != null)
        {
            audioSource.PlayOneShot(attackSound1);
        }
        else if (attackNumber == 2 && attackSound2 != null)
        {
            audioSource.PlayOneShot(attackSound2);
        }
    }
}
