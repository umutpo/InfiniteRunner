using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneSwitcher : MonoBehaviour
{
    private enum SceneType {
        MainMenu,
        Settings,
        GameplayScene
    };

    [SerializeField] private SceneType destinationScene;

   public void Start() {
       Button thisButton = GetComponent<Button>();
       thisButton.onClick.AddListener(() => SceneManager.LoadScene(destinationScene.ToString()));
    }
}
