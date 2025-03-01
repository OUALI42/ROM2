using UnityEngine;
public class LuffyCombatController : MonoBehaviour
{

    private PlayerMovement mouvement;
    private Animator animator;

    [Header("Input Keys")]
    public KeyCode guardKey = KeyCode.R;            
    public KeyCode autoAttackKey = KeyCode.A;     
    public KeyCode specialAttackKey = KeyCode.Z;   
    public KeyCode ultimateAttackKey = KeyCode.E; 

    [Header("Attack Properties")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int autoAttackDamage = 5;
    public int specialAttackDamage = 15;
    public int ultimateAttackDamage = 50;

    [Header("Stamina")]
    public int  specialAttackStaminaCost = 33 ; // Coût en endurance pour l'attaque spéciale
    private LuffyStamina stamina; // Référence au script d'endurance

    public bool isGuarding = false;
    private bool canGuard = true;
    public float guardDuration = 1.5f; // Durée de la garde
    public float guardCooldown = 2f; // Temps de recharge avant de pouvoir bloquer à nouveau

    [Header("SoundEffect")]
    public AudioClip autoAttackSound; 
    public AudioClip specialAttackSound; 
    public AudioClip ultimateAttackSound;
    private AudioSource audioSource;

    [Header("cinématique ultimeAttack")]
    public Canvas cinematicCanvas;  // Canvas pour la cinématique
    public float cinematicDuration = 3f;  // Durée de la cinématique
    private bool isInCinematic = false;  // Indicateur pour savoir si la cinématique est en cours

    [Header(" Haki")]
    [SerializeField] private GameObject Hakiprefab; // Le prefab du slash
    [SerializeField] private Transform Hakipoint; // L'endroit où le slash apparaît
    [SerializeField] private float time_destruct;





    void Start()
    {
        mouvement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        stamina = GetComponent<LuffyStamina>(); 
        audioSource = GetComponent<AudioSource>();
        cinematicCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleCombat();
    }
    

    public void TriggerAttack(int damage)
    {
        PerformAttack(damage);
    }


    private void HandleCombat()
    {
        if (Input.GetKeyDown(guardKey) && canGuard)
        {
            StartGuarding();
        }


        if (Input.GetKeyDown(autoAttackKey))
        {
            if (!mouvement.isGrounded){
                SetCombatState("isAutoAttacking");
                animator.SetBool("isJumping", false); //  Désactive l'animation de saut
            }else{
                SetCombatState("isAutoAttacking");
            }
            PlaySound(autoAttackSound); 
        }

        if (Input.GetKeyDown(specialAttackKey) && stamina.currentStamina >= specialAttackStaminaCost)
        {
            mouvement.isAttacking = true;
            SetCombatState("isSpecialAttacking");
            stamina.UseStamina(specialAttackStaminaCost);
            PlaySound(specialAttackSound);
            
        }
        if (Input.GetKeyDown(ultimateAttackKey) && !isInCinematic)
        {
            StartCoroutine(PlayCinematicAndUltimateAttack());
        }

    }
    private System.Collections.IEnumerator PlayCinematicAndUltimateAttack()
    {
        isInCinematic = true;

        // Affiche le canvas de la cinématique
        cinematicCanvas.gameObject.SetActive(true);

        // Gèle le jeu (arrête le temps)
        Time.timeScale = 0f;

        // Attends la durée de la cinématique
        yield return new WaitForSecondsRealtime(cinematicDuration);  // WaitForSecondsRealtime permet de garder un temps réel, même si Time.timeScale est à 0

        // Cache le canvas de la cinématique
        cinematicCanvas.gameObject.SetActive(false);

        // Redémarre le temps du jeu
        Time.timeScale = 1f;

        // Lancer l'attaque ultime
        SetCombatState("isUltimateAttacking");
        PlaySound(ultimateAttackSound);

        // Attends que l'attaque se termine avant de réactiver les autres comportements
        yield return new WaitForSeconds(1f); // 1 seconde par exemple, tu peux ajuster la durée de l'attaque

        isInCinematic = false;
    }


    private void SetCombatState(string state)
    {
        animator.SetBool("isGuarding", false);
        animator.SetBool("isAutoAttacking", false);
        animator.SetBool("isSpecialAttacking", false);
        animator.SetBool("isUltimateAttacking", false);

        animator.SetBool(state, true);
        StartCoroutine(ResetState(state, 0.5f));
    }

    private System.Collections.IEnumerator ResetState(string state, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(state, false);
        mouvement.isAttacking = false;
    }

     // Ajoute cette variable pour définir les ennemis que Luffy peut toucher

    private void PerformAttack(int damage)
    {
        // Vérifie les ennemis dans la zone de l'attaque en utilisant le LayerMask
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Vérifie que l'objet touché est bien de type `MorganHealth` (Morgan)
            MorganHealth enemyHealth = enemy.GetComponent<MorganHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void StartGuarding()
    {
        isGuarding = true;
        canGuard = false;
        animator.SetBool("isGuarding", true);

        Invoke("StopGuarding", guardDuration);
        Invoke("ResetGuardCooldown", guardCooldown);
    }

    void StopGuarding()
    {
        isGuarding = false;
        animator.SetBool("isGuarding", false);
    }

    void ResetGuardCooldown()
    {
        canGuard = true;
    }
       public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Joue le son une seule fois
        }
    }

    void SpawnSlashEffect()
    {
        if (Hakiprefab != null && Hakipoint != null)
        {
            // Créer l'effet à la bonne position
            GameObject slash = Instantiate(Hakiprefab, Hakipoint.position, Quaternion.identity);
            
            // Vérifier la direction du joueur et ajuster l'orientation
            float direction = transform.localScale.x; // Suppose que l'échelle X change selon la direction
            slash.transform.localScale = new Vector3(direction, 1, 1); // Inverse le slash si nécessaire
            
            slash.transform.parent = transform; // Le lier au personnage
            Destroy(slash, time_destruct); // Détruire après 0.5 secondes
        }
    }


}





























