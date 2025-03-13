using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class firstdialogur : MonoBehaviour
{
    public Dialogue dialogue;

    public bool isInRange;

    private TextMeshProUGUI interactUI;


    private void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {

        /*if (isInRange)
        {
            Debug.Log("Player is in range");
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Key M pressed");
                TriggerDialogue();
            }
        }*/
        if(isInRange && Input.GetKeyDown(KeyCode.D))
        {
            TriggerDialogue();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isInRange = true;
            //Debug.Log("Player entered dialogue range");
            interactUI.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isInRange = false;
            //Debug.Log("Player exited dialogue range");
            interactUI.enabled = false;

        }
    }

    void TriggerDialogue()
    {
        /*if (DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager.instance is null");
            return;
        }*/

        DialogueManager.instance.StartDialogue(dialogue);
    }
}