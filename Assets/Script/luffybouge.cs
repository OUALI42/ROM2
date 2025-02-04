
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
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
            return; 

        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void FlipSprite()
    {
        if ((isFacingRight && horizontalInput < 0f) || (!isFacingRight && horizontalInput > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            Debug.Log("Direction inversée : " + (isFacingRight ? "Droite" : "Gauche"));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);

        if (isDead)
            return; 

        
        if (collision.CompareTag("Danger"))
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

        
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; 

        
        GetComponent<Collider2D>().enabled = false;

        
        Invoke("RestartLevel", 5f);
    }

    void RestartLevel()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

































