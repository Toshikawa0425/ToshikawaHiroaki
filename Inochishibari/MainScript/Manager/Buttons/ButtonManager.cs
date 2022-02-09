using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonManager : SingletonMonoBehaviour<ButtonManager>
{
    [SerializeField]
    private GameObject lastSelected;

    public bool isActiveThis = true;

    [SerializeField]
    private MyButton nowSelectingButton = null;

    private void Start()
    {
        if(lastSelected != null)
        {
            SetLastButton(lastSelected);
        }
    }


    private void Update()
    {
        if (isActiveThis)
        {
            if (lastSelected != null)
            {
                CheckCurrentButton();
            }

            if(nowSelectingButton != null)
            {
                PressButton();
            }
        }
    }

    public void SetLastButton(GameObject _obj)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (nowSelectingButton != null)
        {
            nowSelectingButton.Deselected();
        }

        lastSelected = _obj;

        if (lastSelected.GetComponent<MyButton>())
        {
            nowSelectingButton = lastSelected.GetComponent<MyButton>();
            nowSelectingButton.Selected();
        }
        else
        {
            nowSelectingButton = null;
        }
    }

    public void ResetLastButton()
    {
        if (nowSelectingButton != null)
        {
            nowSelectingButton.Deselected();
        }
        EventSystem.current.SetSelectedGameObject(null);
        lastSelected = null;
        nowSelectingButton = null;
    }

    private void CheckCurrentButton()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (lastSelected.gameObject.activeSelf && lastSelected.GetComponent<Button>() != null && lastSelected.GetComponent<Button>().interactable)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
        }
        else
        {
            GameObject _obj = EventSystem.current.currentSelectedGameObject;
            if (lastSelected == _obj)
            {
                return;
            }
            else
            {
                SetLastButton(EventSystem.current.currentSelectedGameObject);
            }
        }
    }

    private void PressButton()
    {
        if (InputSetting.Instance.Accept)
        {
            nowSelectingButton.PlayButton(MyButton.ButtonType.Accept);
        }
        else if (InputSetting.Instance.Cancel)
        {
            nowSelectingButton.PlayButton(MyButton.ButtonType.Cancel);
        }
        else if (InputSetting.Instance.Check)
        {
            nowSelectingButton.PlayButton(MyButton.ButtonType.Check);
        }
    }
}
