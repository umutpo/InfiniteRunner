using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    Text score;
    private enum SceneType
    {
        Score,
        HighestScore,
        MainMenuHighestScore
    };

    [SerializeField] private SceneType destinationScene;

    void Start()
    {
        score = GetComponent<Text>();
        if (destinationScene.ToString() == "Score") {
            score.text = ScoreController.currentScore.ToString();
        }
        else if (destinationScene.ToString() == "HighestScore") 
        {
            score.text = ScoreController.highestScore.ToString();
        }
        else
        {
            score.text = ScoreController.highestScore.ToString();
        }
    }
}
