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
    public Transform player;
    public Text scoreText;
    public float scoreCount;
    public Typer score = new Typer();
    // Update is called once per frame
    void Update()
    {
        score.CalculateScore();
        scoreText.text = "Score "+Mathf.Round(score.score);
        
    }
}
