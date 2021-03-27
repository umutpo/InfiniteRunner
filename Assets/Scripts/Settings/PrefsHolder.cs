using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefsHolder
{
    private const string Lang = "Lang";

    public static void SaveLang(string lang)
    {
        PlayerPrefs.SetString(Lang, lang);
    }

    public static string GetLang()
    {
        string language = PlayerPrefs.GetString(Lang, Application.systemLanguage.ToString());
        return language;
    }
}