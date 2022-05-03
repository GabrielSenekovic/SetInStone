using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;
//using UnityEditor.UI;

/*[CustomEditor(typeof(KeybindButton))]
public class KeybindButtonEditor : UnityEditor.UI.ButtonEditor
{
    SerializedProperty text;
    SerializedProperty action;

    protected override void OnEnable()
    {
        base.OnEnable();
        text = serializedObject.FindProperty("currentBinding");
        action = serializedObject.FindProperty("action");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        KeybindButton targetMenuButton = (KeybindButton)target;

        EditorUtility.SetDirty(target); //Ensures the change is preserved

        EditorGUILayout.PropertyField(text, new GUIContent("Current Binding"));
        EditorGUILayout.PropertyField(action, new GUIContent("Action Text"));

        serializedObject.ApplyModifiedProperties();

        // Show default inspector property editor
    }
}*/
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
