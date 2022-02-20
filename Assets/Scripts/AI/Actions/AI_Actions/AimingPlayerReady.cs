using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Aiming Player Ready")]
    public class AimingPlayerReady : AIAction
    {
        [Header("Action Info")]
        [Tooltip("The animation state enum that associate to this action, this enum has to match the correct animation state inside enemy animator")]
        public StartAimingAnimStateEnum targetStartAimingAnim;

        [Tooltip("Check this if to use animator.crossFade instead of animator.play")]
        public bool useCrossFade;
        
        public override void Execute(AIManager ai)
        {
            ai.isPausingTurnWithAgent = true;
            ai.PlayAimingPlayerReadyAnim(targetStartAimingAnim, useCrossFade);
            ai.skippingScoreCalculation = true;
            ai.currentAction = null;
        }
        
        public enum StartAimingAnimStateEnum
        {
            e_aim_start,
            e_aim_loop,
        }
    }

}