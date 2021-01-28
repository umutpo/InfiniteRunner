using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameOverOperation : MonoBehaviour
{
    public ScoreController scoreController;
    public PlayerController playerController;

    public string GameOverSceneName = "GameOverScene";
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.getGameOverState())
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
