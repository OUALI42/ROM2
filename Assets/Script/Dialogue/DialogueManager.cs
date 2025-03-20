using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    
    public Image dialogueBox; 
    public Sprite[] dialogues; 
    private int index = 0; 
    private bool isPlayerInZone = false; 

    // Si le joueur est dans la zone on peut appuyer sur H et un autre dialogue apparaît.
    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.H))
        {
            NextDialogue();
        }
    }

    // Si le joueur est dans la zone, un dialogue apparait
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            index = 0; 
            ShowDialogue();
        }
    }

    // Si le joueur n’est pas dans la zone, le dialogue n'apparaît pas.
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            dialogueBox.gameObject.SetActive(false);
        }
    }

    // Si il y a bien une image référencée alors la boîte de dialogue s’affiche.
    void ShowDialogue()
    {
        if (dialogues.Length > 0)
        {
            dialogueBox.sprite = dialogues[index];
            dialogueBox.gameObject.SetActive(true);
        }
    }

    // Tant qu'il y a une image référencée, les dialogues continuent de s'afficher, et quand il dépasse le nombre d'images référencer cela s'arrête.
    void NextDialogue()
    {
        index++; 
        if (index < dialogues.Length)
        {
            dialogueBox.sprite = dialogues[index]; 
        }
        else
        {
            dialogueBox.gameObject.SetActive(false); 
        }
    }
}