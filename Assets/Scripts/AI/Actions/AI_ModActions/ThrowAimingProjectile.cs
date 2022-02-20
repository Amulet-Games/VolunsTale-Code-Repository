using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Mod Actions/ThrowAimingProjectile")]
    public class ThrowAimingProjectile : AIModAction
    {
        [Header("Attack Refs.")]
        [Tooltip("The references that included all the nesscary infos to deal damage to player.")]
        [SerializeField] protected AI_AttackRefs _throwAimingProjectileAttackRefs;

        [Header("Action Info")]
        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public AimAttackAnimStateEnum targetAimAttackAnim;

        [Tooltip("Check this if to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;

        [Tooltip("How much base damage is this attack deals to player without any buff.")]
        public float damageToPlayer;

        [Header("Attack RootMotion Info")]
        [Tooltip("Check this if you Don't want to change attack root motion velocity based on distance to player.")]
        public bool ignoreRootMotionCalculation;

        [Tooltip("How far the enemy move when performing this attack?")]
        public float attackRootMotionVelocity;

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToAttack();
            ai.currentAttackVelocity = attackRootMotionVelocity;
            ai.ignoreAttackRootMotionCalculate = ignoreRootMotionCalculation;
            ai.currentAttackRefs = _throwAimingProjectileAttackRefs;

            ai.PlayAimAttackAnim(targetAimAttackAnim, useCrossFade);
        }

        public enum AimAttackAnimStateEnum
        {
            e_aim_attack_1,
            e_aim_attack_2,
        }
    }
}