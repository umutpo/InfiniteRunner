﻿using System.Collections;
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
        RecipeScene
    };

    [SerializeField] 
    private SceneType destinationScene;

    public Animator screenTransition;
    public void Start() {
        Button thisButton = GetComponent<Button>();
       thisButton.onClick.AddListener(() => {
        StartCoroutine(loadScene());
       });
    }

    private IEnumerator loadScene()
    {
        screenTransition.SetTrigger("start");
        yield return new WaitForSeconds(1f);
        if (destinationScene.ToString() == "GameplayScene" && isFirstTimePlaying())
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            SceneManager.LoadScene(destinationScene.ToString());
        }
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
