using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneSwitchOnclick : MonoBehaviour
{
    // When a new scene is made, ensure the new scenetype enum value has the same name as the scene
    #region Scene Enumerations
    private enum SceneType {
        Settings,
        GameplayScene,
        MainMenu
    }
    #endregion
    
    [SerializeField] private SceneType destinationScene = SceneType.MainMenu;
    private Button thisButton;
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => SceneManager.LoadScene(destinationScene.ToString()));
    }
}
