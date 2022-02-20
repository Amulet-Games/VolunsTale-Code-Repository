using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Roll Attack Ready")]
    public class RollAttackReady : AIAction
    {
        [Header("Action Info")]
        [Tooltip("The ex attack that enemy will perform following up this roll.")]
        public MSA_RollAttack MSARollAttack;

        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator, \"Roll_tree\" would be 2-dimensions with random direction")]
        public RollAttackReadyAnimStateEnum targetRollAttackReadyAnim;

        [Header("Mods")]
        [Tooltip("Check this if this is a perlious attack instead of normal attack, requires \"PerilousAttackMod\" to work.")]
        public bool isPerliousAttack;

        [Tooltip("Check this if this is a hit Count Action, requires \"HitCountingMod\" to work.")]
        public bool isHitCountAction;

        [Tooltip("Check this if this is a right stance attack, requires \"TwoStanceCombatMod\" to work.")]
        public bool isRightStance;

        [Tooltip("Don't set this to bigger than 0 if no consuming Stamina is intended. If this value is bigger than 0 then it will try to Deplete Enemy stamina by reaching out to mods." +
        " Which will require 'Enemy Stamina Mod' or 'Egil Stamina Mod' to work. ")]
        public float staminaUsage;

        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (ai.enemyAttacked)
                return retVal;

            // If this is a perlious attack but enemy hasn't wait long enough since the previous perlious attack, return.
            if (isPerliousAttack)
            {
                if (ai.GetUsedPerilousAttackBool())
                {
                    return retVal;
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
                        return retVal;
                    }
                }
                // If this is a left stance attack
                else
                {
                    // But enemy currently is in right stance
                    if (ai.GetIsRightStanceBool())
                    {
                        return retVal;
                    }
                }
            }

            for (int i = 0; i < scoreFactors.value.Length; i++)
            {
                retVal += scoreFactors.value[i].Calculate(ai);
            }

            return retVal;
        }

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToRoll();
            ai.isPausingTurnWithAgent = true;

            switch (targetRollAttackReadyAnim)
            {
                case RollAttackReadyAnimStateEnum.e_roll_attack_1_ready_roll_tree:

                    ai.PlayRollAttackReadyAnimTwoDimension(targetRollAttackReadyAnim, GetRandomScore());
                    break;

                case RollAttackReadyAnimStateEnum.e_roll_attack_2_ready_roll_tree:

                    ai.PlayRollAttackReadyAnimTwoDimension(targetRollAttackReadyAnim, GetRandomScore());
                    break;

                case RollAttackReadyAnimStateEnum.e_roll_attack_3_ready_roll_tree:

                    ai.PlayRollAttackReadyAnimTwoDimension(targetRollAttackReadyAnim, GetRandomScore());
                    break;

                case RollAttackReadyAnimStateEnum.e_roll_attack_1_ready:

                    ai.PlayRollAttackReadyAnim(targetRollAttackReadyAnim);
                    break;

                case RollAttackReadyAnimStateEnum.e_roll_attack_2_ready:

                    ai.PlayRollAttackReadyAnim(targetRollAttackReadyAnim);
                    break;

                case RollAttackReadyAnimStateEnum.e_roll_attack_3_ready:

                    ai.PlayRollAttackReadyAnim(targetRollAttackReadyAnim);
                    break;
            }

            InitLinkedRollAttack(ai);

            ai.currentAction = null;
        }

        Vector2 GetRandomScore()
        {
            int randomResult = Random.Range(1, 101);

            if (randomResult >= 67)
            {
                return new Vector2(1, 0);
            }
            else if (randomResult >= 34)
            {
                return new Vector2(-1, 0);
            }
            else if (randomResult >= 17)
            {
                return new Vector2(0, 1);
            }
            else
            {
                return new Vector2(0, -1);
            }
        }

        void InitLinkedRollAttack(AIManager ai)
        {
            if (MSARollAttack != null)
            {
                ai.currentMultiStageAttack = MSARollAttack;
                MSARollAttack.isPerliousAttack = isPerliousAttack;
                MSARollAttack.isHitCountAction = isHitCountAction;
                MSARollAttack.staminaUsage = staminaUsage;
            }
            else
            {
                ai.currentMultiStageAttack = null;
            }
        }

        public enum RollAttackReadyAnimStateEnum
        {
            e_roll_attack_1_ready,
            e_roll_attack_2_ready,
            e_roll_attack_3_ready,
            e_roll_attack_1_ready_roll_tree,
            e_roll_attack_2_ready_roll_tree,
            e_roll_attack_3_ready_roll_tree
        }
    }
}