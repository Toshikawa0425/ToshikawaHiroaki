using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstEvent : MonoBehaviour
{
    public UnityEvent[] firstEvents;
    
    public void TableCamOn()
    {
        TableSceneManager.Instance.TableCamActive();
    }
    public void TableCamOff()
    {
        TableSceneManager.Instance.TableCamNoActive();
    }
    public void PlayEvent(int evNum)
    {
        firstEvents[evNum].Invoke();
    }

    public void SetPlayer(SetStartParam startParam)
    {
        startParam.StartEvent();
    }

    public void SetNowCamera(GameObject _cam)
    {
        CameraController.Instance.SetNowCam(_cam);
    }

    public void OpenGamen(float _time)
    {
        DisplayManager.Instance.GamenOpen(_time);
    }

    public void CloseGamen(float _time)
    {
        DisplayManager.Instance.GamenClose(_time);
    }

    public void BattleGamen()
    {
        DisplayManager.Instance.BattleGamenClose();
    }

    public void BattleCharaSet()
    {
        GameManager.Instance.SetBattleCharas();
    }
}
