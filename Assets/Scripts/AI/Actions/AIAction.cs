using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AIAction : ScriptableObject
    {
        public AIScoreFactors scoreFactors;

        public virtual int TotalScoreForeachAction(AIManager ai)
        {
            int retVal = 0;

            for (int i = 0; i < scoreFactors.value.Length; i++)
            {
                retVal += scoreFactors.value[i].Calculate(ai);
            }

            return retVal;
        }

        public abstract void Execute(AIManager ai);
    }

    [System.Serializable]
    public class AIScoreFactors
    {
        public AI_ScoreFactor[] value;
    }

}