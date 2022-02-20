using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Monitor Can Quit Fighter Mode Early")]
    public class MonitorCanQuitFighterModeEarly : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.MonitorCanQuitFighterModeEarly();
        }

        public override void AIExecute(AIStateManager aiStates)
        {
        }
    }
}