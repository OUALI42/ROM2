using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class mob : MonoBehaviour
{
    public float patrolSpeed = 2f;   
    public float chaseSpeed = 4f;     
    private Rigidbody2D rb;            
    private bool movingRight = true;   
    public bool isChasing = false;    
    private float changeDirectionTime;  
    public Transform player;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
	private BoxCollider2D attackCollider;
  
   


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetRandomDirectionChangeTime();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Transform colliderChild = transform.Find("Detection");
        boxCollider = colliderChild.GetComponent<BoxCollider2D>();
		Transform attackColliderChild = transform.Find("ZonAttack");
        attackCollider = attackColliderChild.GetComponent<BoxCollider2D>();
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        float moveDirection = movingRight ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);
        
        if (Time.time >= changeDirectionTime)
        {
            movingRight = !movingRight;
            SetRandomDirectionChangeTime();
        }
        
        if (moveDirection < 0) {
            spriteRenderer.flipX = true;
            boxCollider.offset = new Vector2(-Mathf.Abs(boxCollider.offset.x), boxCollider.offset.y);
        } 
        else if (moveDirection > 0) {
            spriteRenderer.flipX = false;
            boxCollider.offset = new Vector2(Mathf.Abs(boxCollider.offset.x), boxCollider.offset.y);
        }
    }
    
   

    
    void ChasePlayer()
    {
        if (player == null) return;

        float direction2 = player.position.x > transform.position.x ? 1 : -1;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(direction2 * chaseSpeed, rb.velocity.y);

        // Flipper correctement le sprite
        spriteRenderer.flipX = direction2 < 0; 

        
        if (direction2 > 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }


    void SetRandomDirectionChangeTime()
    {
        changeDirectionTime = Time.time + Random.Range(4f, 10f); 
    }
    
    public void StartChase(Transform target)
    {
        player = target;
        isChasing = true;
    }

    public void StopChase()
    {
        isChasing = false;
    }

	public void StopMovement()
	{
    	rb.velocity = Vector2.zero;
		rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
	}

	public void ResumeMovement(float speed)
	{
		rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    	rb.velocity = new Vector2(speed * (movingRight ? 1 : -1), rb.velocity.y);
	}

}