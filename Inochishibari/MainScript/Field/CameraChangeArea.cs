using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeArea : InOutEvent
{
    [SerializeField]
    private CamInfo positiveInfo;
    [SerializeField]
    private CamInfo negativeInfo;


    [System.Serializable]
    private class CamInfo
    {
        public CameraController.CameraType cameraType;
        public GameObject targetCam;
        public float changeTime;
        public float rotAngle;
    }

    public override void PlayEvent_Positive()
    {
        CameraController.Instance.ChangeCamera(positiveInfo.cameraType, positiveInfo.changeTime, positiveInfo.targetCam,positiveInfo.rotAngle);
        base.PlayEvent_Positive();
    }

    public override void PlayEvent_Negative()
    {
        CameraController.Instance.ChangeCamera(negativeInfo.cameraType, negativeInfo.changeTime, negativeInfo.targetCam,negativeInfo.rotAngle);
        base.PlayEvent_Negative();
    }


}
