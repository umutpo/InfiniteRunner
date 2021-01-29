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
        score = GetComponent<Text>();
        score.text = "Score: " + currentScore;
    }

    void Update()
    {
        
    }

    public void updateHighScore(){
        if(currentScore > highestScore){
        highestScore = currentScore;
        }
    }

    public void setCurrentScore(int score){
        currentScore = score;
    }

    public int getHighestScore(){
        return highestScore;
    }

    public int getCurrentScore(){
        return currentScore;
    }
}
