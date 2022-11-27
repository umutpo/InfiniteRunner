using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI countdownText = null;
    public Action<bool> isInPauseCountdown;

    [SerializeField] private AudioSource countdownSound;
    [SerializeField] private LangText text;

    [SerializeField]
    public TMP_FontAsset defaultFont;
    [SerializeField]
    public TMP_FontAsset turkishFont;

    private void Start() {
        string language = text.GetLanguage();
        if (language == "Turkish")
        {
            countdownText.font = turkishFont;
            countdownText.fontStyle = FontStyles.Bold;
        }
        else
        {
            countdownText.font = defaultFont;
            countdownText.fontStyle = FontStyles.Normal;
        }
    }

    public void StartCountdown()
    {
        isInPauseCountdown.Invoke(true);
        StopAllCoroutines();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        int counter = 3;
        AudioListener.pause = false;
        countdownSound.Play();
        while (counter >= 0)
        {
            if (counter == 0)
            {
                text.SetTextIdentifier("Start");
            }
            else
            {
                text.SetTextIdentifier("");
                countdownText.text = counter.ToString();
            }
            yield return new WaitForSecondsRealtime(1);
            counter--;
        }
        isInPauseCountdown.Invoke(false);
        countdownText.text = "";
        Time.timeScale = 1f;
    }
}
