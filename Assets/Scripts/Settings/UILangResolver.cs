﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UILangResolver : MonoBehaviour
{
    static UILangResolver instance = null;
    private LangResolver _langResolver;

    private void Awake() {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        _langResolver = FindObjectOfType<LangResolver>();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        _langResolver.ResolveTexts();
        Debug.Log(String.Equals(scene.name, "Settings"));
        if (String.Equals(scene.name, "Settings"))
        {
            GameObject dropdownObject = GameObject.Find("Dropdown");
            Debug.Log("Dropdown object is " + dropdownObject);
            if (dropdownObject == null)
                Debug.LogError("Object named Dropdown to represent languages dropdown does not exist. Rename to fix this.");
            Dropdown d = dropdownObject.GetComponent<Dropdown>();
            if (d == null)
                Debug.LogError("Object named Dropdown to represent languages does not have a dropdown UI component attached on it. Add the dropdown component back to fix this.");
            d.onValueChanged.AddListener(delegate {
                ChangeLanguage(d);
            });
            d.value = d.options.FindIndex(option => option.text == PrefsHolder.GetLang());
        }
    }
    public void ChangeLanguage(Dropdown d)
    {
        _langResolver.ChangeLanguage(d.options[d.value].text);
        Debug.Log("Language changed to " + d.options[d.value].text);
    }
}
