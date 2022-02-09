using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KeyBoardManager))]
public class KeyBoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        KeyBoardManager script = (KeyBoardManager)target;
        if (GUILayout.Button("�Z�b�g�L�[�{�[�h"))
        {
            script.SetTexts();
        }
    }
}
