using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartParam : EventBase
{
    [SerializeField]
    private Transform plPos;
    [SerializeField]
    private CharaMove.Direction startDirection;
    [SerializeField]
    private CameraController.CameraType startCameraType;
    [SerializeField]
    private GameObject targetCamera;
    [SerializeField]
    private bool canMove = true;
    [SerializeField]
    private Vector3 scale = Vector3.one;

    public override void StartEvent()
    {
        if (PlayerObj.Instance)
        {
            PlayerObj.Instance.PlayerOn();
        }
        StartCoroutine(SetParamCoroutine());
    }

    private IEnumerator SetParamCoroutine()
    {
        GameManager.Instance.SetPlayerObject(plPos,startDirection,scale,canMove);
        
        yield return null;
        //PlController_Field.Instance.charaMove.SetAnimDirection(startDirection);
        CameraController.Instance.SetCinemaBrain();
        CameraController.Instance.ChangeCamera(startCameraType, 0, targetCamera);
    }
}
