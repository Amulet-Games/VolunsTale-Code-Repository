using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class OnStateEnterBoolRandom : StateMachineBehaviour
    {
        [Header("Percentage")]
        [Tooltip("The percentage of chances that this boolean anim parameter will change it current status to target status.")]
        public int chance;

        [Header("Target Bool Parameter")]
        [Tooltip("The boolean anim parameter that you want to change.")]
        public string boolName;

        [Header("Target Status")]
        [Tooltip("The target status that this boolean anim parameter will change to once it enter to this state.")]
        public bool status;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (Random.Range(1, 100f) <= chance)
            {
                animator.SetBool(boolName, status);
            }
        }
    }
}