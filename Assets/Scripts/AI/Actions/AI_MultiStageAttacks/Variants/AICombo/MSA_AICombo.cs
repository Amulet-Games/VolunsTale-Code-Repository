using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Multi Stage Attacks/MSA AI_Combo")]
    public class MSA_AICombo : MSA_BaseAICombo
    {
        [Header("Combo Info")]
        [Tooltip("The next combo follow up from this combo.")]
        public MSA_BaseAICombo nextAICombo;

        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public ComboAnimStateEnum targetComboAnim;

        [Tooltip("Check this if to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;
        [Tooltip("If choose to play attack anim with crossFade, this value define the smooth time of transition.")]
        public float _crossFadeValue = 0.1f;

        [Header("Thershold")]
        [Tooltip("The distance thershold of this combo, if enemy's \"disToPlayer\" exceeded this range enemy will Not perform this combo, recommend to check the first attack action's \"scoreFactors\" range before setting this.")]
        public float rangeThershold;

        [Space(5)]
        [Tooltip("This option determines how much angle(degree) from \"angleToTarget\" would the enemy perform this combo, recommend to check the first attack action's \"scoreFactors\" angle options before setting this.")]
        public AngleOptionsTypeEnum angleThershold;

        [Tooltip("This option determines which direction would angle thershold calculation be based from.")]
        public DirectionOptionsTypeEnum directionThershold;

        public override void Execute(AIManager ai)
        {
            if (!CheckIfNextAIComboAvaliable())
                return;

            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.currentAttackVelocity = attackRootMotionVelocity;
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;
            ai.attackIntervalTimer = 0;

            ai.currentPlayerPredictOffset = _playerPredictOffset;

            ai.currentAttackRefs = _aiMSARefs;
            
            if (useCrossFade)
            {
                ai.CrossFadeAIComboMultiStageAttackAnim(targetComboAnim, _crossFadeValue);
            }
            else
            {
                ai.PlayAIComboMultiStageAttackAnim(targetComboAnim);
            }

            SetComboAction();

            if (staminaUsage > 0)
                ai.DepleteEnemyStamina(staminaUsage);

            ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;
            ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;

            /// Local Funcs.
            bool CheckIfNextAIComboAvaliable()
            {
                if (ai.GetIsEnemyTiredBool())
                {
                    return false;
                }
                else
                {
                    #region Check Range / Angle.
                    if (ai.CheckTargetInRange(rangeThershold))
                    {
                        if (directionThershold == DirectionOptionsTypeEnum.whole360)
                        {
                            return true;
                        }
                        else if (ai.CheckTargetInAngle(directionThershold, angleThershold))
                        {
                            return true;
                        }
                    }

                    return false;
                    #endregion
                }
            }

            void SetComboAction()
            {
                if (nextAICombo != null)
                {
                    ai.currentMultiStageAttack = nextAICombo;
                }
                else
                {
                    ai.currentMultiStageAttack = null;
                }
            }
        }
    }
}