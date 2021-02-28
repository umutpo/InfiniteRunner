using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameOverOperation : MonoBehaviour
{
    public ScoreController scoreController;
    public PlayerController playerController;

    public string GameOverSceneName = "GameOverScene";

    void Start()
    {
    }

    void Update()
    {
        if(playerController.GetGameOverState())
        {
            gameOverOperations();
            enabled = false;
        }
    }


    public void gameOverOperations()
    {
        scoreController.updateHighScore();
        SceneManager.LoadScene(GameOverSceneName);
    }
}
