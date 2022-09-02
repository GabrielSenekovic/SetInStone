using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeybindButton : Button
{
    public Text currentBinding;
    public string action;
    public bool inConflict = false;

    protected override void Start()
    {
        base.Start();
        currentBinding = GetComponentInChildren<Text>();
    }

    public void SetButtonText(string newText)
    {
        currentBinding.text = GetKeyValue(newText);
    }
    public bool IsSamePath(string text)
    {
        return GetKeyValue(text) == currentBinding.text;
    }
    string GetKeyValue(string text)
    {
        bool bindingBegun = false;
        string binding = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '/')
            {
                bindingBegun = true;
                continue;
            }
            if (bindingBegun)
            {
                binding += text[i];
            }
        }
        return binding;
    }
    public void SetConflict(bool value)
    {
        if(value)
        {
            currentBinding.color = Color.red;
            inConflict = true;
        }
        else
        {
            currentBinding.color = Color.white;
            inConflict = false;
        }
    }
}
