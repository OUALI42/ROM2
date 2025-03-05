using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Transform colliderChild = transform.Find("detectionMob1");
        boxCollider = colliderChild.GetComponent<BoxCollider2D>();
		Transform attackColliderChild = transform.Find("zoneAttack");
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

        float direction = player.position.x > transform.position.x ? 1 : -1; 
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);


        // Vérifie si le mob doit changer de direction
        if ((player.position.x > transform.position.x && !movingRight) ||
            (player.position.x < transform.position.x && movingRight))
        {
            Flip();
        }
    }

    void Flip()
	{
    	movingRight = !movingRight;
    	spriteRenderer.flipX = !spriteRenderer.flipX;

    	boxCollider.offset = new Vector2(movingRight ? Mathf.Abs(boxCollider.offset.x) : -Mathf.Abs(boxCollider.offset.x), boxCollider.offset.y);

    	if (attackCollider != null)
    	{
        	attackCollider.offset = new Vector2(movingRight ? Mathf.Abs(attackCollider.offset.x) : -Mathf.Abs(attackCollider.offset.x), attackCollider.offset.y);
        
        	attackCollider.transform.localPosition = new Vector2(movingRight ? Mathf.Abs(attackCollider.transform.localPosition.x) : -Mathf.Abs(attackCollider.transform.localPosition.x), attackCollider.transform.localPosition.y);
    	}
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
        player = null;
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