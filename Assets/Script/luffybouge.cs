using System;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 7f;
    bool isFacingRight = true;
    float jumpPower = 10f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;
    bool isDead = false; 


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

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
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
        animator.SetBool("isJumping", !isGrounded);

        if (isDead)
            return; 

        
        if (collision.CompareTag("Danger") && !FindObjectOfType<LuffyCombatController>().isGuarding)
        {
            Die();
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




// using System;
// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class PlayerMovement : MonoBehaviour
// {
//     float horizontalInput;
//     float moveSpeed = 7f;
//     bool isFacingRight = true;
//     float jumpPower = 10f;
//     bool isGrounded = false;

//     Rigidbody2D rb;
//     Animator animator;
//     bool isDead = false;

//     public Transform groundCheck;
//     public LayerMask groundLayer;
//     public float groundCheckRadius = 0.2f;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//     }

//     void Update()
//     {
//         if (isDead) return;

//         horizontalInput = Input.GetAxis("Horizontal");
//         FlipSprite();

//         animator.SetFloat("xVelocity", Math.Abs(horizontalInput));

//         // Vérifie si le joueur est au sol avec un Raycast
//         isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
//         animator.SetBool("isJumping", !isGrounded);

//         // Saut uniquement si le joueur est bien au sol
//         if (Input.GetButtonDown("Jump") && isGrounded)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, jumpPower);
//         }

//         // Empêcher l'accrochage aux murs
//         if (!isGrounded && Mathf.Abs(rb.velocity.x) < 0.1f)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.5f);
//         }
//     }

//     private void FixedUpdate()
//     {
//         if (isDead) return;

//         rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
//         animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
//         animator.SetFloat("yVelocity", rb.velocity.y);
//     }

//     void FlipSprite()
//     {
//         if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
//         {
//             isFacingRight = !isFacingRight;
//             Vector2 ls = transform.localScale;
//             ls.x *= -1f;
//             transform.localScale = ls;
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (isDead) return;

//         if (collision.CompareTag("Danger") && !FindObjectOfType<LuffyCombatController>().isGuarding)
//         {
//             Die();
//         }
//     }

//     void Die()
//     {
//         if (isDead) return;
//         isDead = true;

//         animator.SetTrigger("Death");

//         rb.velocity = Vector2.zero;
//         rb.isKinematic = true;
//         GetComponent<Collider2D>().enabled = false;

//         Invoke("RestartLevel", 2f);
//     }

//     void RestartLevel()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }
// }





























































































































