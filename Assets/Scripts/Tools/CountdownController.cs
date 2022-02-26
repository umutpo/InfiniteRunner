using System.Collections;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI countdownText = null;

    public bool isInCountdown = true;

    [SerializeField] private AudioSource countdownSound;
    [SerializeField] private LangText text;

    private void Start() {
        StartCountdown();
    }

    public void StartCountdown()
    {
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
        countdownText.text = "";
        Time.timeScale = 1f;
    }
}
