using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer_SE : SingletonMonoBehaviour<AudioPlayer_SE>
{
    AudioSource audioSource;
    bool isPlaying = false;

    private new void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySE(AudioClip _se, float _volume)
    {
        if (isPlaying)
        {
            return;
        }
            audioSource.volume = _volume;
            audioSource.PlayOneShot(_se);
            isPlaying = true;
        StartCoroutine(CanPlayCoroutine());
    }

    IEnumerator CanPlayCoroutine()
    {
        int i = 0;
        while(i < 5)
        {
            i++;
            yield return null;
        }

        isPlaying = false;
    }
}
