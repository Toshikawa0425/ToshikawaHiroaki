using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MainCameraSetting : SingletonMonoBehaviour<MainCameraSetting>
{
    [SerializeField]
    private PostProcessVolume postProcessVolume;
    [SerializeField]
    private DepthOfField depthOfField;

    private new void Awake()
    {
        SetPostPro(postProcessVolume);
    }

    public void SetPostPro(PostProcessVolume _volume = null)
    {
        if (_volume == null)
        {
            if (postProcessVolume == null)
            {
                postProcessVolume = GameObject.Find("MainPostPro").GetComponent<PostProcessVolume>();
            }
        }
        else
        {
            postProcessVolume = _volume;
        }

        foreach (PostProcessEffectSettings item in postProcessVolume.profile.settings)
        {
            if (item as DepthOfField)
            {
                depthOfField = item as DepthOfField;
            }
        }
    }


    public void SetDepthOfField(float _distance)
    {
        depthOfField.focusDistance.value = _distance;
    }
}
