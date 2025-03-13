using UnityEngine;

public class AudioTroll : MonoBehaviour
{
    public AudioClip[] playlist;
    public AudioSource audioSource;

    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    void Update()
    {
        
    }
}
