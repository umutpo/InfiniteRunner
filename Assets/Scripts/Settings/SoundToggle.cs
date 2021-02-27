using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Sprite unmutedSoundSprite;
    [SerializeField] private Sprite mutedSoundSprite;
    void Start()
    {
        Button thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(ToggleSound);
    }


    void ToggleSound()
    {
        if (GetComponent<Image>().sprite == unmutedSoundSprite)
            GetComponent<Image>().sprite = mutedSoundSprite;
        else if (GetComponent<Image>().sprite == mutedSoundSprite)
            GetComponent<Image>().sprite = unmutedSoundSprite;
        else Debug.LogWarning("Volume icon is in an undefined sprite state. You might have forgotten to change or deleted the serialized variable sprite references.");
    }
}
