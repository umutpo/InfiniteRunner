using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ComicsController : MonoBehaviour
{
    private const string FIRST_TIME_PLAYING_KEY = "FirstTimePlaying";

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
    [SerializeField]
    public List<string> openingComicsSubtitles;

    [SerializeField]
    public TMP_FontAsset defaultFont;
    [SerializeField]
    public TMP_FontAsset turkishFont;

    private int currentOpeningComicIndex = 1;
    private float timer = 0f;
    private float automaticSkipTime = 4f;
    private Image backgroundImage = null;
    private LangText subtitles = null;
    private TextMeshProUGUI textBox = null;
    private Button thisButton = null;

    public void Awake()
    {
        thisButton = this.GetComponent<Button>();
        GameObject comic = GameObject.Find("Comic");
        if (!comic)
        {
            Debug.LogError("Object named Comic does not exist in the scene. Ensure that this exists and it contains both the subtitles and comic image background as a child");
        }
        backgroundImage = comic.GetComponentInChildren<Image>();
        subtitles = comic.GetComponentInChildren<LangText>();
        textBox = comic.GetComponentInChildren<TextMeshProUGUI>();

        string language = subtitles.GetLanguage();
        if (language == "Turkish")
        {
            textBox.font = turkishFont;
            textBox.fontStyle = FontStyles.Bold;
        } else {
            textBox.font = defaultFont;
            textBox.fontStyle = FontStyles.Normal;
        }

        thisButton.onClick.AddListener(() => {
            selectComicRoutine();
        });
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > automaticSkipTime)
        {
            selectComicRoutine();
        }
    }

    private void selectComicRoutine()
    {
        if (currentOpeningComicIndex < openingComics.Count)
        {
            StartCoroutine(skipComic());
        }
        else
        {
            StartCoroutine(loadScene());
        }
    }
    public void completelySkipComic() {
        StartCoroutine(loadScene());
    }

    private IEnumerator skipComic()
    {
        timer = 0;
        currentOpeningComicIndex++;
        backgroundImage.sprite = openingComics[currentOpeningComicIndex - 1];
        subtitles.SetTextIdentifier(openingComicsSubtitles[currentOpeningComicIndex - 1]);
        if (currentOpeningComicIndex >= openingComics.Count)
        {
            thisButton.transition = Selectable.Transition.None;
        }
        yield return false;
    }

    private IEnumerator loadScene()
    {
        timer = 0;
        Time.timeScale = 1; // reset the time scale every time a new scene is loaded so the gameplay animations wont freeze
        screenTransition.SetTrigger("start");
        yield return new WaitForSecondsRealtime(1f);
        if (isFirstTimePlaying())
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
