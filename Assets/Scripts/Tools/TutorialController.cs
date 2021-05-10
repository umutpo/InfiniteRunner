using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    const float JUMP_TUTORIAL_TRIGGER_POSITION = 25.0f;
    const float CHANGE_LANES_TUTORIAL_TRIGGER_POSITION = 50.0f;
    const float HUD_INTRO_TEXT_TRIGGER_POSITION = 75.0f;
    const float END_TUTORIAL_TRIGGER_POSITION = 1000.0f;

    const string JUMP_TUTORIAL_TEXT = "Jump!";
    const string CHANGE_LANES_TUTORIAL_TEXT = "Move Right!";
    const string HUD_INTRO_TUTORIAL_TEXT = "See the dishes you are on your way to completing!";

    [SerializeField]
    private GameObject _player;
    private PlayerController _playerController;

    [SerializeField]
    private GameObject _tutorialCommand;
    private Text _tutorialCommandText;

    bool jumpFlag = true;
    bool changeLanesFlag = true;
    bool hudIntroFlag = true;

    void Start()
    {
        PlayerController.StopTutorial += StopTextDisplay;

        if (_player != null)
        {
            _playerController = _player.GetComponent<PlayerController>();
        }

        if (_tutorialCommand != null)
        {
            _tutorialCommandText = _tutorialCommand.GetComponent<Text>();
        }
    }

    void Update()
    {
        float currentPlayerPositionZ = _player.transform.position.z;
        if (jumpFlag && currentPlayerPositionZ >= JUMP_TUTORIAL_TRIGGER_POSITION)
        {
            jumpFlag = false;
            StartTextDisplay(JUMP_TUTORIAL_TEXT);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.upArrowKey));
        } 
        else if (changeLanesFlag && currentPlayerPositionZ >= CHANGE_LANES_TUTORIAL_TRIGGER_POSITION)
        {
            changeLanesFlag = false;
            StartTextDisplay(CHANGE_LANES_TUTORIAL_TEXT);
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.rightArrowKey));
        }
        else if (hudIntroFlag && currentPlayerPositionZ >= HUD_INTRO_TEXT_TRIGGER_POSITION)
        {
            hudIntroFlag = false;
            StartTextDisplay(HUD_INTRO_TUTORIAL_TEXT);
            StartCoroutine(_playerController.StartTutorial());
        }
        else if (_player.transform.position.z >= END_TUTORIAL_TRIGGER_POSITION)
        {
            SceneManager.LoadScene("GameplayScene");
        }
    }

    private void StartTextDisplay(string text)
    {
        _tutorialCommandText.text = text;
        _tutorialCommand.SetActive(true);
    }

    private void StopTextDisplay()
    {
        _tutorialCommandText.text = "";
        _tutorialCommand.SetActive(false);
    }
}
