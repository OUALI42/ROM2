using UnityEngine;

public class PropulsionZone : MonoBehaviour
{
    public float propulsionForce = 1000f;

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