using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GokuAnimAttack : MonoBehaviour
{
    private GokuMove Move;
    public Animator animator;
    public bool isAttacking = false; 
    public bool isSuperSaiyan = false;

    [Header("Touches personnalisables")]
    [SerializeField] public KeyCode punchKey = KeyCode.A; 
    [SerializeField] public KeyCode  footKey = KeyCode.E; 
    [SerializeField] public KeyCode  KameKey = KeyCode.R;

    [Header("cinématique de transformation en SSJ1")]
    public Canvas cinematicCanvas;  
    public float cinematicDuration = 3f;  
    private bool isInCinematic = false;  
    [SerializeField] private GameObject éclair1prefab; 
    [SerializeField] private Transform éclair1pooint; 
    [SerializeField] private GameObject éclair2prefab; 
    [SerializeField] private Transform éclair2pooint; 
    [SerializeField] private GameObject Auraprefab; 
    [SerializeField] private Transform Aurapooint; 
    [SerializeField] private GameObject bouleprefab; 
    [SerializeField] private Transform boulepooint; 
    [SerializeField] private GameObject Kameprefab; 
    [SerializeField] private Transform Kamepooint; 
    [SerializeField] private float time_destruct;
    [SerializeField] private float time_destruct_for_aura;
    [SerializeField] private float time_destruct_for_boule;
    [SerializeField] private float time_destruct_for_Kame;

    [Header("Gestion des hitboxs")]
    [SerializeField] private GameObject punchHitbox;
    [SerializeField] private GameObject footHitbox;
    [SerializeField] private GameObject kameHitbox;
    
    private Hitbox degat;
    public Canvas transformation;  
    private bool canUseKamehameha = true;

    [Header("Sound Effect")]
    public AudioClip Audio_foot;  
    private AudioSource audioSource;

    [Header("Health")]
    private GokuHealth health;
    public HealthBar healthBar;

    [Header("Ki")]
    public int Ki;
    Ki_Barre kiBar;




    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        health= GetComponent<GokuHealth>();
        Move= GetComponent<GokuMove>();
        animator = GetComponent<Animator>();
        degat = GetComponentInChildren<Hitbox>(); 
        cinematicCanvas.gameObject.SetActive(false);
        transformation.gameObject.SetActive(false);
        kameHitbox.SetActive(false);
        punchHitbox.SetActive(false);
        footHitbox.SetActive(false);

        kiBar = FindObjectOfType<Ki_Barre>(); 
        Ki = 0;
        kiBar.currentKi = 0;
        kiBar.Ki_gestion();
    }


    void Update()
    {
        HandleCombat();

        if (Ki >= 50 && transformation != null) 
        {
            transformation.gameObject.SetActive(true);
        }

        // Gestion de la transformation en SSJ1
        if (Input.GetKeyDown(KeyCode.T) && !isSuperSaiyan && !isInCinematic && Ki >= 50)
        {
            if (transformation != null) 
            {
                health.currentHealth = 100;
                healthBar.SetHealth(100);
                Destroy(transformation.gameObject);
                transformation = null; 
            }
            StartCoroutine(PlayCinematicAndTransform());
        }
    }

    // Gestion du combat
    private void HandleCombat()
    {
        if (isAttacking) return;

        if (Input.GetKeyDown(punchKey))
        {
            if (!isAttacking)
            {
                if (!Move.isGrounded)
                {
                    animator.Play("GokuPunch");
                    animator.SetBool("isJumping", false); 
                    animator.SetBool("isFalling", false); 
                }
                else
                {
                    animator.SetTrigger("Punch");
                }
                StartCoroutine(AttackCooldown(0.5f)); 
            }
        }

        if (Input.GetKeyDown(footKey))
        {
            if (!isAttacking)
            {
                // Si le joueur est en l'air, il joue l'attaque aérienne
                if (!Move.isGrounded)
                {
                    animator.Play("GokuFoot");
                    animator.SetBool("isJumping", false); //  Désactive l'animation de saut
                    animator.SetBool("isFalling", false); //  Désactive l'animation de chute
                }
                else
                {
                    animator.SetTrigger("Foot");
                }
                PlaySound(Audio_foot); 
                StartCoroutine(AttackCooldown(0.5f)); 
            }
        }

        if (Input.GetKeyDown(KameKey) && isSuperSaiyan && canUseKamehameha)
        {
            if (!isAttacking)
            {
                // Si le joueur est en l'air, il joue l'attaque aérienne
                if (!Move.isGrounded)
                {
                    animator.Play("GokuKamehamehassj1");
                    animator.SetBool("isJumping", false); // Désactive l'animation de saut
                    animator.SetBool("isFalling", false); // Désactive l'animation de chute
                }
                else
                {
                    animator.SetTrigger("Kame");
                }
                
                StartCoroutine(KamehamehaCooldown(30f)); // Cooldown plus long
            }
        }
}
    private IEnumerator KamehamehaCooldown(float cooldownTime)
    {
        canUseKamehameha = false;
        yield return new WaitForSeconds(cooldownTime);
        canUseKamehameha = true;
    }

    private IEnumerator PlayCinematicAndTransform()
    {
        if (!isSuperSaiyan && !isInCinematic)
        {
            isInCinematic = true;
            cinematicCanvas.gameObject.SetActive(true); // Affiche la cinématique
            Time.timeScale = 0f; // Stoppe le temps

            yield return new WaitForSecondsRealtime(cinematicDuration); // Attends sans être affecté par Time.timeScale

            Time.timeScale = 1f; 
            cinematicCanvas.gameObject.SetActive(false); 
            isInCinematic = false;

            // Lance la transformation après la cinématique
            StartCoroutine(TransformToSuperSaiyan());
        }
    }


    // Gestion de la transformation
    private IEnumerator TransformToSuperSaiyan()
    {
        if (!isSuperSaiyan)
        {
            isSuperSaiyan = true; 
            animator.SetBool("isSuperSaiyan", true); // Active la transition vers GokuSsj1
            
            animator.Play("GokuSsj1"); // Joue l'animation de transformation
            yield return new WaitForSeconds(1.5f); 

            // Boost des stats
            Move.moveSpeed *= 1.5f;

            // Augmenter les dégâts après la transformation en Super Saiyan
            degat.damage = Mathf.RoundToInt(degat.damage + 10f); 

            // Appliquer les nouvelles animations SSJ
            animator.runtimeAnimatorController = Move.superSaiyanController;
        }
    }



    private IEnumerator AttackCooldown(float duration)
    {
        isAttacking = true;     
        yield return new WaitForSeconds(duration); // Attend la fin de l'animation d'attaque
        isAttacking = false; 
    }

    void eclair1()
    {
        if (éclair1prefab != null && éclair1pooint != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(éclair1prefab, éclair1pooint.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); 
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, 0.8f); 
        }
    }

    // Gestion de l'éclair durant la tranformation
    void eclair2()
    {
        if (éclair2prefab != null && éclair2pooint != null)
        {
            GameObject slash = Instantiate(éclair2prefab, éclair2pooint.position, Quaternion.identity);
            float direction = transform.localScale.x; 
            slash.transform.localScale = new Vector3(direction, 1, 1); 
            slash.transform.parent = transform; 
            Destroy(slash,  0.8f); 
        }
    }


    // Gestion de l'aura durant la tranformation
    void Aura()
        {
            if (Auraprefab != null && Aurapooint != null)
            {
                GameObject slash = Instantiate(Auraprefab, Aurapooint.position, Quaternion.identity);
                
                float direction = transform.localScale.x; 
                slash.transform.localScale = new Vector3(direction, 1, 1); 
                
                slash.transform.parent = transform; 
                Destroy(slash, time_destruct_for_aura); 
            }
        }


    // Gestion Boule de Kikoa
    void Boule()
    {
        if (bouleprefab != null && boulepooint != null)
        {
            GameObject slash = Instantiate(bouleprefab, boulepooint.position, Quaternion.identity);
            
            float direction = transform.localScale.x; 
            slash.transform.localScale = new Vector3(direction, 1, 1); 
            
            slash.transform.parent = transform; 
            Destroy(slash, time_destruct_for_boule); 
        }
    }


    // Gestion de l'effet du kamehameha 
    void Kame()
    {
        if (Kameprefab != null && Kamepooint != null)
        {
            
            GameObject slash = Instantiate(Kameprefab, Kamepooint.position, Quaternion.identity);
            
            
            float direction = transform.localScale.x; 
            slash.transform.localScale = new Vector3(direction, 1, 1); 
            
            slash.transform.parent = transform; 
            Destroy(slash, time_destruct_for_Kame); 
        }
    }


    // Gestion des hitboxs
     void PunchAttack()
    {
        StartCoroutine(ActivateHitbox(punchHitbox, 0.2f)); 
    }

    void FootAttack()
    {
        StartCoroutine(ActivateHitbox(footHitbox, 0.8f));
    }

    void KameAttack()
    {
        StartCoroutine(ActivateHitbox(kameHitbox, 1.3f)); 
    }

    private IEnumerator ActivateHitbox(GameObject hitbox, float duration)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(duration);
        hitbox.SetActive(false);
    }


    // Joue le son 
    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); 
        }
    }


}










