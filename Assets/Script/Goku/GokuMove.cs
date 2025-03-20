using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GokuMove : MonoBehaviour
{
    float horizontalInput;
    [SerializeField] public float moveSpeed = 5f;
    public bool isFacingRight = true;
    [SerializeField] public float jumpPower = 5f;
    [SerializeField] private float jumpBoostMultiplier = 0.5f; 
    [SerializeField] private float maxJumpTime = 0.3f; 
    private bool isJumping = false;
    private float jumpTimeCounter;
    public bool isGrounded = false;
    public bool isDashing = false;
    private float dashCooldownTimeLeft;
    [SerializeField] private float dashSpeed = 15f; 
    [SerializeField] private float dashDuration = 0.2f; 
    [SerializeField] private float dashCooldown = 1f; 
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;

    Rigidbody2D rb;
    Animator animator;
    [SerializeField] public AnimatorOverrideController superSaiyanController;
    private GokuHealth health;
    private GokuAnimAttack attack;
    public AudioClip Audio_tp; 
    public AudioClip Audio_jump; 
    public GameObject murInvisible;
    public GameObject murInvisible2;




    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dashCooldownTimeLeft = 0f;
        health = GetComponent<GokuHealth>();
        attack = GetComponent<GokuAnimAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.isDead == true) return;
        if (isDashing) return;
        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();
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
                    animator.SetBool("isJumping", false); 
                    animator.SetBool("isFalling", false); 
                }
                else
                {
                   animator.Play("GokuDash");
                }
                StartCoroutine(PlayDashWithDelay()); 
                attack.PlaySound(Audio_tp);
        }
    }

    // Delai pour le Dash
    IEnumerator PlayDashWithDelay()
    {
        yield return new WaitForSeconds(0.1f);  
        StartCoroutine(Dash());
    }


    // Gestion du saut
    private void HandleJump()
    {
        if(health.isDead == true) return;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            attack.PlaySound(Audio_jump);
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", true);
            
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.linearVelocity += Vector2.up * (jumpBoostMultiplier * Time.deltaTime * 10);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

    }

    // Gestion des déplacements 
    private void FixedUpdate()
    {
        if(health.isDead == true) return;
        if (isDashing) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    // Gestion du flip
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

    // Gestion du Dash
    private IEnumerator Dash()
    {
        isDashing = true;
        dashCooldownTimeLeft = dashCooldown;

        float dashDirection = isFacingRight ? 1f : -1f;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0);
            yield return null;
        }
        animator.SetBool("isDashing",false);
        isDashing = false;
    }


    // Quand un objet rentre en collision avec goku
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isJumping", false);
            animator.Play("Movement");
            isGrounded = true;
        } 
        if (other.gameObject.CompareTag("Snow-Volcan")) // 
        {
            // Gestion des transitions
            SceneManager.LoadScene("Volcan");
        }
        
        if (other.gameObject.CompareTag("murInvisible")) // 
        {
            
            murInvisible.SetActive(true);
        }
        if (other.gameObject.CompareTag("murInvisible2")) // 
        {
            
            murInvisible2.SetActive(true);
        }
        
        if (other.gameObject.CompareTag("zone1-credits 1")) // 
        {
            
            SceneManager.LoadScene("Credits 1");
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Pike")) 
        {
            animator.Play("GokuDeath");
            health.Die();
        }
    }

    // Gestion du saut lorsque qu'il sort du sol
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            animator.SetBool("isJumping", true);
            isGrounded = false;
        }   
    }
}









