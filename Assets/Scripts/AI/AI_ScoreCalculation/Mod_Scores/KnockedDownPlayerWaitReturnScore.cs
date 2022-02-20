using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/KnockedDownPlayerWaitReturnScore")]
    public class KnockedDownPlayerWaitReturnScore : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "Returns returnScore when enemy \"IsKnockDownPlayerWait\" bool is true.\nOtherwise returns 0.";
        }

        public override int Calculate(AIManager ai)
        {
            if (ai.GetIsKnockDownPlayerWait())
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}