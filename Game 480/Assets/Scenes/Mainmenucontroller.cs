using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenucontroller : MonoBehaviour
{
    public CanvasGroup OptionPanel;

    public void Playgame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public void Option() {

        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;

    }
    public void Back()
    {

        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;

    }
    public void QuitGame() {
        #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif


       Application.Quit();

    }
    public void LoadMenu() {
        SceneManager.LoadScene("mainmenutitle");
    }
    public void Gameover() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
}
