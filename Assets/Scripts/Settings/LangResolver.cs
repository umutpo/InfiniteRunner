using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LangResolver : MonoBehaviour
{
    static LangResolver instance = null;
    private const char Separator = '=';
    private readonly Dictionary<string, LangData> _langData = new Dictionary<string, LangData>();
    private readonly List<string> _supportedLanguages = new List<string>();
    private string _language;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ReadProperties();
        }
    }
    private void ReadProperties()
   {
      foreach (var file in Resources.LoadAll<TextAsset>("LangFiles"))
      {
         string language = file.name;
         var lang = new Dictionary<string, string>();
         foreach (var line in file.text.Split('\n'))
         {
            var prop = line.Split(Separator);
            lang[prop[0]] = prop[1];
         }
         _langData[language] = new LangData(lang);
         _supportedLanguages.Add(language);
      }
      ResolveLanguage();
   }
   private void ResolveLanguage()
   {
      _language = PrefsHolder.GetLang();
      if (!_supportedLanguages.Contains(_language))
      {
         _language = "English";
      }
   }
    public void ResolveTexts()
    {
        var lang = _langData[_language].Lang;
        foreach (var langText in Resources.FindObjectsOfTypeAll<LangText>())
        {
            langText.ChangeText(lang[langText.GetTextIdentifier()]);
        }
    }
    public void ChangeLanguage(string targetLanguage)
    {
        _language = targetLanguage;
        ResolveTexts();
        PrefsHolder.SaveLang(_language);
    }
    private class LangData
    {
        public readonly Dictionary<string, string> Lang;
    
        public LangData(Dictionary<string, string> lang)
        {
            Lang = lang;
        }
    }
}