using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Throw Returnal Projectile Wait Score")]
    public class ThrowReturnalProjectileWaitScore : AI_ScoreFactor
    {
        private void OnEnable()
        {
            Notes = "Returns returnScore when enemy \"isThrowProjectile\" is false.";
        }

        public override int Calculate(AIManager ai)
        {
            if (!ai.GetIsThrowProjectile())
            {
                return m_ReturnScore;
            }

            return 0;
        }
    }
}