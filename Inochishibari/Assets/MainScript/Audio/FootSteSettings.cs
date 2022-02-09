using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteSettings : SingletonMonoBehaviour<FootSteSettings>
{
    public FootStep_Terrain[] footSteps_Terrain;
    public FootStep_Obj[] footSteps_Obj;

    [System.Serializable]
    public class FootStep_Obj
    {
        public string tagName = "";
        public AudioClip se;
        public float volume;
    }

    [System.Serializable]
    public class FootStep_Terrain
    {
        public AudioClip se;
        public float volume;
    }


    public FootStep.SeInfo GetSE_Terrain(int _groundNum)
    {
        if (_groundNum >= footSteps_Terrain.Length)
        {
            return null;
        }

        FootStep_Terrain _step = footSteps_Terrain[_groundNum];
        FootStep.SeInfo seInfo = new FootStep.SeInfo();
        seInfo.se = _step.se;
        seInfo.volume = _step.volume;


        return seInfo;
    }

    public FootStep.SeInfo GetSE_Obj(string _tagName)
    {
        foreach (FootStep_Obj se in footSteps_Obj)
        {
            if (se.tagName == _tagName)
            {
                FootStep.SeInfo seInfo = new FootStep.SeInfo();
                seInfo.se = se.se;
                seInfo.volume = se.volume;

                return seInfo;
            }
        }

        return null;
    }
}
