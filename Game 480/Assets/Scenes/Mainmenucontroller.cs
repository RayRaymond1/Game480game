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

       Application.Quit();

    }
}
