using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Monitor Can Quit Neglect State Early")]
    public class MonitorCanQuitNeglectStateEarly : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.MonitorCanQuitNeglectStateEarly();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}