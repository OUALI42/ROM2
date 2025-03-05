using UnityEngine;

public class BlockInteraction : MonoBehaviour
{
    private bool playerInRange = false; // Le joueur est à portée ?

    void Update()
    {
        // Si le joueur est proche et appuie sur la touche "A"
        if (playerInRange && Input.GetKeyDown(KeyCode.D))
        {
            Destroy(gameObject); // Supprime le bloc
        }
    }

    // Détecte si le joueur entre en contact avec le bloc
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Vérifie que c'est bien le joueur
        {
            playerInRange = true;
        }
    }

    // Détecte si le joueur quitte le contact avec le bloc
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
