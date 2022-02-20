using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Passive Actions/Injured Revenge Attack")]
    public class InjuredRevengeAttack : AIPassiveAction
    {
        [Header("Attack Refs.")]
        [Tooltip("The references that included all the nesscary infos to deal damage to player.")]
        [SerializeField] protected AI_AttackRefs _aiAttackRefs;

        [Tooltip("How far the enemy moves when attacking.")]
        public float attackRootMotionVelocity;

        [Tooltip("The amount of turning predict of this attack needed in order to force enemy face player.")]
        public float playerPredictOffset;

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.currentAttackVelocity = attackRootMotionVelocity;

            ai.currentPlayerPredictOffset = playerPredictOffset;

            ai.currentAttackRefs = _aiAttackRefs;

            ai.PlayEgilRevengeAttackAnim();
        }
    }
}