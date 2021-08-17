using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverAudio : MonoBehaviour
{
    [SerializeField] private List<AudioSource> gameEnd;

    void Start()
    {
        PlayGameEnd();
    }

    public void PlayGameEnd()
    {
        if (gameEnd != null)
        {
            int sfx = Random.Range(0, gameEnd.Count);
            gameEnd[sfx].Play();
        }
    }

}
