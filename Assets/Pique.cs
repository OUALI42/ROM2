using UnityEngine;

public class Pique : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            KillPlayer(other.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
        player.SetActive(false);
       Debug.Log("Le joueur est mort !");
    }
}