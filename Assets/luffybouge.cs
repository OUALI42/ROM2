// using UnityEngine;

// public class luffybouge : MonoBehaviour
// {
//     public float moveSpeed;
//     public float jumpForce;

//     public bool isJumping;
//     public bool isGrounded;

//     public Transform groundCheckLeft;
//     public Transform groundCheckRight;

//     public Rigidbody2D rb;

//     private Vector3 velocity = Vector3.zero;

//     void FixedUpdate()
//     {

//         isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position);

//         float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

//          if(Input.GetButtonDown("Jump") && isGrounded)
//         {
//             isJumping = true;
//         }

//         MovePlayer(horizontalMovement);
//     }

//     void MovePlayer(float _horizontalMovement)
//     {
//         Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
//         rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

//         if(isJumping == true){
//             rb.AddForce(new Vector2(0f,jumpForce));
//             isJumping = false;
//         }
//     }
   
// }



using UnityEngine;

public class LuffyBouge : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 300f;

    public bool isJumping;
    public bool isGrounded;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public LayerMask groundLayer;

    public Rigidbody2D rb;

    public Animator animator;

    public SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector3.zero;

    private float jumpCooldown = 0.1f;
    private float lastJumpTime = 0f;

    void FixedUpdate()
    {
        // Détection du sol
        isGrounded = Physics2D.OverlapArea(groundCheckLeft.position, groundCheckRight.position, groundLayer);
        Debug.DrawLine(groundCheckLeft.position, groundCheckRight.position, Color.red);

        // Mouvement horizontal
        float horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed;

        // Saut
        if (Input.GetButtonDown("Jump") && isGrounded && Time.time - lastJumpTime > jumpCooldown)
        {
            isJumping = true;
            lastJumpTime = Time.time;
        }

        MovePlayer(horizontalMovement);

        Flip(rb.linearVelocity.x);

        float characterVelocity = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("speed", characterVelocity);
    }

    void MovePlayer(float _horizontalMovement)
    {
        // Vitesse horizontale
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.linearVelocity.y);
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref velocity, .05f);

        // Saut
        if (isJumping)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce / rb.mass);
            isJumping = false;
        }
    }

    void Flip(float _velocity){
        if(_velocity > 0.1f){
            spriteRenderer.flipX = false;

        }else if(_velocity < -0.1f){
            spriteRenderer.flipX = true;
        }

    }
}
