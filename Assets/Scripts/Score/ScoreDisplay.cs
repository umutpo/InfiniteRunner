using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    Text score;
    private enum SceneType
    {
        Score,
        HighestScore
    };

    [SerializeField] private SceneType destinationScene;

    void Start()
    {
        score = GetComponent<Text>();
        if (destinationScene.ToString() == "Score") {
            score.text = "Score: " + ScoreController.currentScore;
        }
        else
        {
            score.text = "Highest Score: " + ScoreController.highestScore;
        }
    }


}
