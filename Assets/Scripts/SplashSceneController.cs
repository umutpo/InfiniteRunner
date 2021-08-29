using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashSceneController : MonoBehaviour
{
    private enum SceneType
    {
        MainMenu,
        Settings,
        GameplayScene,
        CreditsScene,
        TutorialScene,
        RecipeScene
    };

    public Animator screenTransition;

    [SerializeField]
    private SceneType destinationScene;

    private float timer = 0f;
    private float automaticSkipTime = 2f;

    public void Awake()
    {

    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > automaticSkipTime)
        {
            StartCoroutine(loadScene());
        }
    }

    private IEnumerator loadScene()
    {
        AudioListener.pause = false; // reset mute state every time when switching to a new scene
        Time.timeScale = 1; // reset the time scale every time a new scene is loaded so the gameplay animations wont freeze
        screenTransition.SetTrigger("start");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(destinationScene.ToString());
        yield return false;
    }
}
