using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    
    private Vector3 cameraPos;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float height;

    [SerializeField]
    private float distance;
    [SerializeField]
    private GameObject nowCamera;
    [SerializeField]
    private GameObject lastCamera;
    [SerializeField]
    private float lastRot;
    [SerializeField]
    private GameObject camera_Near;
    [SerializeField]
    private GameObject camera_Midle;
    [SerializeField]
    private GameObject camera_Far;

    [SerializeField]
    private Cinemachine.CinemachineBrain cinemachineBrain;


    public enum CameraType
    {
        Near,
        Midle,
        Far,
        Other,
        Last
    }



    

    private void LateUpdate()
    {
        cameraPos = playerTransform.position + playerTransform.up * height; //+ Vector3.Scale(-transform.forward * offset, new Vector3(1, 0, 1)) + Vector3.up * heightOffset;  //Vector3.Lerp(transform.position,player.position + player.up * 1.0f,Time.deltaTime * followSpeed);
        
        transform.position = cameraPos;

        distance = Vector3.Distance(Camera.main.transform.position, playerTransform.position);
        MainCameraSetting.Instance.SetDepthOfField(distance);
    }

    public void SetNowCam(GameObject _cam)
    {
        nowCamera = _cam;
    }

    public void SetCinemaBrain()
    {
        cinemachineBrain = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
    }

    public void SetBlendTime(float _time)
    {
        cinemachineBrain.m_DefaultBlend.m_Time = _time;
    }

    public void ChangeCamera(CameraType _type,float changeTime,GameObject _targetCam = null, float _camRotation = 0)
    {   
        cinemachineBrain.m_DefaultBlend.m_Time = changeTime;
        StartCoroutine(CameraChangeCoroutine(_type,  _targetCam,_camRotation));

    }

    private IEnumerator CameraChangeCoroutine(CameraType _type, GameObject _targetCam = null, float _camRotation = 0)
    {
        yield return null;
        switch (_type)
        {
            case CameraType.Near:
                lastCamera = nowCamera;
                lastRot = _camRotation;
                transform.rotation = Quaternion.Euler(0, _camRotation, 0);
                yield return null;
                camera_Near.SetActive(true);
                if (nowCamera != null && nowCamera != camera_Near)
                {
                    nowCamera.SetActive(false);
                }
                nowCamera = camera_Near;
                break;

            case CameraType.Midle:
                lastCamera = nowCamera;
                transform.rotation = Quaternion.Euler(0, _camRotation, 0);
                yield return null;
                camera_Midle.SetActive(true);
                if (nowCamera != null && nowCamera != camera_Midle)
                {
                    nowCamera.SetActive(false);
                }
                nowCamera = camera_Midle;
                break;

            case CameraType.Far:
                lastCamera = nowCamera;
                transform.rotation = Quaternion.Euler(0, _camRotation, 0);
                yield return null;
                camera_Far.SetActive(true);
                if (nowCamera != null && nowCamera != camera_Far)
                {
                    nowCamera.SetActive(false);
                }
                nowCamera = camera_Far;
                break;

            case CameraType.Other:
                lastCamera = nowCamera;
                _targetCam.SetActive(true);
                if (nowCamera != null && nowCamera != _targetCam)
                {
                    nowCamera.SetActive(false);
                }
                nowCamera = _targetCam;
                break;

            case CameraType.Last:
                transform.rotation = Quaternion.Euler(0, lastRot, 0);
                yield return null;

                lastCamera.SetActive(true);

                if (nowCamera != null && nowCamera != lastCamera)
                {
                    nowCamera.SetActive(false);
                }
                nowCamera = lastCamera;
                lastCamera = null;
                lastRot = 0;
                break;
        }
    }
}
