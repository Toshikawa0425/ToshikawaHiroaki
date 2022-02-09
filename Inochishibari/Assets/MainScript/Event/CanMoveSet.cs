using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveSet : MonoBehaviour
{
    public void CanMoveOn()
    {
        PlController_Field.Instance.CanMoveOn();
    }

    public void CanMoveOff()
    {
        PlController_Field.Instance.CanMoveOff();
    }
}
