using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEvent : MonoBehaviour
{
    [SerializeField]
    private Sprite imageSprite;

    public void PlayImage()
    {
        DisplayManager.Instance.DisplayImage(imageSprite);
    }

    public void DeleteImage()
    {
        DisplayManager.Instance.DisplayImage(null);
    }
}
