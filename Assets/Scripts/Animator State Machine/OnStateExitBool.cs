using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateExitBool : StateMachineBehaviour
{
    public string boolName;
    public bool status;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, status);
    }
}
