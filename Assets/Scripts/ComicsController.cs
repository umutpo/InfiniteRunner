﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private int currentOpeningComicIndex = 1;
    private float timer = 0f;
    private float automaticSkipTime = 2f;
    private Image backgroundImage = null;
    private LangText subtitles = null;
    private Button thisButton = null;

    public void Awake()
    {
        thisButton = this.GetComponent<Button>();
        backgroundImage = this.GetComponentInChildren<Image>();
        subtitles = this.GetComponentInChildren<LangText>();

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