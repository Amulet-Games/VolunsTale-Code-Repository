using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Execution Attack On Interval")]
    public class EXE_AttackOnInterval : AIAction
    {
        [Header("Attack Refs.")]
        [Tooltip("The references that included all the nesscary infos to deal damage to player." +
            "Note that this only applied when Egil hit obstacle along with player in execution attack." +
            "Execution Attack Refs will be hidden in 'EgilExecutionMod'.")]
        public AI_AttackRefs _exeAttacksRef;

        [Header("Mods")]
        [Tooltip("Don't set this to bigger than 0 if no consuming Stamina is intended. If this value is bigger than 0 then it will try to Deplete Enemy stamina by reaching out to mods." +
        " Which will require 'Enemy Stamina Mod' or 'Egil Stamina Mod' to work. ")]
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

        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (ai.enemyAttacked)
                return retVal;

            if (ai.GetIsExecutionWait())
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

            ai.currentAttackRefs = _exeAttacksRef;

            ai.TryCatchPlayerToExecute();

            if (staminaUsage > 0)
                ai.DepleteEnemyStamina(staminaUsage);
            
            if (applyControllerCameraYMovement)
                ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;

            if (applyControllerCameraZoom)
                ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;

            ai.currentAction = null;
        }
    }
}