// using UnityEngine;

// public class LuffyBouge : MonoBehaviour
// {
//     public float moveSpeed = 5f;
//     public float jumpForce = 300f;

//     public Transform groundCheckLeft;
//     public Transform groundCheckRight;
//     public LayerMask groundLayer;

//     public Rigidbody2D rb;
//     public Animator animator;
//     public SpriteRenderer spriteRenderer;

//     private bool isGrounded ;
//     private bool isJumping ;
//     private bool isFalling  ; // Nouveau booléen pour détecter une chute
//     private Vector3 velocity = Vector3.zero;

//     private float jumpCooldown = 0.9f; // Temps avant de pouvoir sauter à nouveau
//     private float lastJumpTime = 0f;
    

//     void FixedUpdate()
//     {
//         // Détection du sol
//         isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, groundLayer);
//         Debug.DrawLine(groundCheckLeft.position, groundCheckRight.position, Color.red);
        

//         // Détection de chute
//         isFalling = !isGrounded && rb.linearVelocity.y < -0.1f;

//         // Mouvement horizontal
//         float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

//         // Saut (uniquement si le cooldown est passé et que le personnage est au sol)
//         if (Input.GetButtonDown("Jump") && isGrounded && Time.time - lastJumpTime > jumpCooldown)
//         {
//             isJumping = true;
//             lastJumpTime = Time.time; // Enregistre l'heure du dernier saut
//         }

//         MovePlayer(horizontalMovement);

//         // Flip le sprite en fonction de la direction
//         Flip(rb.linearVelocity.x);

//         // Animation
//         HandleAnimations(horizontalMovement);
//     }

//     void MovePlayer(float _horizontalMovement)
//     {
//         // Mouvement horizontal
//         Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.linearVelocity.y);
//         rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, .05f);

//         // Mouvement de saut
//         if (isJumping)
//         {
//             rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce / rb.mass);
//             isJumping = false;
//         }
//     }

//     void Flip(float _velocity)
//     {
//         if (_velocity > 0.1f)
//         {
//             spriteRenderer.flipX = false;
//         }
//         else if (_velocity < -0.1f)
//         {
//             spriteRenderer.flipX = true;
//         }
//     }

//     void HandleAnimations(float horizontalMovement)
// {
//     // Détecter la vitesse absolue pour l'animation de marche
//     float characterVelocity = Mathf.Abs(rb.linearVelocity.x);
//     Debug.Log($"IsGrounded: {isGrounded}, IsJumping: {animator.GetBool("IsJumping")}, IsFalling: {animator.GetBool("IsFalling")}");

//     // Mettre à jour l'Animator pour la marche et l'état au sol
//     animator.SetFloat("speed", characterVelocity);
//     animator.SetBool("IsGrounded", isGrounded);

//     // Gérer l'animation de saut
//     if (!isGrounded && rb.linearVelocity.y > 0.1f) // Si le personnage monte
//     {
//         animator.SetBool("IsJumping", true);
//         animator.SetBool("IsFalling", false);
//     }
//     else if (!isGrounded && rb.linearVelocity.y < -0.1f) // Si le personnage descend
//     {
//         animator.SetBool("IsJumping", false);
//         animator.SetBool("IsFalling", true);
//     }
//     else if (isGrounded) // Si le personnage touche le sol
//     {
//         animator.SetBool("IsJumping", false);
//         animator.SetBool("IsFalling", false);
//     }
// }



// }




using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 5f;
    bool isFacingRight = true; // Initialise à droite
    float jumpPower = 7f;
    bool isGrounded = false;

    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
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
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
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
    }
}


































