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
    public void LoadMenu() {
        SceneManager.LoadScene("mainmenutitle");
    }
    public void Gameover() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }


    public static List<string> sceneList = new List<string>();

    public static void LoadScene(string sceneName)
    {
        // Add the current scene to the list
        sceneList.Add(SceneManager.GetActiveScene().name);

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    public static void LoadPreviousScene()
    {
        // Get the second last scene from the list
        string previousScene = sceneList[sceneList.Count - 2];

        // Remove the last scene from the list
        sceneList.RemoveAt(sceneList.Count - 1);

        // Load the previous scene
        SceneManager.LoadScene(previousScene);
    }
}


