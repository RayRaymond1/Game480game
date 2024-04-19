using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text scoreText;
    public EventManager EventManagerObject;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score "+ EventManagerObject.score.ToString();
        
    }
}
