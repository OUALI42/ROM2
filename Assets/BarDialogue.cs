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

    void ShowPressHImage()
    {
        if (pressHImage != null)
        {
            pressHImage.gameObject.SetActive(true);
        }
    }
}
