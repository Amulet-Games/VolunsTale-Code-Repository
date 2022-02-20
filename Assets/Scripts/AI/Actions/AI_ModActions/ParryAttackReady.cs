using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Mod Actions/Parry Attack Ready")]
    public class ParryAttackReady : AIModAction
    {
        [Header("Action Info")]
        [Tooltip("The parry attack that follows up.")]
        public MSA_ParryAttack MSAParryAttack;

        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public ParryReadyAnimStateEnum targetParryReadyAnim;

        [Tooltip("Check this if to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;

        [Header("Attack RootMotion Info")]
        [Tooltip("Check this if you DON'T want to change attack root motion velocity based on enemy distance to player.")]
        public bool ignoreRootMotionCalculation;

        [Tooltip("How far the enemy moves when attacking.")]
        public float attackRootMotionVelocity;

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.isPausingTurnWithAgent = true;
            ai.currentAttackVelocity = attackRootMotionVelocity;
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;

            ai.currentMultiStageAttack = MSAParryAttack;

            ai.currentAction = null;
            
            ai.iKHandler.isUsingIK = false;
            ai.PlayParryAttackReadyAnim(targetParryReadyAnim, useCrossFade);
        }

        public enum ParryReadyAnimStateEnum
        {
            e_parry_attack_1_ready,
            e_RS_parry_attack_1_ready,
            e_LS_parry_attack_1_ready
        }
    }

}