using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    [SerializeField] private AudioSource gameStart;
    [SerializeField] private List<AudioSource> obstacleHit;
    [SerializeField] private List<AudioSource> recipeCompletion;
    [SerializeField] private List<AudioSource> criticApproaching;
    [SerializeField] private AudioSource jump;
    [SerializeField] private AudioSource slide;
    [SerializeField] private AudioSource laneSwitch;
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
        {
            int sfx = Random.Range(0, obstacleHit.Count);
            obstacleHit[sfx].Play();
        }
    }

    public void PlayRecipeCompletion()
    {
        if (recipeCompletion != null)
        {
            int sfx = Random.Range(0, recipeCompletion.Count);
            recipeCompletion[sfx].Play();
        }
    }

    public void PlayCriticApproaching()
    {
        if (criticApproaching != null)
        {
            int sfx = Random.Range(0, criticApproaching.Count);
            criticApproaching[sfx].Play();
        }
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
