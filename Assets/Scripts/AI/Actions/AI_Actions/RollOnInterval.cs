using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Roll On Interval")]
    public class RollOnInterval : AIAction
    {
        [Header("Action Info")]
        [Tooltip("The Enum that speciflied which roll anim state will be played, " +
                 "\"Roll_tree\" would be 2-dimensions with random direction, " +
                 "\"Two_Stance_roll\" would only work for enemy using two stance combat mod.")]
        public RollAnimStateEnum targetRollAnim;

        [Header("Mods")]
        [Tooltip("Check this if this is a hit Count Action, requires \"HitCountingMod\" to work.")]
        public bool isHitCountAction;

        [Tooltip("Check this if this is a player Spammed Attacking Action, requires \"observePlayerStatusMod\" to work.")]
        public bool isSpammedAttackingAction;

        [Tooltip("Don't set this to bigger than 0 if no consuming Stamina is intended. If this value is bigger than 0 then it will try to Deplete Enemy stamina by reaching out to mods." +
        " Which will require 'Enemy Stamina Mod' or 'Egil Stamina Mod' to work. ")]
        public float staminaUsage;

        public override int TotalScoreForeachAction(AIManager aiManager)
        {
            int retVal = 0;

            if (aiManager.GetEnemyRolledBool())
                return retVal;

            for (int i = 0; i < scoreFactors.value.Length; i++)
            {
                retVal += scoreFactors.value[i].Calculate(aiManager);
            }

            return retVal;
        }

        public override void Execute(AIManager ai)
        {
            ai.aIStates.Set_AnimMoveRmType_ToRoll();
            ai.isPausingTurnWithAgent = true;

            PlayRollAnimation();

            ai.SetEnemyRolledBoolToTrue();

            if (staminaUsage > 0)
                ai.DepleteEnemyStamina(staminaUsage);

            if (isHitCountAction)
                ai.ResetHitCountingStates();

            if (isSpammedAttackingAction)
                ai.ResetSpammedAttackingStatus();

            ai.currentAction = null;

            void PlayRollAnimation()
            {
                switch (targetRollAnim)
                {
                    case RollAnimStateEnum.e_roll_tree:
                        ai.Play2DRollAnimation(GetRandomDirection_All());
                        break;
                    case RollAnimStateEnum.e_roll_tree_F_L_R:
                        ai.Play2DRollAnimation(GetRandomDirection_F_L_R());
                        break;
                    case RollAnimStateEnum.e_roll_tree_B_L_R:
                        ai.Play2DRollAnimation(GetRandomDirection_B_L_R());
                        break;
                    case RollAnimStateEnum.two_stance_roll:
                        ai.PlayTwoStanceRollAnimation();
                        break;
                    case RollAnimStateEnum.e_roll_1:
                    case RollAnimStateEnum.e_roll_2:
                    case RollAnimStateEnum.e_roll_3:
                        ai.Play1DRollAnimation(targetRollAnim);
                        break;
                }
            }
        }
        
        Vector2 GetRandomDirection_All()
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

        Vector2 GetRandomDirection_F_L_R()
        {
            int randomResult = Random.Range(1, 4);

            if (randomResult == 1)
            {
                return new Vector2(1, 0);
            }
            else if (randomResult == 2)
            {
                return new Vector2(-1, 0);
            }
            else
            {
                return new Vector2(0, 1);
            }
        }

        Vector2 GetRandomDirection_B_L_R()
        {
            int randomResult = Random.Range(1, 4);

            if (randomResult == 1)
            {
                return new Vector2(1, 0);
            }
            else if (randomResult == 2)
            {
                return new Vector2(-1, 0);
            }
            else
            {
                return new Vector2(0, -1);
            }
        }

        public enum RollAnimStateEnum
        {
            e_roll_1,
            e_roll_2,
            e_roll_3,
            e_roll_tree,
            two_stance_roll,
            e_roll_tree_F_L_R,
            e_roll_tree_B_L_R
        }
    }
}