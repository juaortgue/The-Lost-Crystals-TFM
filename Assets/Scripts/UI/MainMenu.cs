using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   

    public void LoadFirstScene(){
        SceneManager.LoadScene(2);
    }
    
    public void ExitGame(){
        Application.Quit();
    }

    public void LoadControlsScene(){
        SceneManager.LoadScene(1);
    }
    public void LoadMainMenuScene(){
        SceneManager.LoadScene(0);
    }
    
}
