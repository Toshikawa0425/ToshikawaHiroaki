using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MapPoint : MonoBehaviour
{
    public List<MapPoint> aroundPoints = new List<MapPoint>();
    [SerializeField]
    private GameObject pointButton;
    public Transform pos;

    [SerializeField]
    private GameObject selectingIcon;

    public PointType pointType;

    

    [SerializeField]
    private Talk beforeEventTalk;
    [SerializeField]
    private WorldMapManager.NextSceneInfo nextSceneInfo;
    

    public enum PointType
    {
        None,
        Town,
        Dangeon
    }

    public void ActiveThisPoint()
    {
        pointButton.SetActive(true);
    }

    public void InActiveThisPoint()
    {
        pointButton.SetActive(false);
    }

    public void Select()
    {
        ButtonManager.Instance.SetLastButton(pointButton);
    }

    public void SelectedThisPoint()
    {
        WorldMapManager.Instance.SetPoint(this, true);
    }
    

    public void IsSelecting()
    {
        selectingIcon.SetActive(true);
    }

    public void DeSelecting()
    {
        selectingIcon.SetActive(false);
    }


    

    public void PlayEvent()
    {
        switch (pointType)
        {
            default:
                WorldMapManager.Instance.ActivateAroundPoints();
                break;

            case PointType.Dangeon:
            case PointType.Town:
                WorldMapManager.Instance.SetNextSceneInfo(nextSceneInfo);
                beforeEventTalk.StartEvent();
                break;
        }
    }
}
