using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image dialogueImage; // Image UI où s'affiche le dialogue
    public Sprite[] dialogues; // Liste des images de dialogue
    private int currentDialogueIndex = 0; // Index du dialogue actuel
    private bool isPlayerInZone = false; // Vérifie si le joueur est dans la zone

    void Start()
    {
        dialogueImage.gameObject.SetActive(false); // Cache l’image au début
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.H)) // Appui sur "H" pour changer de dialogue
        {
            NextDialogue();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Vérifie si le joueur entre dans la zone
        {
            isPlayerInZone = true;
            ShowDialogue();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Si le joueur sort de la zone
        {
            isPlayerInZone = false;
            dialogueImage.gameObject.SetActive(false);
            currentDialogueIndex = 0; // Réinitialise le dialogue
        }
    }

    void ShowDialogue()
    {
        if (dialogues.Length > 0)
        {
            dialogueImage.sprite = dialogues[currentDialogueIndex];
            dialogueImage.gameObject.SetActive(true);
        }
    }

    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Length - 1)
        {
            currentDialogueIndex++;
            dialogueImage.sprite = dialogues[currentDialogueIndex];
        }
        else
        {
            dialogueImage.gameObject.SetActive(false); // Cache le dialogue après le dernier
        }
    }
}
