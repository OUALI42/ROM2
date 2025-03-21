using UnityEngine;

public class attackLuffy : MonoBehaviour
{
    public int damage; // Dégâts infligés

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("mob")) // Vérifie si c'est un ennemi
        {
            MorganHealth morganHealth2 = other.GetComponent<MorganHealth>();
            if (morganHealth2 != null)
            {
                morganHealth2.TakeDamage(damage);
            }
        }

    }
}
