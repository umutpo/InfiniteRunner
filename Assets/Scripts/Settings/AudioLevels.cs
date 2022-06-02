using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioLevels : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake() {
        if (musicSlider != null && sfxSlider != null)
        {
            ReadVolumes();
        }
    }

    public void SetMusicLevel(float musicLvl) { 
        PrefsHolder.SaveVolMusic(musicLvl);
        masterMixer.SetFloat("volMusic", Mathf.Log10(PrefsHolder.GetVolMusic()) * 20f);
    }

    public void SetSfxLevel(float sfxLvl) {   
        PrefsHolder.SaveVolSfx(sfxLvl);
        masterMixer.SetFloat("volSfx", Mathf.Log10(PrefsHolder.GetVolSfx()) * 20f);
    }
 
    private void ReadVolumes() {
        musicSlider.value = PrefsHolder.GetVolMusic();
        sfxSlider.value = PrefsHolder.GetVolSfx(); 
    }

}
