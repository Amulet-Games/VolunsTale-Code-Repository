using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Monitor Animation Jobs")]
    public class MonitorAnimationJobs : MonoAction
    {
        public override void Execute(StateManager states)
        {
            states.a_hook.MonitorAnimationJobs();
        }
    }
}