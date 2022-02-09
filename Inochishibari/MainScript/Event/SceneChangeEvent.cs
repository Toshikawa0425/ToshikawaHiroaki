using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeEvent : EventBase
{
    [SerializeField]
    private string loadSceneName = "";
    [SerializeField]
    private LoadSceneMode sceneMode;
    [SerializeField]
    private int loadEventNum = 0;

    [SerializeField]
    private bool changeScene = false;

    public override void StartEvent()
    {
        if (!changeScene)
        {
            SceneEventManager.Instance.LoadScene(loadSceneName, sceneMode, loadEventNum);
        }
        else
        {
            SceneEventManager.Instance.ChangeScene(loadSceneName, loadEventNum);
        }
    }


    
}
