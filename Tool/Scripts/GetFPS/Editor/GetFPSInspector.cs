using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GetFPS))]
public class GetFPSInspector : Editor
{
    private SerializedObject obj;
    private GetFPS getFPS;
    private SerializedProperty fpsRate;
    private SerializedProperty rateNum;
    private SerializedProperty show;
    private SerializedProperty ints;

    private void OnEnable()
    {
        getFPS = (GetFPS)target;
        obj = new SerializedObject(target);
        fpsRate = obj.FindProperty("fpsRate");
        rateNum = obj.FindProperty("rateNum");
        show = obj.FindProperty("show");
    }


    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(show);
        getFPS.fpsRate = (Rate)EditorGUILayout.EnumPopup("Type", getFPS.fpsRate);
        if (getFPS.fpsRate == Rate.Customize)
        {
            EditorGUILayout.PropertyField(rateNum);
        }
        obj.ApplyModifiedProperties();
    }

}
