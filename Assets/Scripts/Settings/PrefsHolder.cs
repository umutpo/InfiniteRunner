using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefsHolder
{
    private const string Lang = "Lang";
    private const string VolMusic = "VolMusic";
    private const string VolSfx = "VolSfx";


    public static void SaveLang(string lang)
    {
        PlayerPrefs.SetString(Lang, lang);
    }

    public static string GetLang()
    {
        string language = PlayerPrefs.GetString(Lang, Application.systemLanguage.ToString());
        return language;
    }

    public static void SaveVolMusic(float music)
    {
        PlayerPrefs.SetFloat(VolMusic, music);
    }

    public static void SaveVolSfx(float sfx)
    {
        PlayerPrefs.SetFloat(VolSfx, sfx);
    }

    public static float GetVolMusic()
    {
        return PlayerPrefs.GetFloat(VolMusic, 1f);
    }

    public static float GetVolSfx()
    {
        return PlayerPrefs.GetFloat(VolSfx, 1f);
    }

}