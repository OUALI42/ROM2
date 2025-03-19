using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image dialogueBox; // L'image affichant le dialogue
    public Sprite[] dialogues; // Tableau contenant toutes les images des dialogues
    private int index = 0; // Index du dialogue actuel
    private bool isPlayerInZone = false; // Vérifie si le joueur est dans la zone

    void Update()
    {
        // Vérifie si le joueur est dans la zone et appuie sur H
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.H))
        {
            NextDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si le joueur est entré dans la zone
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            index = 0; // Réinitialise le dialogue
            ShowDialogue();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Désactive le dialogue quand le joueur quitte la zone
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            dialogueBox.gameObject.SetActive(false);
        }
    }

    void ShowDialogue()
    {
        if (dialogues.Length > 0)
        {
            dialogueBox.sprite = dialogues[index];
            dialogueBox.gameObject.SetActive(true);
        }
    }

    void NextDialogue()
    {
        index++; // Passe à l'image suivante
        if (index < dialogues.Length)
        {
            dialogueBox.sprite = dialogues[index]; // Change l'image affichée
        }
        else
        {
            dialogueBox.gameObject.SetActive(false); // Cache le dialogue quand c'est fini
        }
    }
}
