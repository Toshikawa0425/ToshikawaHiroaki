using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class KeyBoardManager : SingletonMonoBehaviour<KeyBoardManager>
{
    [SerializeField]
    private UnityEvent afterEvent;
    [SerializeField]
    private List<TextMeshProUGUI> textLists = new List<TextMeshProUGUI>();

    [SerializeField]
    private TextMeshProUGUI[] nameTexts;
    [SerializeField]
    private Image[] underBarImg;

    [SerializeField]
    private List<Transform> lineList = new List<Transform>(); 

    [SerializeField]
    private int horizonNum;
    [SerializeField]
    private int verticalNum;

    [SerializeField]
    private int nowTxtNum = 0;
    [SerializeField]
    private int nowLine = 0;

    [SerializeField]
    private string playerName = "";

    [SerializeField]
    private int nameNum = 3;

    private bool canSelect = true;
    private bool canMove = true;

    private TextMeshProUGUI nowTMPro;

    [SerializeField]
    private Color noSelectedColor;
    [SerializeField]
    private Color selectedColor;

    private void Start()
    {
        //SetTexts();
        nameNum = 3;
        SetUnderBarColor(0, 3);
        nowLine = 1;
        nowTxtNum = 0;
        nowTMPro = null;
        SelectText();
    }

    private void Update()
    {
        if (canMove)
        {
            ChangeText();
            
        }

        if (canSelect)
        {
            InputText();
            DeleteText();
            DesideName();
        }
    }

    public void SetTexts()
    {
        textLists = new List<TextMeshProUGUI>();

        foreach(Transform line in lineList)
        {
            foreach(Transform txt in line)
            {
                textLists.Add(txt.GetComponent<TextMeshProUGUI>());
            }
        }
    }


    private void ChangeText()
    {
        HorizontalSelect(InputSetting.Instance.Horizontal);
        VerticalSelect(InputSetting.Instance.Vertical);
    }

    private void HorizontalSelect(float _value)
    {
        if (_value != 0)
        {
            StartCoroutine(MoveInterval());

            if (_value > 0)
            {
                if(nowTxtNum == ((verticalNum * nowLine) - 1))
                {
                    return;
                }
                else
                {
                    for(int i = nowTxtNum + 1; i <= nowLine * verticalNum;i++)
                    {
                        if(textLists[i].text != "")
                        {
                            nowTxtNum = i;
                            SelectText();
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            else
            {
                if(nowTxtNum == ((nowLine - 1) * verticalNum))
                {
                    return;
                }
                else
                {
                    for (int i = nowTxtNum - 1; i >= (nowLine - 1) * verticalNum; i--)
                    {
                        if (textLists[i].text != "")
                        {
                            nowTxtNum = i;
                            SelectText();
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }


            SelectText();
        }
        else
        {
            return;
        }
    }

    private void VerticalSelect(float _value)
    {
        if(_value != 0)
        {
            StartCoroutine(MoveInterval());

            if(_value > 0)
            {
                if(nowLine != 1)
                {
                    int _txt = nowTxtNum;
                    for(int i = nowLine - 1; i >= 1; i--)
                    {
                        _txt -= verticalNum;

                        if(textLists[_txt].text == "")
                        {
                            continue;
                        }
                        else
                        {
                            nowLine = i;
                            nowTxtNum = _txt;
                            SelectText();
                            break;
                        }
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if(nowLine != horizonNum)
                {
                    int _txt = nowTxtNum;
                    for (int i = nowLine + 1; i <= horizonNum; i++)
                    {
                        _txt += verticalNum;

                        if (textLists[_txt].text == "")
                        {
                            continue;
                        }
                        else
                        {
                            nowLine = i;
                            nowTxtNum = _txt;
                            SelectText();
                            break;
                        }
                    }
                    return;
                }
                else
                {
                    return;
                }
            }

            
        }
        else
        {
            return;
        }
        
    }

    private void SetUnderBarColor(int pre, int next)
    {
        if (pre != 6)
        underBarImg[pre].color = noSelectedColor;
        if(next != 6)
        underBarImg[next].color = selectedColor;
    }

    private void SelectText()
    {
        if(nowTMPro != null)
        {
            nowTMPro.color = noSelectedColor;
        }
        nowTMPro = textLists[nowTxtNum];
        nowTMPro.color = selectedColor;
    }

    private void InputText()
    {
        if (InputSetting.Instance.Accept)
        {
            StartCoroutine(SelectInterval());
            if (nameNum != 6)
            {
                nameTexts[nameNum].text = nowTMPro.text;
                nameNum++;
                SetUnderBarColor(nameNum - 1, nameNum);
            }
            else
            {
                return;
            }
        }
    }

    private void DeleteText()
    {
        if (InputSetting.Instance.Cancel)
        {
            StartCoroutine(SelectInterval());
            if (nameNum != 0)
            {
                nameNum--;
                nameTexts[nameNum].text = "";
                SetUnderBarColor(nameNum + 1, nameNum);
            }
            else
            {
                return;
            }
        }
    }

    private void DesideName()
    {
        if (InputSetting.Instance.Attack)
        {
            StartCoroutine(SelectInterval());
            if (nameNum > 0)
            {
                foreach (TextMeshProUGUI txt in nameTexts)
                {
                    playerName += txt.text;
                }

                Debug.Log(playerName);
                afterEvent.Invoke();
                gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }

    private IEnumerator MoveInterval()
    {
        canMove = false;
        int _flame = 0;
        while (_flame < 8)
        {
            _flame++;
            yield return null;
        }
        canMove = true;
    }

    private IEnumerator SelectInterval()
    {
        canSelect = false;
        int _flame = 0;
        while (_flame < 8)
        {
            _flame++;
            yield return null;
        }
        canSelect = true;
    }
}
