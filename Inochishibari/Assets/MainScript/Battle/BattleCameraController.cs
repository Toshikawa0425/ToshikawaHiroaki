using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraController : SingletonMonoBehaviour<BattleCameraController>
{
    public void SetDepthOfField(Transform _target)
    {
        float distance = Vector3.Distance(Camera.main.transform.position, _target.position);
        MainCameraSetting.Instance.SetDepthOfField(distance);
    }
}
