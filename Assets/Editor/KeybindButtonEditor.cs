using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(KeybindButton))]
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
}
