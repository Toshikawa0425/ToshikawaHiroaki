using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Create2DObject : MonoBehaviour
{
    [MenuItem("GameObject/3D Object/Prop")]
    public static void Create2D()
    {
        GameObject obj = (GameObject)Resources.Load("2DObject");
        Instantiate(obj);
    }
}
