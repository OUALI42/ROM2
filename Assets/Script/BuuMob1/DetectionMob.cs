using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionMob : MonoBehaviour
{
    private mob parentMob;

    void Awake()
    {
        parentMob = GetComponentInParent<mob>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.name == "Detection")
        {
            parentMob.StartChase(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.name == "Detection")
        {
            parentMob.StopChase();
        }
    }


}