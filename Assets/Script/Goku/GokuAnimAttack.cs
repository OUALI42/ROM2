// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GokuAnimAttack : MonoBehaviour
// {
//     private GokuMove Move;
//     private Animator animator;
//     public bool isAttacking = false; // Empêche le spam

//     [Header("Touches personnalisables")]
//     [SerializeField] public KeyCode punchKey = KeyCode.A; 
//     [SerializeField] public KeyCode  footKey = KeyCode.E; 
//     [SerializeField] public KeyCode  Ssj1Key = KeyCode.R; 
    

//     void Start()
//     {
//         Move= GetComponent<GokuMove>();
//         animator = GetComponent<Animator>();
//     }

//     void Update()
//     {
//         HandleCombat();
//     }

//     private void HandleCombat()
//     {
//         if (isAttacking) return; // Empêche de spammer l'attaque pendant une animation

//         if (Input.GetKeyDown(punchKey))
//         {
//             if (!isAttacking)
//             {
//                 // Si le joueur est en l'air, il joue l'attaque aérienne
//                 if (!Move.isGrounded)
//                 {
//                     animator.Play("GokuPunch");
//                     animator.SetBool("isJumping", false); //  Désactive l'animation de saut
//                     animator.SetBool("isFalling", false); //  Désactive l'animation de chute
//                 }
//                 else
//                 {
//                     animator.SetTrigger("Punch");
//                 }
//                 StartCoroutine(AttackCooldown(0.5f)); 
//             }
//         }
        

//         if (Input.GetKeyDown(footKey))
//         {
//             if (!isAttacking)
//             {
//                 // Si le joueur est en l'air, il joue l'attaque aérienne
//                 if (!Move.isGrounded)
//                 {
//                     animator.Play("GokuFoot");
//                     animator.SetBool("isJumping", false); //  Désactive l'animation de saut
//                     animator.SetBool("isFalling", false); //  Désactive l'animation de chute
//                 }
//                 else
//                 {
//                     animator.SetTrigger("Foot");
//                 }
//                 StartCoroutine(AttackCooldown(0.5f)); 
//             }
//         }

//         if (Input.GetKeyDown(Ssj1Key))
//         {
//             animator.SetTrigger("Ssj1");
//         }
        
//     }
    


//     private IEnumerator AttackCooldown(float duration)
//     {
//         isAttacking = true;     
//         yield return new WaitForSeconds(duration); // Attend la fin de l'animation d'attaque
//         isAttacking = false; // Permet de réutiliser les attaques après le cooldown
//     }

// }




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GokuAnimAttack : MonoBehaviour
{
    private GokuMove Move;
    private Animator animator;
    public bool isAttacking = false; // Empêche le spam
    public bool isSuperSaiyan = false;

    [Header("Touches personnalisables")]
    [SerializeField] public KeyCode punchKey = KeyCode.A; 
    [SerializeField] public KeyCode  footKey = KeyCode.E; 
    [SerializeField] public KeyCode  Ssj1Key = KeyCode.R;

    

    void Start()
    {
        Move= GetComponent<GokuMove>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleCombat();

        if (Input.GetKeyDown(KeyCode.T) && !isSuperSaiyan) // Vérifie si Goku n'est PAS déjà transformé
            {
                StartCoroutine(TransformToSuperSaiyan());
            }
    }

   private void HandleCombat()
    {
        if (isAttacking) return;

        if (Input.GetKeyDown(punchKey))
        {
            if (!isAttacking)
            {
                // Si le joueur est en l'air, il joue l'attaque aérienne
                if (!Move.isGrounded)
                {
                    animator.Play("GokuPunch");
                    animator.SetBool("isJumping", false); //  Désactive l'animation de saut
                    animator.SetBool("isFalling", false); //  Désactive l'animation de chute
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
                StartCoroutine(AttackCooldown(0.5f)); 
            }
        }
    }


    private IEnumerator TransformToSuperSaiyan()
    {
        if (!isSuperSaiyan)
        {
            isSuperSaiyan = true; 
            animator.SetBool("isSuperSaiyan", true); // Active la transition vers GokuSsj1
            
            animator.Play("GokuSsj1"); // Joue l'animation de transformation
            yield return new WaitForSeconds(1.5f); // Temps de l'animation

            // Boost des stats
            Move.moveSpeed *= 1.2f;
            Move.jumpPower *= 1.2f;

            // Appliquer les nouvelles animations SSJ
            animator.runtimeAnimatorController = Move.superSaiyanController;
        }
    }



    private IEnumerator AttackCooldown(float duration)
    {
        isAttacking = true;     
        yield return new WaitForSeconds(duration); // Attend la fin de l'animation d'attaque
        isAttacking = false; // Permet de réutiliser les attaques après le cooldown
    }

}



