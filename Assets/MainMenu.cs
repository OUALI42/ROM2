using UnityEngine;
using UnityEngine.SceneManagement;
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
    public void StartGame(){

        audioSource.clip = ButtonPlaySound;
        audioSource.Play();
        SceneManager.LoadScene("bar");

    }
    public void SettingsButton(){
        audioSource.clip = buttonMenuSound;
        audioSource.Play();
        settingsWindow.SetActive(true);
    }

    public void CloseSettingsMenu(){
        audioSource.clip = buttonMenuSound;
        audioSource.Play();
        settingsWindow.SetActive(false);
    }
    
    public void OpenCredits(){
        SceneManager.LoadScene("credits 1");
        
    }


    public void QuitGame(){
        audioSource.clip = buttonMenuSound;
        audioSource.Play();
        Application.Quit();
    }
    
}