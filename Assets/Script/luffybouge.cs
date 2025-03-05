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
    public bool isAttacking = false; // Empêche les actions comme le saut pendant une attaque



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead)
            return; 

        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        animator.SetFloat("xVelocity", Math.Abs(horizontalInput));

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
        if (isDead)
            return; 

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector2 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            // Debug.Log("Direction inversée : " + (isFacingRight ? "Droite" : "Gauche"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", false);
        animator.Play("Movement");

        if (isDead)
            return; 

        
        if (collision.CompareTag("Danger") && !FindObjectOfType<LuffyCombatController>().isGuarding)
        {
            Die();
        }
        
        if (collision.gameObject.CompareTag("ennemi")) // Vérifie si l'objet touché est un ennemi
        {
            
            SceneManager.LoadScene("zone1");
        }
        
        if (collision.gameObject.CompareTag("Fin"))
        {
            
            SceneManager.LoadScene("Credits");
        }
        if (collision.gameObject.CompareTag("Bar"))
        {
            
            SceneManager.LoadScene("zone1");
        }


    }
    

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

       
        animator.SetTrigger("Death");

        
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; 

        
        GetComponent<Collider2D>().enabled = false;

        
        Invoke("RestartLevel", 2f);
    }

    void RestartLevel()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}



































































































































