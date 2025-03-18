using UnityEngine;

public class PNJAnimationController : MonoBehaviour
{
    private Animator animator;  
    public string idleAnimation = "Idle";  // Animation par défaut
    public string firstExitAnimation = "Exit1";  // Première animation après la sortie du joueur
    public string secondExitAnimation = "Exit2";  // Seconde animation (finale)

    private void Start()
    {
        animator = GetComponent<Animator>();  
        animator.Play(idleAnimation); // Démarre avec l'animation Idle
    }

    // Quand le joueur quitte la zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Joue la première animation de sortie
            animator.Play(firstExitAnimation);

            // Après la première animation, jouer la seconde et rester dessus
            Invoke("PlayFinalAnimation", animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    // Joue la dernière animation et reste dessus
    private void PlayFinalAnimation()
    {
        animator.Play(secondExitAnimation);
    }
}
