using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateExitDualBool : StateMachineBehaviour
{
    [Header("Target Bool Parameter")]
    public string boolName;
    public string secondBoolName;

    [Header("Target Status")]
    public bool status;
    public bool secondBoolStatus;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
        animator.SetBool(secondBoolName, secondBoolStatus);
    }
}
