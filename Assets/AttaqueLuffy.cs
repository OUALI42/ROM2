using UnityEngine;

public class LuffyCombatController : MonoBehaviour
{
    private Animator animator;

    [Header("Input Keys")]
    public KeyCode guardKey = KeyCode.R;            
    public KeyCode autoAttackKey = KeyCode.A;     
    public KeyCode specialAttackKey = KeyCode.Z;   
    public KeyCode ultimateAttackKey = KeyCode.E; 

    void Start()
    {
       
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
        HandleCombat();
    }

    private void HandleCombat()
    {
       
        if (Input.GetKeyDown(guardKey))
        {
            SetCombatState("isGuarding");
        }

        
        if (Input.GetKeyDown(autoAttackKey))
        {
            SetCombatState("isAutoAttacking");
        }

        
        if (Input.GetKeyDown(specialAttackKey))
        {
            SetCombatState("isSpecialAttacking");
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

        // Optionnel : Désactivation automatique après un délai
        StartCoroutine(ResetState(state, 0.5f)); // Désactive l'état après 0.5s
    }

    private System.Collections.IEnumerator ResetState(string state, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool(state, false);
    }
}




