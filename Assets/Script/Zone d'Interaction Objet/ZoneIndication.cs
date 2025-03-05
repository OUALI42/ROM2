using UnityEngine;

public class ZoneIndication : MonoBehaviour
{
    public GameObject panel; // Le panel à activer/désactiver
    private bool isPlayerInZone = false;

    // Cette fonction est appelée quand un objet entre dans la zone de collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur qui entre
        {
            isPlayerInZone = true;
            ShowPanel(true); // Affiche le panel
        }
    }

    // Cette fonction est appelée quand un objet quitte la zone de collision
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Vérifie si c'est le joueur qui quitte
        {
            isPlayerInZone = false;
            ShowPanel(false); // Cache le panel
        }
    }

    // Active ou désactive le panel
    private void ShowPanel(bool show)
    {
        panel.SetActive(show);
    }

    // Pour être sûr que le panel est désactivé au début
    private void Start()
    {
        panel.SetActive(false);
    }
}