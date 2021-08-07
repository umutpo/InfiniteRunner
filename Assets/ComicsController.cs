using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComicsController : MonoBehaviour
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

    [SerializeField]
    public List<Sprite> openingComics;

    private int currentOpeningComicIndex = 1;
    private Image backgroundImage = null;

    public void Awake()
    {
        Button thisButton = this.GetComponent<Button>();
        backgroundImage = this.transform.parent.GetComponentInChildren<Image>();

        thisButton.onClick.AddListener(() => {
            Debug.Log("Clicked button!");
            StartCoroutine(playComicsDecision());
        });
    }

    private IEnumerator playComicsDecision()
    {
        Debug.Log("Came here");
        if (currentOpeningComicIndex < 4)
        {
            Debug.Log("Chose skipComic");
            skipComic();
        }
        else
        {
            Debug.Log("Chose loadScene");
            loadScene();
        }

        yield return false;
    }

    private IEnumerator skipComic()
    {
        screenTransition.SetTrigger("start");
        if (backgroundImage != null)
        {
            currentOpeningComicIndex++;
            backgroundImage.sprite = openingComics[currentOpeningComicIndex - 1];
        }
        yield return new WaitForSecondsRealtime(1f);
    }

    private IEnumerator loadScene()
    {
        Time.timeScale = 1; // reset the time scale every time a new scene is loaded so the gameplay animations wont freeze
        screenTransition.SetTrigger("start");
        yield return new WaitForSecondsRealtime(1f);
        if (destinationScene.ToString() == "GameplayScene")
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            SceneManager.LoadScene(destinationScene.ToString());
        }
        yield return false;
    }
}
