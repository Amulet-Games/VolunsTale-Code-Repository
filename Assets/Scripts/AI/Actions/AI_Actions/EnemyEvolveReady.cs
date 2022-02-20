﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/Enemy Evolve Ready")]
    public class EnemyEvolveReady : AIAction
    {
        public override int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            if (!ai.GetIsEnemyEvolvable())
                return retVal;

            for (int i = 0; i < scoreFactors.value.Length; i++)
            {
                retVal += scoreFactors.value[i].Calculate(ai);
            }

            return retVal;
        }

        public override void Execute(AIManager ai)
        {
            ai.SetIsEvolveStartedStatusToTrue();
        }
    }
}