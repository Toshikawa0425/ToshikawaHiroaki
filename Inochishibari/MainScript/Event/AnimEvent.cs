using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private TransitionType transitionType;
    [SerializeField]
    private string paramName;
    [SerializeField]
    private bool boolParam;
    
    public enum TransitionType
    {
        Trigger,
        Bool
    }
    public void ChangeAnim()
    {
        switch (transitionType)
        {
            case TransitionType.Trigger:
                animator.SetTrigger(paramName);
                break;
            case TransitionType.Bool:
                animator.SetBool(paramName, boolParam);
                break;
        }
    }
}
