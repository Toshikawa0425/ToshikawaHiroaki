using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChildChange : MonoBehaviour
{
    public GameObject changeObj;
    public Transform[] parentObjs;

    public void PlayChange()
    {
        foreach (Transform _parent in parentObjs)
        {
            for (int i = _parent.childCount - 1; i >= 0; i--)
            {
                GameObject obj = Instantiate(changeObj, _parent.GetChild(i));
                obj.transform.parent = _parent;
                DestroyImmediate(_parent.GetChild(i).gameObject);
            }
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(ChildChange))]

    public class ChildChangeEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ChildChange script = target as ChildChange;

            if (GUILayout.Button("チャイルドチェンジ"))
            {
                script.PlayChange();
            }
        }
    }
#endif

}
