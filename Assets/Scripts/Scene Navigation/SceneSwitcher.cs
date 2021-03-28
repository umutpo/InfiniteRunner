using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class SceneSwitcher : MonoBehaviour
{
    // When new scenes are added, add their name to the enum list below verbatim
    private enum SceneType {
        MainMenu,
        Settings,
        GameplayScene,
        CreditsScene,
        TutorialScene,
        RecipeScene
    };

    [SerializeField] private SceneType destinationScene;

   public void Start() {
       Button thisButton = GetComponent<Button>();
       thisButton.onClick.AddListener(() => {
           loadScene();
       });
    }


    private void loadScene()
    {
        if (destinationScene.ToString() == "GameplayScene")
        {
            if (isFirstTimePlaying())
            {
                SceneManager.LoadScene("TutorialScene");
            }
            else
            {
                SceneManager.LoadScene(destinationScene.ToString());
            }
        }
        else
        {
            SceneManager.LoadScene(destinationScene.ToString());
        }
    }
    private bool isFirstTimePlaying()
    {
        if (!PlayerPrefs.HasKey("FirstTimePlaying"))
        {
            PlayerPrefs.SetInt("FirstTimePlaying", 1);
            return true;
        }
        else
        {
            return false;
        }
    }
}
