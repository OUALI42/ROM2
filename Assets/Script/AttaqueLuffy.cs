using UnityEngine;


public class LuffyCombatController : MonoBehaviour
{
    
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

    

    void Start()
    {
        animator = GetComponent<Animator>();
        stamina = GetComponent<LuffyStamina>(); // On récupère le script d'endurance sur le même GameObject
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
            SetCombatState("isAutoAttacking");
            
        }

        if (Input.GetKeyDown(specialAttackKey) && stamina.currentStamina >= specialAttackStaminaCost)
        {
            SetCombatState("isSpecialAttacking");
            stamina.UseStamina(specialAttackStaminaCost);
            
        }

        if (Input.GetKeyDown(ultimateAttackKey))
        {
            SetCombatState("isUltimateAttacking");
            
        }

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


}















