using UnityEngine;
using UnityEngine.SceneManagement;
//
public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonMenuSound;
    public AudioClip ButtonPlaySound;
    
    
   
    public string levelToLoad ;

    public GameObject settingsWindow;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    //on joue la musique mise en paramètre et on lance le bruitage des boutons//
    public void StartGame(){

        audioSource.clip = ButtonPlaySound;
        audioSource.Play();
        SceneManager.LoadScene("selectLevel");

    }
    
    //bruitage des boutons et on affiche le panels des paramètres//
    public void SettingsButton(){
        audioSource.clip = buttonMenuSound;
        audioSource.Play();
        settingsWindow.SetActive(true);
    }

    //ici meme chose mais on desactive le panel//
    public void CloseSettingsMenu(){
        audioSource.clip = buttonMenuSound;
        audioSource.Play();
        settingsWindow.SetActive(false);
    }
    
    //on charge simplement la scène des credits//
    public void OpenCredits(){
        SceneManager.LoadScene("credits 1");
        
    }


    //bruitages des boutons + on appelle une fonction de Unity pour fermer le programme//
    public void QuitGame(){
        audioSource.clip = buttonMenuSound;
        audioSource.Play();
        Application.Quit();
    }
    
}