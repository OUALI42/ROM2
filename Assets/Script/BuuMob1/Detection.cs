using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionMob1Script : MonoBehaviour
{
    private mob parentMob;
    public GameObject healthBarBroly; // Référence à la barre de vie
    void Start()
    {
        healthBarBroly.SetActive(false);
    }

    void Awake()
    {
        parentMob = GetComponentInParent<mob>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            healthBarBroly.SetActive(true);
            parentMob.StartChase(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            healthBarBroly.SetActive(false);
            parentMob.StopChase();
        }
    }
}