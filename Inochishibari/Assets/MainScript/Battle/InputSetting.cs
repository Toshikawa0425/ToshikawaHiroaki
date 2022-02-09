using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSetting : SingletonMonoBehaviour<InputSetting>
{
    public bool canButton = true;
    [SerializeField]
    private string accept = "Accept";
    [SerializeField]
    private string cancel = "Cancel";
    [SerializeField]
    private string check = "Check";
    [SerializeField]
    private string horizontal = "Horizontal";
    [SerializeField]
    private string vertical = "Vertical";
    [SerializeField]
    private string attack = "Attack";
    [SerializeField]
    private string charaChange_1 = "CharaChange_1";
    [SerializeField]
    private string charaChange_2 = "CharaChange_2";
    [SerializeField]
    private string menu = "Menu";

    private void Start()
    {
        canButton = true;
    }

    public float Horizontal
    {
        get {
            return Input.GetAxisRaw(horizontal); 
        }
    }

    public float Vertical
    {
        get { return Input.GetAxisRaw(vertical); }
    }

    public bool Dash
    {
        get { return Input.GetButton(cancel); }
    }

    public float Horizontal_Once
    {
        get
        {
            float value = Input.GetAxisRaw(horizontal);
            if (value != 0)
            {
                if (canButton)
                {
                    StartCoroutine(IntervalCanButton());
                    Debug.Log(value);
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public float Vertical_Once
    {
        get
        {
            float value = Input.GetAxisRaw(vertical);
            if (value != 0)
            {
                if (canButton)
                {
                    StartCoroutine(IntervalCanButton());
                    Debug.Log(value);
                    return value;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public bool Attack
    {
        get { return Input.GetButtonDown(attack); }
    }

    public bool CharaChange_1
    {
        get { return Input.GetButtonDown(charaChange_1); }
    }

    public bool CharaChange_2
    {
        get { return Input.GetButtonDown(charaChange_2); }
    }

    public bool Accept 
    {
        get
        {
            if (Input.GetButtonDown(accept))
            {
                if (canButton)
                {
                    StartCoroutine(IntervalCanButton());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public bool Cancel
    {
        get
        {
            return Input.GetButtonDown(cancel);
        }
    }

    public bool Check
    {
        get
        {
            if (Input.GetButtonDown(check))
            {
                if (canButton)
                {
                    StartCoroutine(IntervalCanButton());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }



    public bool Menu
    {
        get
        {
            if (Input.GetButtonDown(menu))
            {
                if (canButton)
                {
                    StartCoroutine(IntervalCanButton());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    private IEnumerator IntervalCanButton()
    {
        canButton = false;
        int _flame = 0;
        while (_flame < 10)
        {
            _flame++;
            yield return null;
        }
        canButton = true;
    }
}
