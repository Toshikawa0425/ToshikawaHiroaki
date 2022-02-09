using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleLog : SingletonMonoBehaviour<BattleLog>
{
    [SerializeField]
    private List<TextMeshProUGUI> logs = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<string> logStr = new List<string>();

    int nowNum = 0;

    public void WriteLog(string _str)
    {
        
        for (int i = logStr.Count - 1 ; i >= 0; i--)
        {
            if (i != 0)
            {
                logStr[i] = logStr[i - 1];
            }
            else
            {
                logStr[0] = _str;
            }

            logs[i].text = logStr[i];
        }
    }

    public void ResetLog()
    {
        logStr = new List<string>();
        for(int i = 0; i< logs.Count; i++)
        {
            logs[i].text = "";
            logStr.Add("");
        }
    }
}
