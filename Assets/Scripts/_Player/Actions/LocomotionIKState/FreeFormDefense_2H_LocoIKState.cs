using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/LocomotionIKState/FreeForm 2H Defense Locomotion IKState")]
    public class FreeFormDefense_2H_LocoIKState : BaseLocoIKState
    {
        ///* Handle IK just like update, execute in 'IKWithPlayer' StateAction.
        public override void Tick(StateManager _states)
        {
            _states.FreeFormDefenseLocoIKState_2H_Tick();
        }
        
        ///* Handle IK when player started running in this state.
        public override void OnRunningIK(StateManager _states)
        {
            _states.FreeFormDefenseLocoIKState_2H_OnRunningIK();
        }

        ///* Handle IK when player stopped running in this state.
        public override void OffRunningIK(StateManager _states)
        {
            _states.FreeFormDefenseLocoIKState_2H_OffRunningIK();
        }

        ///* It is irrelevant for this state.
        public override void OnTwoHanding(StateManager _states)
        {
        }

        ///* Handle IK when player stopped two handing weapon in this state.
        public override void OffTwoHanding(StateManager _states)
        {
            _states.FreeFormDefenseLocoIKState_2H_OffTwoHanding();
        }

        ///* It is irrelevant for this state.
        public override void OnDefense(StateManager _states)
        {
        }

        ///* Handle IK when player stopped defensing himself in this state.
        public override void OffDefense(StateManager _states)
        {
            _states.FreeFormDefenseLocoIKState_2H_OffDefense();
        }

        ///* Handle IK when player started lockon to enemy in this state.
        public override void OnLockon(StateManager _states)
        {
            _states.FreeFormDefenseLocoIKState_2H_OnLockon();
        }

        ///* It is irrelevant for this state.
        public override void OffLockon(StateManager _states)
        {
        }
    }
}