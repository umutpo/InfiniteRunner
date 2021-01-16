using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour {
    public static List<string> SceneNames;
    private void Start() {
        SceneNames = new List<string>();
        AddSceneNames();
    }

    private static void AddSceneNames() {
        SceneNames.Add("Settings");
        SceneNames.Add("MainMenu");
        SceneNames.Add("Game");
    }


    public void SwitchScene(string newScene) {
        bool switchedScene = false;
        foreach (string s in SceneNames) {
            if (newScene == s) {
                SceneManager.LoadScene(newScene);
                switchedScene = true;
            }
        }
        if (!switchedScene) {
            Debug.LogError("ERROR: Name selected of scene to switch to doesn't exist. Please check the button onclick for typos, or otherwise update sceneswitcher.cs' SceneNames list for newly added scenes using AddSceneNames()");
        }
    }
    
}
