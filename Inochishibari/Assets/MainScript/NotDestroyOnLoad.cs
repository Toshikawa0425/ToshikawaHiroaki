using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDestroyOnLoad : SingletonMonoBehaviour<NotDestroyOnLoad>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
