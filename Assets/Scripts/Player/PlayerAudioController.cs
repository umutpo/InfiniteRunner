using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource gameStart;
    [SerializeField] private AudioSource obstacleHit;
    [SerializeField] private AudioSource recipeCompletion;
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource slide;
    [SerializeField] private AudioSource laneSwitch;
    [SerializeField] private AudioSource gameEnd;
    [SerializeField] private AudioSource running;

    public void PlayGameStart()
    {
        gameStart.Play();
    }

    public void PlayObstacleHit()
    {
        obstacleHit.Play();
    }

    public void PlayRecipeCompletion()
    {
        recipeCompletion.Play();
    }

    public void PlayJump()
    {
        jump.Play();
    }

    public void PlaySlide()
    {
        slide.Play();
    }

    public void PlayLaneSwitch()
    {
        laneSwitch.Play();
    }

    public void PlayGameEnd()
    {
        gameEnd.Play();
    }

    public void PlayRunning()
    {
        running.Play();
    }

    public void PauseRunning()
    {
        running.Pause();
    }
}
