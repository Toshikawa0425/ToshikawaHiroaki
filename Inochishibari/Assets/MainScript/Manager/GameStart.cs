using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private void Start()
    {
        if (GameObject.Find("TableSceneObj"))
        {
            return;
        }

        Debug.Log("loadTableScene");
        SceneEventManager.Instance.AddScene("TableScene",false);
    }
}
