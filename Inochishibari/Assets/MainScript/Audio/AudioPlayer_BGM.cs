using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_BGM : SingletonMonoBehaviour<AudioPlayer_BGM>
{
    [SerializeField]
    private float nowVolume;
    [SerializeField]
    private AudioClip nowBGM;
    private AudioSource audioSource;

    private new void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        nowVolume = audioSource.volume;
    }

    public void VolumeChange(float _targetVolume, float _changeTime)
    {
        StartCoroutine(VolumeChangeCoroutine(_targetVolume, _changeTime));
    }

    private IEnumerator VolumeChangeCoroutine(float _targetVolume, float _changeTime)
    {
        float _rate = (_targetVolume - nowVolume) / _changeTime;

        do
        {
            nowVolume += _rate * Time.deltaTime;
            audioSource.volume = nowVolume;
            yield return null;
        }
        while (Mathf.Abs(_targetVolume - nowVolume) >= Mathf.Abs(_rate * Time.deltaTime));

        nowVolume = _targetVolume;
        audioSource.volume = nowVolume;
    }

    private IEnumerator PlayBGMCoroutine(AudioClip _bgm, float _outTime, float _inTime, float targetVolume)
    {
        float _rate;
        if (nowBGM != null)
        {

            _rate =  nowVolume / _outTime;
            do
            {
                nowVolume -= _rate * Time.deltaTime;
                audioSource.volume = nowVolume;
                yield return null;
            }
            while (nowVolume >= _rate * Time.deltaTime);
            audioSource.volume = nowVolume;
            yield return null;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        nowVolume = 0;
        audioSource.volume = nowVolume;
        nowBGM = _bgm;
        audioSource.clip = nowBGM;

        yield return null;
        audioSource.Play();

        _rate = targetVolume / _inTime;
        do
        {
            nowVolume += _rate * Time.deltaTime;
            audioSource.volume = nowVolume;
            yield return null;
        }
        while ((targetVolume - nowVolume) >= _rate * Time.deltaTime);

        nowVolume = targetVolume;
        audioSource.volume = nowVolume;
        yield return null;

    }

    public void PlayBGM(AudioClip _bgm, float _outTime, float _inTime, float _targetVolume)
    {
        if (_bgm != null)
        {
            if (nowBGM != _bgm)
            {
                StartCoroutine(PlayBGMCoroutine(_bgm, _outTime, _inTime, _targetVolume));
            }
            else
            {
                if (nowVolume != _targetVolume)
                {
                    VolumeChange(_targetVolume, _inTime);
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            nowBGM = null;
            VolumeChange(0, _inTime);
        }
    }
}
