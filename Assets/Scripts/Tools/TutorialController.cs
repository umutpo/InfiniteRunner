using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    const float JUMP_TUTORIAL_TRIGGER_POSITION = 25.0f;
    const float CHANGE_LANES_TUTORIAL_TRIGGER_POSITION = 50.0f;
    const float END_TUTORIAL_TRIGGER_POSITION = 75.0f;

    [SerializeField]
    private GameObject _player;
    private PlayerController _playerController;

    [SerializeField]
    private GameObject _tutorialCommand;
    private Text _tutorialCommandText;

    bool jumpFlag = true;
    bool changeLanesFlag = true;

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
        if (jumpFlag && _player.transform.position.z >= JUMP_TUTORIAL_TRIGGER_POSITION)
        {
            jumpFlag = false;
            StartTextDisplay("JUMP!");
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.upArrowKey));
        } 
        else if (changeLanesFlag && _player.transform.position.z >= CHANGE_LANES_TUTORIAL_TRIGGER_POSITION)
        {
            changeLanesFlag = false;
            StartTextDisplay("MOVE RIGHT!");
            StartCoroutine(_playerController.StartTutorial(Keyboard.current.rightArrowKey));
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
