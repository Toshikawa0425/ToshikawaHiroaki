using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardChecker : MonoBehaviour
{
    [SerializeField]
    private LayerMask checkLayer;
    public Vector3 ForwardCheck(Vector3 _velocity)
    {
        Ray ray = new Ray((transform.position + Vector3.up) + (_velocity.normalized), Vector3.down);

        //Debug.DrawRay((transform.position + Vector3.up) + (_velocity.normalized), Vector3.down * 1.5f, Color.red,5);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.5f,checkLayer))
        {
            //Debug.Log(hit.collider.gameObject.name);
            return _velocity;
        }

        return Vector3.zero;
    }
}
