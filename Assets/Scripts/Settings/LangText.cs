using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class LangText : MonoBehaviour
{
    [SerializeField] 
    private string textIdentifier;

    public string GetTextIdentifier() {
        return textIdentifier;
    }

    public string GetLanguage() {
        return FindObjectOfType<LangResolver>().GetLanguage();
    }

    public void SetTextIdentifier(string newText)
    {
        textIdentifier = newText;
        FindObjectOfType<LangResolver>().ResolveTexts();
    }

    public void ChangeText(string text)
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = Regex.Unescape(text);
    }
}
