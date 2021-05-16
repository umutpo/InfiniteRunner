using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{
    public Text countdownText = null;

    public bool isInCountdown = true;
    private float timeRemaining;

    private void Start() {
        isInCountdown = true;
        timeRemaining = 4;
    }


    // Update is called once per frame
    void Update()
    {
        if (isInCountdown)
        {                
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining > 3)  countdownText.text = "3";
                else if (timeRemaining > 2) countdownText.text = "2";
                else if (timeRemaining > 1) countdownText.text = "1";
                else countdownText.text = "Start!";
            }
            else
            {
                countdownText.text = "";
                isInCountdown = false;
            }
        }
        
    }
}
