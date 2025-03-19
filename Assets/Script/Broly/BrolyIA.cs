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

    [Header("Kame")]
    [SerializeField] private GameObject BrolyKameprefab;
    [SerializeField] private Transform BrolyKamepooint;
    [SerializeField] private float time_destruct_for_kame;

    [Header("Kikoa Gauche")]
    [SerializeField] private GameObject Kikoa_haut_gauche_prfb;
    [SerializeField] private Transform Kikoa_haut_gauche_point;
    [SerializeField] private GameObject Kikoa_millieu_gauche_prfb;
    [SerializeField] private Transform Kikoa_millieu_gauche_point;
    [SerializeField] private GameObject Kikoa_bas_gauche_prfb;
    [SerializeField] private Transform Kikoa_bas_gauche_point;

    [Header("Kikoa droite")]
    [SerializeField] private GameObject Kikoa_haut_droite_prfb;
    [SerializeField] private Transform Kikoa_haut_droite_point;
    [SerializeField] private GameObject Kikoa_millieu_droite_prfb;
    [SerializeField] private Transform Kikoa_millieu_droite_point;
    [SerializeField] private GameObject Kikoa_bas_droite_prfb;
    [SerializeField] private Transform Kikoa_bas_droite_point;
    [SerializeField] private float time_destruct_for_Kikoa;

    [Header("Lazer bas")]
    [SerializeField] private GameObject Lazer_bas_prfb;
    [SerializeField] private Transform Lazer_bas_point;
    [SerializeField] private float time_destruct_for_Lazer_bas;

    [Header("Lazer haut")]
    [SerializeField] private GameObject Lazer_haut_prfb;
    [SerializeField] private Transform Lazer_haut_point;
    [SerializeField] private float time_destruct_for_Lazer_haut;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrfb;
    [SerializeField] private Transform explosionPoint;
    [SerializeField] private float time_destruct_for_explosion;

    [Header("Hitbox")]
    [SerializeField] private GameObject BrolypunchHitbox;
    [SerializeField] private GameObject BrolyMarteauxHitbox;
    [SerializeField] private GameObject BrolyfootHitbox;
    [SerializeField] private GameObject BrolykameHitbox;
    [SerializeField] private GameObject BrolySuperAttacksHitbox;
    [SerializeField] private GameObject BrolySuperAttacksHitbox2;
    [SerializeField] private GameObject BrolySuperLazerBasHitbox;
    [SerializeField] private GameObject BrolySuperLazerHautHitbox;

    [Header("Sound Effect")]
    // public AudioClip super_attaque1; 
    // public AudioClip super_attaque2;
    // public AudioClip super_lazer;  
    private AudioSource audioSource;

    public float time_for_lazer_bas;
    public float time_for_lazer_haut;
    [SerializeField] private float meleeRange = 5f; // Plage d'attaque en mêlée

    public float pauseBetweenAttacks = 1.5f; // Temps de pause entre chaque attaque
    public float pauseAfterKamehameha = 3f; // Pause spécifique pour le Kamehameha
    public bool isFrozen = true;
    public bool canFlip = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player")?.transform;
        audioSource = GetComponent<AudioSource>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        SetRandomDirectionChangeTime();
        BrolySuperLazerBasHitbox.SetActive(false);
        BrolySuperLazerHautHitbox.SetActive(false);
        BrolySuperAttacksHitbox2.SetActive(false);
        BrolySuperAttacksHitbox.SetActive(false);
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
        rb.linearVelocity = new Vector2(moveDirection * patrolSpeed, rb.linearVelocity.y);

        if (Time.time >= changeDirectionTime)
        {
            movingRight = !movingRight;
            SetRandomDirectionChangeTime();
        }

        
    }


    void ChasePlayer()
    {
        if (player == null || isFrozen) return;

        float direction2 = player.position.x > transform.position.x ? 1 : -1;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction2 * chaseSpeed, rb.velocity.y);

        // Inverser l'échelle seulement si canFlip est activé
        if (canFlip)
        {
            if (direction2 > 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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
        isChasing = false;
    }

    void ChooseAttack()
    {
        if (isFrozen) return;
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
            StartCoroutine(ActivateHitbox(BrolyfootHitbox, 0.6f));
        }
    }

    public void AttackMarteau()
    {
        if (!isAttacking)
        {
            animator.SetTrigger("BrolyMarteaux");
            StartCoroutine(ActivateHitbox(BrolyMarteauxHitbox, 0.8f));
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

    public void SupertAttacks()
    {
        StartCoroutine(ActivateHitbox(BrolySuperAttacksHitbox, 3f)); // Indique que c'est un Kamehameha
    }
    public void SupertAttacks2()
    {
        StartCoroutine(ActivateHitbox(BrolySuperAttacksHitbox2, 2.1f)); 
    }

    public void SupertLazerBas()
    {
        StartCoroutine(ActivateHitbox(BrolySuperLazerBasHitbox, time_for_lazer_bas)); 
    }
    public void SupertLazerHaut()
    {
        StartCoroutine(ActivateHitbox(BrolySuperLazerHautHitbox, time_for_lazer_haut)); 
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
            Destroy(slash, time_destruct_for_kame); // Détruire après 0.5 secondes
        }
    }

    void kikoa_haut_gauche()
    {
        if (Kikoa_haut_gauche_prfb != null && Kikoa_haut_gauche_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Kikoa_haut_gauche_prfb, Kikoa_haut_gauche_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Kikoa); // Détruire après 0.5 secondes
        }
    }

    void kikoa_millieu_gauche()
    {
        if (Kikoa_millieu_gauche_prfb != null && Kikoa_millieu_gauche_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Kikoa_millieu_gauche_prfb, Kikoa_millieu_gauche_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Kikoa); // Détruire après 0.5 secondes
        }
    }

    void kikoa_bas_gauche()
    {
        if (Kikoa_bas_gauche_prfb != null && Kikoa_bas_gauche_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Kikoa_bas_gauche_prfb, Kikoa_bas_gauche_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Kikoa); // Détruire après 0.5 secondes
        }
    }

    void kikoa_haut_droite()
    {
        if (Kikoa_haut_droite_prfb != null && Kikoa_haut_droite_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Kikoa_haut_droite_prfb, Kikoa_haut_droite_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Kikoa); // Détruire après 0.5 secondes
        }
    }

    void kikoa_millieu_droite()
    {
        if (Kikoa_millieu_droite_prfb != null && Kikoa_millieu_droite_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Kikoa_millieu_droite_prfb, Kikoa_millieu_droite_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Kikoa); // Détruire après 0.5 secondes
        }
    }

    void kikoa_bas_droite()
    {
        if (Kikoa_bas_droite_prfb != null && Kikoa_bas_droite_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Kikoa_bas_droite_prfb, Kikoa_bas_droite_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Kikoa); // Détruire après 0.5 secondes
        }
    }

    void Lazer_bas()
    {
        if (Lazer_bas_prfb != null && Lazer_bas_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Lazer_bas_prfb, Lazer_bas_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Lazer_bas); // Détruire après 0.5 secondes
        }
    }

    void Lazer_haut()
    {
        if (Lazer_haut_prfb != null && Lazer_haut_point != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Lazer_haut_prfb, Lazer_haut_point.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_Lazer_haut); // Détruire après 0.5 secondes
        }
    }

    void Explosion()
    {
        if (explosionPrfb != null && explosionPoint != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(explosionPrfb, explosionPoint.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct_for_explosion); // Détruire après 0.5 secondes
        }
    }

    private IEnumerator MoveToPlayer()
    {
        while (Vector3.Distance(transform.position, player.position) > meleeRange)
        {
            // Vérifier si Broly est frozen à chaque itération
            if (isFrozen)
            {
                rb.velocity = Vector2.zero; // Stoppe le mouvement
                yield break; // Sort de la coroutine
            }

            // Déplacer Broly vers la position du joueur
            Vector3 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);
            
            yield return null; // Attendre la prochaine frame
        }
        
        // Arrêter Broly une fois qu'il est assez proche pour attaquer
        rb.velocity = Vector2.zero;
    }



    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Joue le son une seule fois
        }
    }

}



