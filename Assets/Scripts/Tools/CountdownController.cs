using System.Collections;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI countdownText = null;

    public bool isInCountdown = true;
    private float timeRemaining = 4;
    private bool startingCountdown = true;

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
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }
}
