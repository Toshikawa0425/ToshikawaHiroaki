using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DisplayManager : SingletonMonoBehaviour<DisplayManager>
{
    [SerializeField]
    private Animator gamenAnim;
    [SerializeField]
    private Animator battleGamenAnim;


    [SerializeField]
    private GameObject imageObject;
    [SerializeField]
    private Image image;

    public bool gamenFading = false;

    [SerializeField]
    private UnityEvent afterEvents;
    

    public void DisplayImage(Sprite _sprite)
    {
        if (_sprite != null)
        {
            image.sprite = _sprite;
            imageObject.SetActive(true);
        }
        else
        {
            imageObject.SetActive(false);
            image.sprite = null;
        }
    }

    public void BattleGamenOpen()
    {
        battleGamenAnim.SetTrigger("Open");
    }

    public void BattleGamenClose()
    {
        battleGamenAnim.SetTrigger("Close");
    }


    public void GamenOpen(float _time)
    {
        gamenFading = true;
        Debug.Log("GamenOpen : " + _time);
        if (_time == 0)
        {
            _time = 0.1f;
        }
        gamenAnim.SetFloat("Speed", 1 / _time);
        gamenAnim.SetTrigger("Open");
    }

    public void GamenClose(float _time)
    {
        gamenFading = true;
        Debug.Log("GamenClose : " + _time);
        if (_time == 0)
        {
            _time = 0.1f;
        }
        gamenAnim.SetFloat("Speed", 1 / _time);
        gamenAnim.SetTrigger("Close");
    }

    public void SetAfterEvent(UnityEvent _event)
    {
        afterEvents = _event;
    }

    public void Faded()
    {
        gamenFading = false;
        UnityEvent _ev = afterEvents;

            if (_ev.GetPersistentEventCount() > 0)
            {
                afterEvents = new UnityEvent();
                _ev.Invoke();
            }
    }
}
