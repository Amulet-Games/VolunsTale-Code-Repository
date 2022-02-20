using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Kinematic Motion Attack On Interval")]
    public class KMA_AttackOnInterval : AIAction
    {
        [Header("KMA Config.")]
        public _KMA_ActionData _KMA_profile;
        
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

            // If this is a perlious attack but enemy hasn't wait long enough since the previous perlious attack, return.
            if (_KMA_profile.isPerliousAttack)
            {
                if (ai.GetUsedPerilousAttackBool())
                {
                    return retVal;
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
            ai.isPausingTurnWithAgent = true;

            ai.Execute_KMJ(_KMA_profile);

            // Camera effects
            ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;
            ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;

            ai.currentAction = null;
        }
    }

    [Serializable]
    public class _KMA_ActionData
    {
        [Header("Base Config.")]
        public KMA_AttackOnIntervalTypeEnum _KMA_Type;
        public bool _isKMAWaitNeeded;

        [Header("MSA Combo.")]
        [Tooltip("Set if MSA Combo would be used after this KMA executed.")]
        public bool _isUseCombo;
        [Tooltip("MSA Combo that would be used, note that _AICombo here won't be used if _KMA_Type is set to Random, KMA Profile's AI Combo would be used instead.")]
        public MSA_AICombo _AICombo;

        [Header("Mods")]
        [Tooltip("Check this if this is a perlious attack instead of normal attack, requires \"PerilousAttackMod\" to work.")]
        public bool isPerliousAttack;

        public enum KMA_AttackOnIntervalTypeEnum
        {
            V1_ByPhase,
            V2_ByPhase,
            RandomByPhase
        }
    }
}