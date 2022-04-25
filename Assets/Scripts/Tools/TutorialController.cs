using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class TutorialController : MonoBehaviour
{
    const float JUMP_TUTORIAL_TRIGGER_POSITION = 25.0f;
    const float SLIDE_TUTORIAL_TRIGGER_POSITION = 50.0f;
    const float CHANGE_LANES_TUTORIAL_TRIGGER_POSITION = 68.0f;
    const float INGREDIENT_WEIGH_DOWN_TEXT_TRIGGER_POSITION = 72.5f;
    const float HUD_INTRO_TEXT_TRIGGER_POSITION = 80.0f;
    const float COMPLETE_DISH_TRIGGER_POSITION = 82.0f;
    const float END_COMPLETE_DISH_TRIGGER_POSITION = 99.9f;
    const float FINISHED_TUTORIAL_TRIGGER_POSITION = 120.0f;
    const float END_TUTORIAL_TRIGGER_POSITION = 140.0f;

    const string TUTORIAL_INTRO_TEXT = "TUTORIAL_INTRO_TEXT";
    const string INGREDIENT_WEIGH_DOWN_TEXT = "INGREDIENT_WEIGH_DOWN_TEXT";
    const string HUD_INTRO_TUTORIAL_TEXT = "HUD_INTRO_TUTORIAL_TEXT";
    const string COMPLETE_DISH_TUTORIAL_TEXT = "COMPLETE_DISH_TUTORIAL_TEXT";
    const string FINISH_TUTORIAL_TEXT = "FINISH_TUTORIAL_TEXT";

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

    [SerializeField]
    private GameObject _tutorialVisual;
    private Image _tutorialVisualImage;

    [SerializeField]
    private TextMeshProUGUI textBox = null;
    [SerializeField]
    public TMP_FontAsset defaultFont;
    [SerializeField]
    public TMP_FontAsset turkishFont;

    public Animator screenTransition;

    bool jumpFlag = true;
    bool slideFlag = true;
    bool changeLanesFlag = true;
    bool ingredientWeighDownFlag = true;
    bool hudIntroFlag = true;
    bool completeDishFlag = true;
    bool endCompleteDishFlag = true;
    bool finishedTutorialFlag = true;
    bool helloFlag = true;

    void Start()
    {
        PlayerController.StopTutorial += StopTextDisplay;
        PlayerController.StopTutorial += StopImageDisplay;


        if (_player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
            _playerController.DisableAllInput();
            _playerController.EnableTouchInput();
        }

        if (_tutorialCommand != null)
        {
            _tutorialCommandLang = _tutorialCommand.GetComponent<LangText>();
        }

        if (_tutorialVisual != null)
        {
            _tutorialVisualImage = _tutorialVisual.GetComponent<Image>();
        }

        if (textBox != null)
        {
            string language = _tutorialCommandLang.GetLanguage();
            if (language == "Turkish")
            {
                textBox.font = turkishFont;
                textBox.fontStyle = FontStyles.Bold;
            }
            else
            {
                textBox.font = defaultFont;
                textBox.fontStyle = FontStyles.Normal;
            }
        }
    }

    void Update()
    {
        float currentPlayerPositionZ = _player.transform.position.z;
        if (helloFlag)
        {
            helloFlag = false;
            StartTextDisplay(TUTORIAL_INTRO_TEXT);
            StartCoroutine(_playerController.StartTutorial());
        }
        else if (jumpFlag && currentPlayerPositionZ >= JUMP_TUTORIAL_TRIGGER_POSITION)
        {
            jumpFlag = false;
            StartImageDisplay(JumpTutorialImage);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.upArrowKey, PlayerController.SwipeAction.Up));
        }
        else if (slideFlag && currentPlayerPositionZ >= SLIDE_TUTORIAL_TRIGGER_POSITION)
        {
            // TODO: ADD AN OBJECT TO SLIDE UNDER ON UNITY!!!
            slideFlag = false;
            StartImageDisplay(SlideTutorialImage);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.downArrowKey, PlayerController.SwipeAction.Down));
        }
        else if (changeLanesFlag && currentPlayerPositionZ >= CHANGE_LANES_TUTORIAL_TRIGGER_POSITION)
        {
            changeLanesFlag = false;
            StartImageDisplay(ChangeLanesTutorialImage);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.rightArrowKey, PlayerController.SwipeAction.Right));
        }
        else if (ingredientWeighDownFlag && currentPlayerPositionZ >= INGREDIENT_WEIGH_DOWN_TEXT_TRIGGER_POSITION)
        {
            ingredientWeighDownFlag = false;
            StartTextDisplay(INGREDIENT_WEIGH_DOWN_TEXT);
            StartCoroutine(_playerController.StartTutorial());
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
            StartCoroutine(loadGamePlayScene());
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
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 350f);
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 170f);
        } 
        else
        {
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 170f);
            _tutorialVisualImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 350f);
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
    private IEnumerator loadGamePlayScene()
    {
    screenTransition.SetTrigger("start");
    yield return new WaitForSeconds(1f);
    SceneManager.LoadScene("GameplayScene");
    }
}
