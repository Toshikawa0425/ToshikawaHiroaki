using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyButton : MonoBehaviour
{
    public UnityEvent Event_Accept;
    public UnityEvent Event_Cancel;
    public UnityEvent Event_Check;
    public UnityEvent Event_Y;

    [SerializeField]
    private UnityEvent SelectedEvent;
    [SerializeField]
    private UnityEvent DeSelectedEvent;

    public enum ButtonType
    {
        Accept,
        Cancel,
        Check,
        Y
    }




    public void Selected()
    {
        if (SelectedEvent.GetPersistentEventCount() != 0)
        {
            SelectedEvent.Invoke();
        }
    }

    public void Deselected()
    {
        if(DeSelectedEvent.GetPersistentEventCount() != 0)
        {
            DeSelectedEvent.Invoke();
        }
    }

    public void PlayButton(ButtonType buttonType)
    {
        switch (buttonType)
        {
            case ButtonType.Accept:
                Event_Accept.Invoke();
                break;
            case ButtonType.Cancel:
                Event_Cancel.Invoke();
                break;
            case ButtonType.Check:
                Event_Check.Invoke();
                break;
            default:
                break;
        }
    }
}
