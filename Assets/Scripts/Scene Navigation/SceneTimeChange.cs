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

    public GameObject canvasToViewDuringPause;
    public GameObject countDownText;
    public CountdownController countDownController;

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
                countDownController.StopAllCoroutines();
                break;
            case SetToGameState.Continue:
                pauseMenuCanvas.SetActive(false);
                countDownController.StartCountdown();
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

}
