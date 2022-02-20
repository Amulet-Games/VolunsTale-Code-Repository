using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateEnterBool : StateMachineBehaviour
{
    [Header("Target Bool Parameter")]
    [Tooltip("The boolean anim parameter that you want to change.")]
    public string boolName;

    [Header("Target Status")]
    [Tooltip("The target status that this boolean anim parameter will change to once it enter to this state.")]
    public bool status;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
    }
}
