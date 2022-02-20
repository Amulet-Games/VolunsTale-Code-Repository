using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Attack On Interval")]
    public class AttackOnInterval : BaseAttackOnInterval
    {
        [Tooltip("The next attack follow up from this attack.")]
        public MSA_BaseAICombo AICombo;

        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public AttackAnimStateEnum targetAttackAnim;
        [Tooltip("If choose to play attack anim with crossFade, this value define the smooth time of transition.")]
        public float _crossFadeValue = 0.2f;

        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

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
            // If you need to create artifical attack root motion, plz set "applyAttackRootMotion" through animation event.
            if (useArtifiMotion)
            {
                ai.applyAttackArtifMotion = true;
                ai.currentAttackVelocity = attackRootMotionVelocity;
            }
            else
            {
                ai.aIStates.Set_AnimMoveRmType_ToAttack();
                ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;
                ai.currentAttackVelocity = attackRootMotionVelocity;
            }

            ai.isPausingTurnWithAgent = true;
            ai.currentPlayerPredictOffset = playerPredictOffset;

            ai.currentAttackRefs = _aiAttackRefs;

            if (useCrossFade)
            {
                ai.CrossFadeDefaultAttackAnim(targetAttackAnim, _crossFadeValue);
            }
            else
            {
                ai.PlayDefaultAttackAnim(targetAttackAnim);
            }
            
            SetComboAction();

            if (staminaUsage > 0)
                ai.DepleteEnemyStamina(staminaUsage);

            if (isPerliousAttack)
                ai.SetUsedPerilousAttackToTrue();

            if (isHitCountAction)
                ai.ResetHitCountingStates();

            if (isSpammedBlockingAction)
                ai.ResetSpammedBlockingStatus();

            ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;
            ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;

            ai.currentAction = null;

            void SetComboAction()
            {
                if (AICombo != null)
                {
                    ai.currentMultiStageAttack = AICombo;
                }
                else
                {
                    ai.currentMultiStageAttack = null;
                }
            }
        }
    }
}