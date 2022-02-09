using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveEvent : EventBase
{
    [SerializeField]
    private bool isSave = false;
    [SerializeField]
    private GameObject[] activeObjs;
    [SerializeField]
    private GameObject[] notActiveObjs;

    public override void StartEvent()
    {
        if(activeObjs.Length != 0)
        {
            foreach(GameObject actObj in activeObjs)
            {
                actObj.SetActive(true);
                
            /*
                if (isSave)
                {
                    SceneObjDate.Instance.AddToList(actObj);
                }
            */
            }
        }

        if(notActiveObjs.Length != 0)
        {
            foreach(GameObject notActObj in notActiveObjs)
            {
                notActObj.SetActive(false);
                /*
                if (isSave)
                {
                    SceneObjDate.Instance.AddToList(notActObj);
                }
                */
            }
        }
    }
}
