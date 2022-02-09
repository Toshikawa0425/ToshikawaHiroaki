using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayer : MonoBehaviour
{
    public void PlayerCanMoveOff()
    {
        if (PlController_Field.Instance.canMove)
        {
            PlController_Field.Instance.canMove = false;
        }
    }

    public void PlayerCanMoveOn()
    {
        if (!PlController_Field.Instance.canMove)
        {
            PlController_Field.Instance.canMove = false;
        }
    }
}
