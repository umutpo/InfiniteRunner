using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudioPlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<string> IN_GAME_SOUNDTRACK_SCENES;
    [SerializeField]
    private List<string> MENU_SOUNDTRACK_SCENES;

    private static MenuAudioPlayerManager instance = null;

    [SerializeField]
    private AudioSource menuSoundtrack;
    [SerializeField]
    private AudioSource inGameSoundtrack;

    private string currentSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (instance == this) return;
        Destroy(gameObject);
    }

    void Start()
    {
        menuSoundtrack.Play();
        inGameSoundtrack.Play();

        PauseMenuSoundtrack();
        PauseInGameSoundtrack();
    }

    void Update()
    {
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != currentSceneName)
        {
            currentSceneName = newSceneName;
            if(IN_GAME_SOUNDTRACK_SCENES.Contains(currentSceneName))
            {
                PauseMenuSoundtrack();
                UnpauseInGameSoundtrack();
            } 
            else if (MENU_SOUNDTRACK_SCENES.Contains(currentSceneName))
            {
                UnpauseMenuSoundtrack();
                PauseInGameSoundtrack();
            } 
            else
            {
                PauseMenuSoundtrack();
                PauseInGameSoundtrack();
            }
        }
    }

    public void PauseMenuSoundtrack()
    {
        menuSoundtrack.Pause();
    }

    public void UnpauseMenuSoundtrack()
    {
        menuSoundtrack.UnPause();
    }

    public void PauseInGameSoundtrack()
    {
        inGameSoundtrack.Pause();
    }

    public void UnpauseInGameSoundtrack()
    {
        inGameSoundtrack.UnPause();
    }
}
