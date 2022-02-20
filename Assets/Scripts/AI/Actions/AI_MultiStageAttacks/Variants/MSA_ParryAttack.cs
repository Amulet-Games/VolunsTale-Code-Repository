using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Multi Stage Attacks/MSA Parry Attack")]
    public class MSA_ParryAttack : AI_MultiStageAttack
    {
        [Header("Parry Attack Info")]
        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public ParryAttackStateEnum targetParryAttackAnim;

        [Tooltip("Check this if to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.currentAttackVelocity = attackRootMotionVelocity;
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;
            
            ai.currentPlayerPredictOffset = _playerPredictOffset;

            ai.currentAttackRefs = _aiMSARefs;

            ai.PlayParryAttackMultiStagetAttackAnim(targetParryAttackAnim, useCrossFade);

            if (staminaUsage > 0)
                ai.DepleteEnemyStamina(staminaUsage);

            // Camera effects
            ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;
            ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;
        }
    }
}