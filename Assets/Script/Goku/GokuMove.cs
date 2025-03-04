// using System;
// using UnityEngine;
// using System.Collections;

// public class GokuMove : MonoBehaviour
// {
//     float horizontalInput;
//     [SerializeField] private float moveSpeed = 5f;
//     bool isFacingRight = true;
//     [SerializeField] private float jumpPower = 5f;
//     [SerializeField] private float jumpBoostMultiplier = 0.5f; // Boost en maintenant la touche
//     [SerializeField] private float maxJumpTime = 0.3f; // Durée max du boost
//     private bool isJumping = false;
//     private float jumpTimeCounter;
//     public bool isGrounded = false;
//     public bool isDashing = false;
//     private float dashCooldownTimeLeft;
//     [SerializeField] private float dashSpeed = 15f; // Vitesse du dash
//     [SerializeField] private float dashDuration = 0.2f; // Durée du dash
//     [SerializeField] private float dashCooldown = 1f; // Temps de recharge du dash
//     [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;

//     Rigidbody2D rb;
//     Animator animator;
    

    

//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         dashCooldownTimeLeft = 0f;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (isDashing) return;
//         horizontalInput = Input.GetAxis("Horizontal");

//         FlipSprite();

//         // Gestion du saut
//         HandleJump();

//         if (dashCooldownTimeLeft > 0)
//         {
//             dashCooldownTimeLeft -= Time.deltaTime;
//         }

//         if (Input.GetKeyDown(dashKey) && dashCooldownTimeLeft <= 0)
//         {
//             if (!isGrounded)
//                 {
//                     animator.Play("GokuDash");
//                     animator.SetBool("isJumping", false); //  Désactive l'animation de saut
//                     animator.SetBool("isFalling", false); //  Désactive l'animation de chute
//                 }
//                 else
//                 {
//                    animator.Play("GokuDash");
//                 }
//                 // dinoAttackAnimation.PlaySound(dashsong);
//                 StartCoroutine(PlayDashWithDelay()); 
//         }
//     }
//     IEnumerator PlayDashWithDelay()
//     {
//         // Attendre une fraction de seconde pour laisser l'animation démarrer
//         yield return new WaitForSeconds(0.1f);  // Ajuste ce délai si nécessaire

//         // Lancer la fonction Dash après le délai
//         StartCoroutine(Dash());
//     }
//     private void HandleJump()
//     {
//         if (Input.GetButtonDown("Jump") && isGrounded)
//         {
//             isJumping = true;
//             jumpTimeCounter = maxJumpTime;
//             rb.velocity = new Vector2(rb.velocity.x, jumpPower);
//             isGrounded = false;
//             animator.SetBool("isJumping", true);
//         }

//         if (Input.GetButton("Jump") && isJumping)
//         {
//             if (jumpTimeCounter > 0)
//             {
//                 rb.velocity += Vector2.up * (jumpBoostMultiplier * Time.deltaTime * 10);
//                 jumpTimeCounter -= Time.deltaTime;
//             }
//         }

//         if (Input.GetButtonUp("Jump"))
//         {
//             isJumping = false;
//         }
//     }

//     private void FixedUpdate()
//     {
//         if (isDashing) return;
//         rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
//         animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
//         animator.SetFloat("yVelocity", rb.linearVelocity.y);
//     }

//     void FlipSprite()
//     {
//         if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
//         {
//             isFacingRight = !isFacingRight;
//             Vector3 ls = transform.localScale;
//             ls.x *= -1f;
//             transform.localScale = ls;
//         }
//     }
//     private IEnumerator Dash()
//     {
//         isDashing = true;
//         dashCooldownTimeLeft = dashCooldown;

//         float dashDirection = isFacingRight ? 1f : -1f;
//         float startTime = Time.time;

//         while (Time.time < startTime + dashDuration)
//         {
//             rb.velocity = new Vector2(dashDirection * dashSpeed, 0);
//             yield return null;
//         }
//         animator.SetBool("isDashing",false);
//         isDashing = false;
//     }

    

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.gameObject.CompareTag("Ground"))
//         {
//             animator.SetBool("isJumping", false);
//             animator.Play("Movement");
//             isGrounded = true;
//         } 
//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.gameObject.CompareTag("Ground"))
//         {
//             animator.SetBool("isJumping", true);
//             isGrounded = false;
//         }
//     }
// }


using System;
using UnityEngine;
using System.Collections;

public class GokuMove : MonoBehaviour
{
    float horizontalInput;
    [SerializeField] public float moveSpeed = 5f;
    bool isFacingRight = true;
    [SerializeField] public float jumpPower = 5f;
    [SerializeField] private float jumpBoostMultiplier = 0.5f; // Boost en maintenant la touche
    [SerializeField] private float maxJumpTime = 0.3f; // Durée max du boost
    private bool isJumping = false;
    private float jumpTimeCounter;
    public bool isGrounded = false;
    public bool isDashing = false;
    private float dashCooldownTimeLeft;
    [SerializeField] private float dashSpeed = 15f; // Vitesse du dash
    [SerializeField] private float dashDuration = 0.2f; // Durée du dash
    [SerializeField] private float dashCooldown = 1f; // Temps de recharge du dash
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;

    Rigidbody2D rb;
    Animator animator;
    [SerializeField] public AnimatorOverrideController superSaiyanController;




    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashCooldownTimeLeft = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) return;
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        // Gestion du saut
        HandleJump();

        if (dashCooldownTimeLeft > 0)
        {
            dashCooldownTimeLeft -= Time.deltaTime;
        }

        if (Input.GetKeyDown(dashKey) && dashCooldownTimeLeft <= 0)
        {
            if (!isGrounded)
                {
                    animator.Play("GokuDash");
                    animator.SetBool("isJumping", false); //  Désactive l'animation de saut
                    animator.SetBool("isFalling", false); //  Désactive l'animation de chute
                }
                else
                {
                   animator.Play("GokuDash");
                }
                // dinoAttackAnimation.PlaySound(dashsong);
                StartCoroutine(PlayDashWithDelay()); 
        }
    }
    IEnumerator PlayDashWithDelay()
    {
        // Attendre une fraction de seconde pour laisser l'animation démarrer
        yield return new WaitForSeconds(0.1f);  // Ajuste ce délai si nécessaire

        // Lancer la fonction Dash après le délai
        StartCoroutine(Dash());
    }
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity += Vector2.up * (jumpBoostMultiplier * Time.deltaTime * 10);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if(isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        dashCooldownTimeLeft = dashCooldown;

        float dashDirection = isFacingRight ? 1f : -1f;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = new Vector2(dashDirection * dashSpeed, 0);
            yield return null;
        }
        animator.SetBool("isDashing",false);
        isDashing = false;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isJumping", false);
            animator.Play("Movement");
            isGrounded = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }
    }
}




