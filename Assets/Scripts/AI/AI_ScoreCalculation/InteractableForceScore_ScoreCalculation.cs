using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Interactable Force Score.")]
    public class InteractableForceScore_ScoreCalculation : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "This is a Force Score if enemy has searched interactables but haven't execute it yet.";
        }

        public override int Calculate(AIManager ai)
        {
            if (ai.GetIsSwitchTargetToInteractable())
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}