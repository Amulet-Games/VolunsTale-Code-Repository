using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/LocomotionIKState/Freeform 1H Locomotion IK State")]
    public class FreeForm_1H_LocoIKState : BaseLocoIKState
    {
        ///* Handle IK just like update, execute in 'IKWithPlayer' StateAction.
        public override void Tick(StateManager _states)
        {
            _states.FreeFormLocoIKState_1H_Tick();
        }
        
        ///* Handle IK when player start running in this state.
        public override void OnRunningIK(StateManager _states)
        {
            _states.FreeFormLocoIKState_1H_OnRunningIK();
        }

        ///* Handle IK when player stop running in this state.
        public override void OffRunningIK(StateManager _states)
        {
            _states.FreeFormLocoIKState_1H_OffRunningIK();
        }

        ///* Handle IK when player start two handing weapon in this state.
        public override void OnTwoHanding(StateManager _states)
        {
            _states.FreeFormLocoIKState_1H_OnTwoHanding();
        }

        ///* It is irrelevant for this state.
        public override void OffTwoHanding(StateManager _states)
        {
        }

        ///* Handle IK when player started defensing himself in this state.
        public override void OnDefense(StateManager _states)
        {
            _states.FreeFormLocoIKState_1H_OnDefense();
        }

        ///* It is irrelevant for this state.
        public override void OffDefense(StateManager _states)
        {
        }

        ///* Handle IK when player started lockon to enemy in this state.
        public override void OnLockon(StateManager _states)
        {
            _states.FreeFormLocoIKState_1H_OnLockon();
        }

        ///* It is irrelevant for this state.
        public override void OffLockon(StateManager _states)
        {
        }
    }
}