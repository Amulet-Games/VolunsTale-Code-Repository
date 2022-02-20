using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_ScoreFactor : ScriptableObject
    {
        [TextArea(1, 30)]
        public string Notes = "This component shouldn't be removed, it does important stuff.";

        [Header("Return Score")]
        public int m_ReturnScore;

        public abstract int Calculate(AIManager ai);
    }
}