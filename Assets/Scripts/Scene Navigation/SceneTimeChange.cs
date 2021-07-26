using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneTimeChange : MonoBehaviour
{
    private enum SetToGameState
    {
        Pause,
        Continue
    }
    [SerializeField] private SetToGameState setToGameState;
    [SerializeField] private GameObject pauseMenuCanvas;
    private AudioListener audioListener;

    public void Start()
    {
        Button thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(PauseOrContinueHandler);
    }

    public void PauseOrContinueHandler()
    {
        switch (setToGameState)
        {
            case SetToGameState.Pause:
                Time.timeScale = 0f;
                AudioListener.pause = true;
                pauseMenuCanvas.SetActive(true);
                break;
            case SetToGameState.Continue:
                Time.timeScale = 1f;
                AudioListener.pause = false;
                pauseMenuCanvas.SetActive(false);
                break;
        }
    }
}
