using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(Area))]
public class AreaEditor : SelectableEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Area area = (Area)target;
        EditorUtility.SetDirty(area);
        
        if (GUILayout.Button("Collect Rooms"))
        {
            area.CollectAllRooms();
        }
        if (GUILayout.Button("Set Room Links"))
        {
           // area.SetAllLinks();
        }
    }
}
