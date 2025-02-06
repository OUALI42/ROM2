using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour{

    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI ;


    void Update(){

         if(Input.GetKeyDown(KeyCode.Escape)){

            if(gameIsPaused){

                Resume();
            }
            else{

                Pause();
            }
         }
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void Resume(){
    
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void LoadMainMenu(){

        
        Resume();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame(){

        Application.Quit();
    }

    public void Replay(){
        
        Resume();
        SceneManager.LoadScene("zone1");
    }
    
}
