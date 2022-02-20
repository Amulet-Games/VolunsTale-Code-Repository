using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Damage Particle Attack On Interval")]
    public class DP_AttackOnInterval : AIAction
    {
        [Header("Attack Refs.")]
        [Tooltip("The references that included all the nesscary infos to deal damage to player.")]
        [SerializeField] protected AI_AttackRefs _dpAttacksRef;

        [Header("Mods")]
        [Tooltip("Check this if this is a hit Count Action, requires \"HitCountingMod\" to work.")]
        public bool isHitCountAction;

        [Tooltip("Check this if this is a player Spammed Blocking Action, requires \"observePlayerStatusMod\" to work.")]
        public bool isSpammedBlockingAction;

        [Tooltip("Check this if this is attack consum stamina, requires \"StaminaUsageMod\" to work.")]
        public bool isConsumStamina;

        [Tooltip("How much stamina this action cost, requires \"StaminaUsageMod\" to work.")]
        public float staminaUsage;

        [Header("Attack Roots.")]
        [Tooltip("Check this if you DON'T want to change attack root motion velocity based on enemy distance to player.")]
        public bool ignoreRootMotionCalculation;

        [Tooltip("How far the enemy moves when attacking.")]
        public float attackRootMotionVelocity;

        public float playerPredictOffset;

        [Header("Special Camera Effect")]
        [Tooltip("Check this if you want player's camera follow enemy Y axis movement.")]
        public bool applyControllerCameraYMovement;

        [Tooltip("Check this if you want player camera zoom in.")]
        public bool applyControllerCameraZoom;

        [Header("Action Info")]
        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public DamageParticleAttackMod.DpAttackAnimStateEnum targetAttackAnim;
        
        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (ai.enemyAttacked)
                return retVal;

            if (ai.GetUsedDpAttack())
                return retVal;

            for (int i = 0; i < scoreFactors.value.Length; i++)
            {
                retVal += scoreFactors.value[i].Calculate(ai);
            }

            return retVal;
        }

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;
            ai.currentAttackVelocity = attackRootMotionVelocity;
            
            ai.isPausingTurnWithAgent = true;
            ai.currentPlayerPredictOffset = playerPredictOffset;

            ai.currentAttackRefs = _dpAttacksRef;

            ai.HandleDpAttack(targetAttackAnim);
            
            if (isConsumStamina)
                ai.DepleteEnemyStamina(staminaUsage);
            
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