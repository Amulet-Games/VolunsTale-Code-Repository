using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Power Weapons Actions/AI Power Weapon Attack On Interval")]
    public class PW_AttackOnInterval : BaseAttackOnInterval
    {
        [Tooltip("The next power weapon attack follow up from this attack.")]
        public MSA_PW_AICombo PW_AICombo;

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
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.currentAttackVelocity = attackRootMotionVelocity;
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;

            ai.isPausingTurnWithAgent = true;
            ai.currentPlayerPredictOffset = playerPredictOffset;

            ai.currentAttackRefs = _aiAttackRefs;

            if (useCrossFade)
            {
                ai.CrossFade_PW_AttackAnim(targetAttackAnim, _crossFadeValue);
            }
            else
            {
                ai.Play_PW_AttackAnim(targetAttackAnim);
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

            if (applyControllerCameraYMovement)
                ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;

            if (applyControllerCameraZoom)
                ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;

            ai.currentAction = null;

            void SetComboAction()
            {
                if (PW_AICombo)
                {
                    if (!ai.GetIsCurrentPowerWeaponBroke())
                    {
                        ai.currentMultiStageAttack = PW_AICombo;
                    }
                    else
                    {
                        ai.currentMultiStageAttack = null;
                    }
                }
                else
                {
                    ai.currentMultiStageAttack = null;
                }
            }
        }
    }
}