using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameOverOperation : MonoBehaviour
{
    public ScoreController scoreController;
    public PlayerController playerController;
    public Animator screenTransition;

    private const string GameOverSceneName = "GameOverScene";

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
        StartCoroutine(loadGameOverScene());
    }

    private IEnumerator loadGameOverScene()
    {
        screenTransition.SetTrigger("start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(GameOverSceneName);
    }
}
