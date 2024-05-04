using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCredits : MonoBehaviour
{
    public void LoadCreditsScene()
    {
        // Load the credits scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }
}
