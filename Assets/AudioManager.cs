using UnityEngine;

public class AudioManagerScene : MonoBehaviour
{
    private static AudioManagerScene instance;

    void Awake()
    {
        // Si une musique est déjà en cours, on détruit celle qui vient d'être créée
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Sinon, on garde cet AudioManager et on empêche sa destruction
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
