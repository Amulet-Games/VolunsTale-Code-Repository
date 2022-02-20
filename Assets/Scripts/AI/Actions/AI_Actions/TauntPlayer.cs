using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Taunt Player")]
    public class TauntPlayer : AIAction
    {
        [Header("Action Info")]
        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public TauntAnimStateEnum targetTauntAnim;

        [Tooltip("Check this to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;

        [Tooltip("Check this to disable enemy head ik when taunting player.")]
        public bool disableHeadIK;

        public float _playerPredictOffset;

        [Header("Mods")]
        [Tooltip("Check this if this is a hit Count Action, requires \"HitCountingMod\" to work.")]
        public bool isHitCountAction;

        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (ai.GetTauntedPlayerBool())
            {
                return retVal;
            }

            for (int i = 0; i < scoreFactors.value.Length; i++)
            {
                retVal += scoreFactors.value[i].Calculate(ai);
            }

            return retVal;
        }

        public override void Execute(AIManager ai)
        {
            ai.SetTauntedPlayerToTrue();
            ai.isPausingTurnWithAgent = disableHeadIK;
            ai.currentPlayerPredictOffset = _playerPredictOffset;

            ai.PlayTauntPlayerAnim(targetTauntAnim, useCrossFade);

            if (isHitCountAction)
                ai.ResetHitCountingStates();

            ai.currentAction = null;
        }

        public enum TauntAnimStateEnum
        {
            e_taunt_1,
            e_taunt_2,
            e_taunt_3
        }
    }
}