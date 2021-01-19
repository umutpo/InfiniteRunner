using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
private Text score;
private int currentScore = 0;
private static int highestScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
        score.text = "Score: " + currentScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// This will replace highest score with currentScore if currrentScore is higher
public void updateHighScore(){
    if(currentScore > highestScore){
    highestScore = currentScore;
    }
}

//This will set currentScore to the given score integer
public void setCurrentScore(int score){
    currentScore = score;
}

//return highestScore
public int getHighestScore(){
    return highestScore;
}

//return currentScore
public int getCurrentScore(){
    return currentScore;
}
}
