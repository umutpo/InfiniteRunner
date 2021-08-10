using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneTimeChange : MonoBehaviour
{
    private enum Handler
    {
        canvasToViewDuringPauseHandler,
        PauseOrCoutinueHandler
    }
    [SerializeField] private Handler handler;
    private enum SetToGameState
    {
        Pause,
        Continue
    }
    [SerializeField] private SetToGameState setToGameState;
    [SerializeField] private GameObject pauseMenuCanvas;
    private AudioListener audioListener;

    public GameObject canvasToViewDuringPause;
    public GameObject countDownText;

    private int countDownLength = 3;

    public void Start()
    {
        Button thisButton = GetComponent<Button>();
        if(handler == Handler.canvasToViewDuringPauseHandler)
        {
            thisButton.onClick.AddListener(canvasToViewDuringPauseHandler);
        } else
        {
            thisButton.onClick.AddListener(PauseOrContinueHandler);
        }

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
                CountdownController countDownController = countDownText.GetComponent<CountdownController>();
                AudioListener.pause = false;
                pauseMenuCanvas.SetActive(false);
                countDownController.StartCoroutine(Countdown(countDownLength, countDownController));
                break;
        }
    }

    public void canvasToViewDuringPauseHandler ()
    {
        switch (setToGameState)
        {
            case SetToGameState.Pause:
                canvasToViewDuringPause.SetActive(true);
                pauseMenuCanvas.SetActive(false);
                break;
            case SetToGameState.Continue:
                canvasToViewDuringPause.SetActive(false);
                pauseMenuCanvas.SetActive(true);
                break;
        }
    }

    IEnumerator Countdown(int seconds, CountdownController countdownController)
    {
        TMPro.TextMeshProUGUI countdownText = countdownController.countdownText;
            int counter = seconds;
            while (counter >= 0)
            {
                if (counter == 0)
                {
                    countdownText.text = "Start!";
                }
                else
                {
                    countdownText.text = counter.ToString();
                }
                yield return new WaitForSecondsRealtime(1);
                counter--;
            }
            countdownText.text = "";
            Time.timeScale = 1f;
    }

}
