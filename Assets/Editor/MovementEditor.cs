/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(Movement))]

public class MovementEditor: SelectableEditor
{
    protected static bool showGeneral = false;
    protected static bool showTools = false; 
    protected static bool showJump = false; 
    protected static bool showSummot = false; 
    protected static bool showSwim = false;
    protected static bool showDebug = false;
    protected static bool showParticles = false;
    protected override void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {
        showGeneral = EditorGUILayout.Foldout(showGeneral, "General");
        if(showGeneral)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bodyTransform"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentVelocity"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playerAnimator"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bubbleAnimator"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("movingDirection"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("mainCollider"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onFire"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("health"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("room"));
            EditorGUI.indentLevel--;
        }
        showTools = EditorGUILayout.Foldout(showTools, "Tools");
        if(showTools)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isFlung"));
            EditorGUI.indentLevel--;
        }
        showJump = EditorGUILayout.Foldout(showJump, "Jumping");
        if(showJump)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("groundCheck"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("whatIsGround"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("amntOfJumpsMax"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpLimit"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpBufferMax"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("jumpForce"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("airJumpForce"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("gravityModifier"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cancelJumpSpeed"));
            EditorGUI.indentLevel--;
        }
        showSummot = EditorGUILayout.Foldout(showSummot, "Summoting");
        if (showSummot)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeCheck"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("wallCheck"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("wallCheckDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("surfaceCheckDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgePosBot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgePos1"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgePos2"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbXOffset1"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbYOffset1"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbXOffset2"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbYOffset2"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("forceLedgeClimb"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hangingFromLedge"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeDetected"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ledgeClimbTimer"));
            EditorGUI.indentLevel--;
        }
        showSwim = EditorGUILayout.Foldout(showSwim, "Swimming");
        if(showSwim)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("whatIsWater"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("submerged"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("touchingWater"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("touchingSurface"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("surfaceCheck"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("surfaceCheckDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalDirection"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("swimmingSpeed"));
            EditorGUI.indentLevel--;
        }
        showParticles = EditorGUILayout.Foldout(showParticles, "Particles");
        if (showParticles)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("doubleJump"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("caneVFX"));
            EditorGUI.indentLevel--;
        }
        showDebug = EditorGUILayout.Foldout(showDebug, "Debug");
        if(showDebug)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("movementDebug"));
            EditorGUI.indentLevel--;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
*/