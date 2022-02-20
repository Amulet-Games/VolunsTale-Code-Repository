using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Move In Fix Direction")]
    public class MoveInFixDirection : AIAction
    {
        [Header("Action Info")]
        public FixDirectionMoveMod.FixDirectionTypeEnum moveEnum;

        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (ai.GetIsFixDirectionInCooldownBool())
                return retVal;

            for (int i = 0; i < scoreFactors.value.Length; i++)
            {
                retVal += scoreFactors.value[i].Calculate(ai);
            }

            return retVal;
        }

        public override void Execute(AIManager ai)
        {
            // RESET BOOLEANS
            ai.isPausingTurnWithAgent = true;
            ai.isLockOnMoveAround = false;
            ai.isMovingToward = false;
            
            ai.SetIsMovingFixDirectionToTrue();
        }
    }
}
