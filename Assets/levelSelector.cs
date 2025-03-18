using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelector : MonoBehaviour
{
    public void LoadLevelPassed(string levelName)
    {
        
        SceneManager.LoadScene(levelName);
    }
    
    public void LoadMainMenu(){
        
        SceneManager.LoadScene("MainMenu");
    }
    
}
