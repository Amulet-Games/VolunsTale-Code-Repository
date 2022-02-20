using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Throw Attack On Interval")]
    public class ThrowAttackOnInterval : BaseAttackOnInterval
    {
        public ThrowAttackAnimStateEnum targetThrowAttackAnim;

        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (ai.currentThrowableWeapon == null)
                return retVal;

            if (CheckActionEligible(ai))
            {
                for (int i = 0; i < scoreFactors.value.Length; i++)
                {
                    retVal += scoreFactors.value[i].Calculate(ai);
                }
            }

            return retVal;
        }

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.currentAttackVelocity = attackRootMotionVelocity;
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;

            ai.isPausingTurnWithAgent = true;
            ai.currentPlayerPredictOffset = playerPredictOffset;

            ai.currentAttackRefs = _aiAttackRefs;

            ai.PlayThrowableAttackAnim(targetThrowAttackAnim, useCrossFade);
            
            if (staminaUsage > 0)
                ai.DepleteEnemyStamina(staminaUsage);

            if (isPerliousAttack)
                ai.SetUsedPerilousAttackToTrue();

            if (isHitCountAction)
                ai.ResetHitCountingStates();

            if (isSpammedBlockingAction)
                ai.ResetSpammedBlockingStatus();

            if (applyControllerCameraYMovement)
                ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;

            if (applyControllerCameraZoom)
                ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;

            ai.currentAction = null;
        }
    }
}