using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Scores/Timer Score Calculation")]
    public class Timer_ScoreCalculation : AI_ScoreFactor
    {
        [Header("Timer Rate")]
        public float timer;
        public float timerRate = 0.02f;

        public override int Calculate(AIManager ai)
        {
            int retVal = 0;
            timer += ai._delta;
            if (timer >= timerRate)
            {
                timer = 0;
                retVal = m_ReturnScore;
            }

            return retVal;
        }
    }
}