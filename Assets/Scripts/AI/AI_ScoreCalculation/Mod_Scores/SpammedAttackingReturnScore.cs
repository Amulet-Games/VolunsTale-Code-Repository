using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Spammed Attacking ReturnScore")]
    public class SpammedAttackingReturnScore : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "Returns returnScore when enemy \"hasSpammedAttacking\" is true.";
        }

        public override int Calculate(AIManager ai)
        {
            if (ai.GetHasSpammedAttackingBool())
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}