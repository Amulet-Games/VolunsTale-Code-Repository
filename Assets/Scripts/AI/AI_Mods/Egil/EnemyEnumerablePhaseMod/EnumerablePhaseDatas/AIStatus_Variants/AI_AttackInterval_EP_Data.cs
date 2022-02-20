using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/AI_AttackInterval_EP_Data")]
    public class AI_AttackInterval_EP_Data : EnumerablePhaseData
    {
        [Header("Attack Interval EP Data.")]
        public float _nextPhaseStdAttackIntervalRate = 3.25f;
        public float _nextPhaseAttackRandomizeAmount = 2.75f;

        public override void SetNewPhaseData(AIManager _ai)
        {
            AIStateManager _aiStates = _ai.aIStates;

            _ai.maxAttackIntervalRate = _nextPhaseStdAttackIntervalRate;
            _ai.minAttackIntervalRate = _nextPhaseAttackRandomizeAmount;
        }
    }
}