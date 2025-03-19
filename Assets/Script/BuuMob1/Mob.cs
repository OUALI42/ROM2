using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mob : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public Transform pointA;  // Point de départ de la patrouille
    public Transform pointB;  // Point d’arrivée de la patrouille
    public Transform player;  // Référence du joueur

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private BoxCollider2D attackCollider;

    private Transform targetPoint;  // Prochain point de patrouille
    public bool isChasing = false;
    private bool isMoving = true; // Variable pour gérer l'état du mouvement
    private Vector2 directionInitiale = new Vector2(1, 0);
    public float vitesse = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (rb == null)
            Debug.LogError(gameObject.name + " n'a pas de Rigidbody2D !");
        
        if (spriteRenderer == null)
            Debug.LogError(gameObject.name + " n'a pas de SpriteRenderer !");
        
        // Récupérer le BoxCollider de detection
        Transform colliderChild = transform.Find("Detection");
        if (colliderChild != null)
        {
            boxCollider = colliderChild.GetComponent<BoxCollider2D>();
            if (boxCollider == null)
                Debug.LogError("Detection existe mais n'a pas de BoxCollider2D !");
        }
        else
        {
            Debug.LogError("Detection est introuvable sous " + gameObject.name);
        }

        // Récupérer le BoxCollider de zoneAttack
        Transform attackColliderChild = transform.Find("ZonAttack");
        if (attackColliderChild != null)
        {
            attackCollider = attackColliderChild.GetComponent<BoxCollider2D>();
            if (attackCollider == null)
                Debug.LogError("ZonAttack existe mais n'a pas de BoxCollider2D !");
        }
        else
        {
            Debug.LogError("ZonAttack est introuvable sous " + gameObject.name);
        }

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Initialisation du mouvement vers le point A
        targetPoint = pointA;
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (targetPoint == null) return;

        float moveDirection = (targetPoint.position.x > transform.position.x) ? 1 : -1;
        rb.velocity = new Vector2(moveDirection * patrolSpeed, rb.velocity.y);

        // Retourner le sprite selon la direction
        spriteRenderer.flipX = moveDirection < 0;

        // Vérifier si l'ennemi est proche du point cible
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA; // Changer de point
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        float direction = (player.position.x > transform.position.x) ? 1 : -1;
        rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

        spriteRenderer.flipX = direction < 0;
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
        isMoving = false;
        
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
    }
    
    public void ResumeMovement(float speed)
    {
        isMoving = true;
        
        GetComponent<Rigidbody2D>().velocity = directionInitiale * vitesse;
    }
    
    
}




