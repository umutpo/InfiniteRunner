﻿using System.Collections;
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
    public void ChangeText(string text)
    {
        GetComponent<Text>().text = Regex.Unescape(text);
    }
}