using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Multi Stage Attacks/MSA Roll Attack")]
    public class MSA_RollAttack : AI_MultiStageAttack
    {
        [Header("Ex Attack Info")]
        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public RollAttackAnimStateEnum targetRollAttackAnim;

        [Tooltip("Check this if to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;

        [HideInInspector] public bool isHitCountAction;
        [HideInInspector] public bool isPerliousAttack;

        public override void Execute(AIManager ai)
        {
            // Set AI Stats
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.currentAttackVelocity = attackRootMotionVelocity;
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;
            
            ai.currentPlayerPredictOffset = _playerPredictOffset;

            ai.currentAttackRefs = _aiMSARefs;

            // Play Anim
            ai.PlayRollMultiStageAttackAnim(targetRollAttackAnim, useCrossFade);
            
            // Mods
            if (staminaUsage > 0)
                ai.DepleteEnemyStamina(staminaUsage);

            if (isPerliousAttack)
                ai.SetUsedPerilousAttackToTrue();

            if (isHitCountAction)
                ai.ResetHitCountingStates();
            
            // Camera effects
            ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;
            ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;
        }
    }
}