using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Returnal Projectile Throw Attack Interval")]
    public class ReturnalProjectileThrowAttackInterval : BaseAttackOnInterval
    {
        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (ai.GetIsThrowProjectile())
                return retVal;

            if (CheckActionEligible(ai))
            {
                if (CheckIsTargetOutsideValidHeight(ai))
                    return retVal;

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

            ai.PlayReturnalProjectileThrowAttackAnim(useCrossFade);
            
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

        bool CheckIsTargetOutsideValidHeight(AIManager _ai)
        {
            if (Mathf.Abs(_ai.playerStates.mTransform.position.y - _ai.mTransform.position.y) >= 0.45)
            {
                return true;
            }

            return false;
        }
    }
}