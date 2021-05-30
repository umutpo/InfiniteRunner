using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAnimator : MonoBehaviour
{
    public Animator screenTransition;
    public void sceneTransition()
    {
        StartCoroutine(loadAnimation());
    }

    IEnumerator loadAnimation()
    {
        screenTransition.SetTrigger("start");
        yield return new WaitForSeconds(1f);
    }
}
