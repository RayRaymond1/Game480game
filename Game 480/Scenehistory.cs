using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
//Code for Mainmenu controls when three levels are finished

public class SceneHistory : MonoBehaviour
{
    private static List<int> sceneHistory = new List<int>(); // Running history of scenes

    public static void LoadScene(int buildIndex)
    {
        // Save the current scene to the history before loading the next one
        sceneHistory.Add(SceneManager.GetActiveScene().buildIndex);

        // Load the requested scene
        SceneManager.LoadScene(buildIndex);
    }

    public static void LoadPreviousScene()
    {
        if (sceneHistory.Count > 0)
        {
            // Get the last scene from the history
            int lastSceneIndex = sceneHistory[sceneHistory.Count - 1];

            // Remove the last scene from the history
            sceneHistory.RemoveAt(sceneHistory.Count - 1);

            // Load the last scene
            SceneManager.LoadScene(lastSceneIndex);
        }
        else
        {
            Debug.LogWarning("No previous scene in history");
        }
    }
}
