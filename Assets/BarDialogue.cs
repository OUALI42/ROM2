using UnityEngine;
using UnityEngine.UI;

public class BarDialogue : MonoBehaviour
{
    public Image dialogueBox; 
    public Sprite[] dialogues; 
    public Image pressHImage;
    private int index = 0; 
    private bool isPlayerInZone = false; 
    private bool dialogueStarted = false;

    // Si on est dans la zone, on appuie sur h et le dialogue peut commencer sinon non
    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.H))
        {
            if (!dialogueStarted)
            {
                dialogueStarted = true;
                ShowDialogue();
            }
            else
            {
                NextDialogue();
            }
        }
    }

    // Si le joueur est dans la zone on peut appuyer sur h
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            index = 0;
            dialogueStarted = false;
            ShowPressHImage();
        }
    }

    // Si le joueur n'est pas dans la zone on ne peut appuyer sur h
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            dialogueBox.gameObject.SetActive(false);
            pressHImage.gameObject.SetActive(false);
        }
    }

    void ShowDialogue()
    {
        if (index >= 0 && index < dialogues.Length)
        {
            dialogueBox.sprite = dialogues[index];
            dialogueBox.gameObject.SetActive(true);
            pressHImage.gameObject.SetActive(false);
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
            pressHImage.gameObject.SetActive(false);
            dialogueStarted = false;
            index = 0; 
        }
    }

    // L'image "Appuyer sur H" passe avant les autre images
    void ShowPressHImage()
    {
        if (pressHImage != null)
        {
            pressHImage.gameObject.SetActive(true);
        }
    }
}
