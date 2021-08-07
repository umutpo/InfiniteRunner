using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneSwitcher : MonoBehaviour
{
    private const string FIRST_TIME_PLAYING_KEY = "FirstTimePlaying";

    private enum SceneType {
        MainMenu,
        Settings,
        GameplayScene,
        CreditsScene,
        TutorialScene,
        RecipeScene,
        ComicScene
    };

    [SerializeField] 
    private SceneType destinationScene;

    public Animator screenTransition;
    public void Awake() {
        Button thisButton = GetComponent<Button>();
       thisButton.onClick.AddListener(() => {
            StartCoroutine(loadScene());
       });
    }

    private IEnumerator loadScene()
    {
        Time.timeScale = 1; // reset the time scale every time a new scene is loaded so the gameplay animations wont freeze
        screenTransition.SetTrigger("start");
        yield return new WaitForSecondsRealtime(1f);
        if (destinationScene.ToString() == "GameplayScene" && isFirstTimePlaying())
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            SceneManager.LoadScene(destinationScene.ToString());
        }
        yield return false;
    }

    private bool isFirstTimePlaying()
    {
        bool isFirstTime = !PlayerPrefs.HasKey(FIRST_TIME_PLAYING_KEY);
        if (isFirstTime)
        {
            PlayerPrefs.SetInt(FIRST_TIME_PLAYING_KEY, 1);
        }

        return isFirstTime;
    }

}
