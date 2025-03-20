using System;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    [SerializeField] private float moveSpeed = 7f;
    bool isFacingRight = true;
    [SerializeField] private float jumpPower = 10f;
    public bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;
    bool isDead = false; 
    public bool isAttacking = false; 



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Si mort aucun n'impact
        if (isDead)return; 

        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        animator.SetFloat("xVelocity", Math.Abs(horizontalInput));

        // Gestion du saut
        if (Input.GetButtonDown("Jump") && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
            animator.Play("Movement");
        }
    }

    private void FixedUpdate()
    {
        //Si mort aucun n'impact
        if (isDead)return; 

        //Gestion des déplacements
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        // Gestion du changement de direction du personnage
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector2 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", false); //Désactivé l'animation de saut
        animator.Play("Movement");

        if (isDead)return; 

        // Rencontre en collision d'un danger = mort
        if (collision.CompareTag("Danger") && !FindObjectOfType<LuffyCombatController>().isGuarding)
        {
            Die();
        }

        // Gestion des transitions
        if (collision.gameObject.CompareTag("bar-zone1")) // Transition dans la zone bar
        {
            
            SceneManager.LoadScene("Didacticiel V.Final");
        }
        if (collision.gameObject.CompareTag("didactitiel-menu")) // Transition dans la scene didacticiel
        {
            
            SceneManager.LoadScene("MainMenu");
        }
    }
    

    // Gestion de la mort et son animation
    void Die()
    {
        if (isDead)return;

        isDead = true;

       
        animator.SetTrigger("Death");
        
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; 

        GetComponent<Collider2D>().enabled = false;

        Invoke("RestartLevel", 2f);
    }

    // Relance du niveaux
    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}



































































































































