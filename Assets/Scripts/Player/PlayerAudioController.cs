using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    [SerializeField] private AudioSource gameStart;
    [SerializeField] private AudioSource obstacleHit;
    [SerializeField] private AudioSource recipeCompletion;
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource slide;
    [SerializeField] private AudioSource laneSwitch;
    [SerializeField] private AudioSource gameEnd;
    [SerializeField] private AudioSource running;
    [SerializeField] private AudioSource itemCollection;

    // Audio played on awake instead
    // public void PlayGameStart()
    // {
    //     if (gameStart != null)
    //     gameStart.Play();
    // }

    public void PlayObstacleHit()
    {
        if (obstacleHit != null)
            obstacleHit.Play();
    }

    public void PlayRecipeCompletion()
    {
        if (recipeCompletion != null)
            recipeCompletion.Play();
    }

    public void PlayJump()
    {
        if (jump != null)
            jump.Play();
    }

    public void PlaySlide()
    {
        if (slide != null)
            slide.Play();
    }

    public void PlayLaneSwitch()
    {
        if (laneSwitch != null)
            laneSwitch.Play();
    }

    public void PlayGameEnd()
    {
        if (gameEnd != null)
            gameEnd.Play();
    }

    public void PlayRunning()
    {
        if (running != null && !running.isPlaying)
            running.Play();
    }

    public void PauseRunning()
    {
        if (running != null)
            running.Pause();
    }

    public void PlayItemCollection()
    {
        if (itemCollection != null)
            itemCollection.Play();
    }
}
