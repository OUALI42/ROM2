using UnityEngine;

public class ZoneDialogue : MonoBehaviour
{
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.SendMessage("ShowDialogue");
        }
    }
}
