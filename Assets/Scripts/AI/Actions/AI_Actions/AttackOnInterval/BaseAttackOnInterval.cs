using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BaseAttackOnInterval : AIAction
    {
        [Header("Attack Refs.")]
        [Tooltip("The references that included all the nesscary infos to deal damage to player.")]
        [SerializeField] protected AI_AttackRefs _aiAttackRefs;

        [Header("Mods")]
        [Tooltip("Check this if this is a perlious attack instead of normal attack, requires \"PerilousAttackMod\" to work.")]
        public bool isPerliousAttack;

        [Tooltip("Check this if this is a hit Count Action, requires \"HitCountingMod\" to work.")]
        public bool isHitCountAction;

        [Tooltip("Check this if this is a player Spammed Blocking Action, requires \"observePlayerStatusMod\" to work.")]
        public bool isSpammedBlockingAction;

        [Tooltip("Check this if this is a right stance attack, requires \"TwoStanceCombatMod\" to work.")]
        public bool isRightStance;

        [Tooltip("Don't set this to bigger than 0 if no consuming Stamina is intended. If this value is bigger than 0 then it will try to Deplete Enemy stamina by reaching out to mods." +
        " Which will require 'Enemy Stamina Mod' or 'Egil Stamina Mod' to work. ")]
        public float staminaUsage;

        [Header("Attack Roots.")]
        [Tooltip("Check this If this attack action's animation doesn't contains root motion/")]
        public bool useArtifiMotion;

        [Tooltip("Check this if you DON'T want to change attack root motion velocity based on enemy distance to player.")]
        public bool ignoreRootMotionCalculation;

        [Tooltip("How far the enemy moves when attacking.")]
        public float attackRootMotionVelocity;

        [Tooltip("The amount of turning predict of this attack needed in order to force enemy face player.")]
        public float playerPredictOffset;
        
        [Header("Special Camera Effect")]
        [Tooltip("Check this if you want player's camera follow enemy Y axis movement.")]
        public bool applyControllerCameraYMovement;

        [Tooltip("Check this if you want player camera zoom in.")]
        public bool applyControllerCameraZoom;

        [Header("Action Info")]
        [Tooltip("Check this to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;
        
        public bool CheckActionEligible(AIManager ai)
        {
            if (ai.enemyAttacked)
            {
                return false;
            }

            // If this is a perlious attack but enemy hasn't wait long enough since the previous perlious attack, return.
            if (isPerliousAttack)
            {
                if (ai.GetUsedPerilousAttackBool())
                {
                    return false;
                }
            }

            if (ai.GetCheckCombatStanceBool())
            {
                // If this is a right stance attack
                if (isRightStance)
                {
                    // But enemy currently is in left stance
                    if (!ai.GetIsRightStanceBool())
                    {
                        return false;
                    }
                }
                // If this is a left stance attack
                else
                {
                    // But enemy currently is in right stance
                    if (ai.GetIsRightStanceBool())
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        
        public enum AttackAnimStateEnum
        {
            e_attack_1,
            e_attack_2,
            e_attack_3,
            e_attack_4,
            e_attack_5,
            e_attack_6,
            e_attack_7,
            e_attack_8,
            e_attack_9,
            e_combo_1_a,
            e_combo_2_a,
            e_combo_3_a,
            e_combo_4_a,
            e_combo_5_a,
            e_combo_6_a,
            e_combo_7_a,
            e_combo_8_a,
            e_combo_9_a,
            e_combo_10_a,
            e_combo_11_a,
            e_combo_12_a,
        }

        public enum ThrowAttackAnimStateEnum
        {
            e_throw_attack_1,
            e_throw_attack_2,
            e_throw_attack_3
        }
    }
}