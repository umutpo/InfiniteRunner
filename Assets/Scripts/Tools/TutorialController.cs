using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    const float JUMP_TUTORIAL_TRIGGER_POSITION = 25.0f;
    const float SLIDE_TUTORIAL_TRIGGER_POSITION = 50.0f;
    const float CHANGE_LANES_TUTORIAL_TRIGGER_POSITION = 75.0f;
    const float HUD_INTRO_TEXT_TRIGGER_POSITION = 80.0f;
    const float COMPLETE_DISH_TRIGGER_POSITION = 82.0f;
    const float END_COMPLETE_DISH_TRIGGER_POSITION = 99.9f;
    const float FINISHED_TUTORIAL_TRIGGER_POSITION = 120.0f;
    const float END_TUTORIAL_TRIGGER_POSITION = 140.0f;

    const string HUD_INTRO_TUTORIAL_TEXT = "See the dishes you are on your way to completing!";
    const string COMPLETE_DISH_TUTORIAL_TEXT = "Complete the Burger!";
    const string FINISH_TUTORIAL_TEXT = "Now, time to cook!";

    [SerializeField]
    private Sprite JumpTutorialImage;
    [SerializeField]
    private Sprite SlideTutorialImage;
    [SerializeField]
    private Sprite ChangeLanesTutorialImage;

    [SerializeField]
    private GameObject _player;
    private PlayerController _playerController;

    [SerializeField]
    private GameObject _tutorialCommand;
    private LangText _tutorialCommandLang;
    private Text _tutorialCommandText;

    [SerializeField]
    private GameObject _tutorialVisual;
    private Image _tutorialVisualImage;

    bool jumpFlag = true;
    bool slideFlag = true;
    bool changeLanesFlag = true;
    bool hudIntroFlag = true;
    bool completeDishFlag = true;
    bool endCompleteDishFlag = true;
    bool finishedTutorialFlag = true;

    void Start()
    {
        PlayerController.StopTutorial += StopTextDisplay;
        PlayerController.StopTutorial += StopImageDisplay;

        if (_player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
            _playerController.DisableAllInput();
        }

        if (_tutorialCommand != null)
        {
            _tutorialCommandLang = _tutorialCommand.GetComponent<LangText>();
            _tutorialCommandText = _tutorialCommand.GetComponent<Text>();
        }

        if (_tutorialVisual != null)
        {
            _tutorialVisualImage = _tutorialVisual.GetComponent<Image>();
        } 
    }

    void Update()
    {
        float currentPlayerPositionZ = _player.transform.position.z;
        if (jumpFlag && currentPlayerPositionZ >= JUMP_TUTORIAL_TRIGGER_POSITION)
        {
            jumpFlag = false;
            StartImageDisplay(JumpTutorialImage);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.upArrowKey));
        }
        else if (slideFlag && currentPlayerPositionZ >= SLIDE_TUTORIAL_TRIGGER_POSITION)
        {
            // TODO: ADD AN OBJECT TO SLIDE UNDER ON UNITY!!!
            slideFlag = false;
            StartImageDisplay(SlideTutorialImage);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.downArrowKey));
        }
        else if (changeLanesFlag && currentPlayerPositionZ >= CHANGE_LANES_TUTORIAL_TRIGGER_POSITION)
        {
            changeLanesFlag = false;
            StartImageDisplay(ChangeLanesTutorialImage);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.rightArrowKey));
        }
        else if (hudIntroFlag && currentPlayerPositionZ >= HUD_INTRO_TEXT_TRIGGER_POSITION)
        {
            hudIntroFlag = false;
            StartTextDisplay(HUD_INTRO_TUTORIAL_TEXT);
            StartCoroutine(_playerController.StartTutorial());
        }
        else if (completeDishFlag && currentPlayerPositionZ >= COMPLETE_DISH_TRIGGER_POSITION)
        {
            _playerController.EnableAllInput();
            completeDishFlag = false;
            StartTextDisplay(COMPLETE_DISH_TUTORIAL_TEXT);
        }
        else if (endCompleteDishFlag && currentPlayerPositionZ >= END_COMPLETE_DISH_TRIGGER_POSITION)
        {
            endCompleteDishFlag = false;
            StopTextDisplay();
        }
        else if (finishedTutorialFlag && currentPlayerPositionZ >= FINISHED_TUTORIAL_TRIGGER_POSITION)
        {
            finishedTutorialFlag = false;
            StartTextDisplay(FINISH_TUTORIAL_TEXT);
        }
        else if (currentPlayerPositionZ >= END_TUTORIAL_TRIGGER_POSITION)
        {
            SceneManager.LoadScene("GameplayScene");
        }
    }

    private void StartTextDisplay(string text)
    {
        _tutorialCommandLang.SetTextIdentifier(text);
        _tutorialCommand.SetActive(true);
    }

    private void StartImageDisplay(Sprite image)
    {
        if (image == ChangeLanesTutorialImage)
        {
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 400f);
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200f);
        } 
        else
        {
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200f);
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400f);
        }
        _tutorialVisualImage.sprite = image;
        _tutorialVisualImage.color = new Color(255f, 255f, 255f, 255f);
        _tutorialVisual.SetActive(true);
    }

    private void StopTextDisplay()
    {
        _tutorialCommandLang.SetTextIdentifier("");
        _tutorialCommand.SetActive(false);
    }

    private void StopImageDisplay()
    {
        _tutorialVisualImage.sprite = null;
        _tutorialVisualImage.color = new Color(255f, 255f, 255f, 0f);
        _tutorialVisual.SetActive(false);
    }
}
