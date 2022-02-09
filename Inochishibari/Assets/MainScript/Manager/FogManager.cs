using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : SingletonMonoBehaviour<FogManager>
{
    public void ChangeFogColor(Color _color, float _changeTime)
    {
        StartCoroutine(ChangeFogColorCoroutine(_color, _changeTime));
    }
    
    private IEnumerator ChangeFogColorCoroutine(Color _color,float _changeTime)
    {
        float _flame = _changeTime * Application.targetFrameRate;

        for (int i = 0; i < _flame; i++)
        {
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, _color, Time.deltaTime / _changeTime);
            yield return null;
        }

        RenderSettings.fogColor = _color;
    }
}
