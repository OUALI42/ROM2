using UnityEngine;


// Si le joueur entre dans la zone, le code récupère son rigidbody et cela propulse le joueur dans les aires.
public class PropulsionZone : MonoBehaviour
{
    public float propulsionForce = 50f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
           
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, propulsionForce);
            }
        }
    }
}

