using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    private Text score;
    public static int currentScore = 0;
    public static int highestScore = 0;

    void Start()
    {
        currentScore = 0;
        score = GetComponent<Text>();
        score.text = "Score: " + currentScore;
    }

    void Update()
    {
        score.text = "Score: " + currentScore;
    }

    public void updateHighScore(){
        if(currentScore > highestScore){
            highestScore = currentScore;
        }
    }
}
