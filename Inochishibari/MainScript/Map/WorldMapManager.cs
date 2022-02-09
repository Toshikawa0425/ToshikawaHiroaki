using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapManager : SingletonMonoBehaviour<WorldMapManager>
{
    [SerializeField]
    private GameObject sceneObj;
    [SerializeField]
    private Transform playerPiece;

    [SerializeField]
    private MapPoint nowPoint;
    [SerializeField]
    private MapPoint currentPoint;
    [SerializeField]
    private MapPoint selectingPoint;

    [SerializeField]
    private List<MapPoint> nextPoints = new List<MapPoint>();

    [SerializeField]
    private bool canMove = true;

    [SerializeField]
    private NextSceneInfo nextSceneInfo;

    [System.Serializable]
    public class NextSceneInfo
    {
        public string sceneName;
        public int evNum;
    }


    public enum Direction
    {
        Up,
        Down,
        Right,
        Left,
        Center
    }

    public void StartSetting()
    {
        DisplayManager.Instance.GamenOpen(1.0f);
        TableSceneManager.Instance.TableCamNoActive();
    }



    private void MovePieceToPosition(bool _playEv)
    {
        Vector3 _pos = nowPoint.pos.position;
        playerPiece.position = new Vector3(_pos.x, playerPiece.position.y, _pos.z);


        if (_playEv)
        {
            nowPoint.PlayEvent();
        }
        else
        {
            ActivateAroundPoints();
        }
    }

    public void SetCurrentPoint()
    {
        SetPoint(currentPoint);
    }

    public void SetPoint(MapPoint _point, bool _playEv = false)
    {
        currentPoint = nowPoint;
        nowPoint = _point;

        if (nextPoints.Count > 0)
        {
            InactivateAroundPoints();
        }

        nextPoints = nowPoint.aroundPoints;
        
        MovePieceToPosition(_playEv);
    }

    public void SetFirstPoint(MapPoint _point)
    {
        nowPoint = _point;
    }

    public void SelectFirstPoint()
    {
        SetPoint(nowPoint);
    }

    public void ActivateAroundPoints()
    {
        if (nextPoints.Count > 0)
        {
            foreach (MapPoint _point in nextPoints)
            {
                _point.ActiveThisPoint();
            }

            nextPoints[0].Select();
        }
    }

    public void InactivateAroundPoints()
    {
        ButtonManager.Instance.ResetLastButton();
        foreach (MapPoint _point in nextPoints)
        {
            _point.InActiveThisPoint();
        }
    }

    public void SetNextSceneInfo(NextSceneInfo _info)
    {
        nextSceneInfo = _info;

    }

    public void GoDangeonScene()
    {
        DisplayManager.Instance.GamenClose(1.0f);
        SceneEventManager.Instance.ChangeScene(nextSceneInfo.sceneName, nextSceneInfo.evNum);
    }

    public void GoTownScene()
    {
        DisplayManager.Instance.GamenClose(1.0f);
        SceneEventManager.Instance.LoadScene(nextSceneInfo.sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single, nextSceneInfo.evNum);
    }
}
