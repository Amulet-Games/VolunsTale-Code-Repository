using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Spammed Blocking ReturnScore")]
    public class SpammedBlockingReturnScore : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "Returns returnScore when enemy \"hasSpammedBlocking\" is true.";
        }

        public override int Calculate(AIManager ai)
        {
            if (ai.GetHasSpammedBlockingBool())
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}