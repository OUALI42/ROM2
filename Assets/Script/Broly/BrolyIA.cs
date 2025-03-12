using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrolyBoss : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private bool isAttacking = false;
    public bool isChasing = false;
    private bool movingRight = true;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    private float changeDirectionTime;
    public Transform player;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private BoxCollider2D attackCollider;

    [Header("Attaques")]
    [SerializeField] private GameObject BrolyKameprefab;
    [SerializeField] private Transform BrolyKamepooint;
    [SerializeField] private float time_destruct;
    [SerializeField] private GameObject BrolypunchHitbox;
    [SerializeField] private GameObject BrolyMarteauxHitbox;
    [SerializeField] private GameObject BrolyfootHitbox;
    [SerializeField] private GameObject BrolykameHitbox;
    [SerializeField] private float meleeRange = 5f; // Plage d'attaque en mêlée

    public float pauseBetweenAttacks = 1.5f; // Temps de pause entre chaque attaque
    public float pauseAfterKamehameha = 3f; // Pause spécifique pour le Kamehameha
    public bool isFrozen = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        SetRandomDirectionChangeTime();

        BrolykameHitbox.SetActive(false);
        BrolypunchHitbox.SetActive(false);
        BrolyMarteauxHitbox.SetActive(false);
        BrolyfootHitbox.SetActive(false);
    }

    void Update()
    {
        if (isFrozen) return; // Empêche tout mouvement et attaque

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Move();
        }

        // Appeler ChooseAttack pour effectuer une attaque aléatoire à chaque mise à jour
        if (player != null && !isAttacking)
        {
            ChooseAttack();
        }
    }

    void Move()
    {
        if (isFrozen) return; // Stop le mouvement si figé
        float moveDirection = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);

        if (Time.time >= changeDirectionTime)
        {
            movingRight = !movingRight;
            SetRandomDirectionChangeTime();
        }

        
    }

    void ChasePlayer()
    {
        if (player == null || isFrozen) return;

        float direction = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

        // Broly doit se tourner vers le joueur en fonction de la position X
        if (direction > 0 && !spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true; // Tourner vers la droite
        }
        else if (direction < 0 && spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false; // Tourner vers la gauche
        }
    }


    void SetRandomDirectionChangeTime()
    {
        changeDirectionTime = Time.time + Random.Range(4f, 10f);
    }

    public void StartChase(Transform target)
    {
        player = target;
        isChasing = true;
    }

    public void StopChase()
    {
        player = null;
        isChasing = false;
    }

    void ChooseAttack()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool isClose = distance <= meleeRange;

        animator.SetBool("IsClose", isClose);

        if (!isClose)
        {
            animator.SetTrigger("BrolyWalk");
            StartCoroutine(MoveToPlayer()); // Déplacer Broly vers le joueur
            return;
        }

        int attackType = Random.Range(0, isClose ? 4 : 4); // 3 attaques en mêlée, 1 attaque à distance
       switch (attackType)
        {
            case 0:
                AttackPunch(); // BrolyPunch1
                break;
            case 1:
                AttackFoot(); // BrolyPunch2
                break;
            case 2:
                AttackMarteau(); // BrolyMarteaux
                break;
            case 3:
                AttackKamehameha(); // BrolyKame
                break;
        }

    }

  

    private IEnumerator ActivateHitbox(GameObject hitbox, float duration, bool isKamehameha = false)
    {
        isAttacking = true;
        hitbox.SetActive(true);
        yield return new WaitForSeconds(duration);
        hitbox.SetActive(false);
        
        // Utilise une pause différente si c'est un Kamehameha
        yield return new WaitForSeconds(isKamehameha ? pauseAfterKamehameha : pauseBetweenAttacks);
        isAttacking = false;
    }



    // Définir les attaques spécifiques
    public void AttackPunch()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("BrolyPunch1");
            StartCoroutine(ActivateHitbox(BrolypunchHitbox, 0.3f));
        }
    }

    public void AttackFoot()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("BrolyFoot");
            StartCoroutine(ActivateHitbox(BrolyfootHitbox, 0.8f));
        }
    }

    public void AttackMarteau()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("BrolyMarteaux");
            StartCoroutine(ActivateHitbox(BrolyMarteauxHitbox, 1f));
        }
    }

    public void AttackKamehameha()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("BrolyKame");
            StartCoroutine(ActivateHitbox(BrolykameHitbox, 1.3f, true)); // Indique que c'est un Kamehameha
        }
    }

    void KameBroly()
    {
        if (BrolyKameprefab != null && BrolyKamepooint != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(BrolyKameprefab, BrolyKamepooint.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct); // Détruire après 0.5 secondes
        }
    }

    // Déplacer Broly vers le joueur
    private IEnumerator MoveToPlayer()
    {
        // Déplacer Broly vers la position du joueur
        while (Vector3.Distance(transform.position, player.position) > meleeRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
            yield return null;
        }
        // Arrêter Broly une fois qu'il est assez proche pour attaquer
        rb.velocity = Vector2.zero;
    }
}
