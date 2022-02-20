using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/RollIntervalMod_EP_Data")]
    public class RollIntervalMod_EP_Data : EnumerablePhaseData
    {
        [Header("Roll Interval Mod EP Data.")]
        public float _nextPhaseStdRollIntervalRate = 7;
        public float _nextPhaseRollIntervalRandomizeAmount = 4;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.RollIntervalMod_SetNewPhaseData(this);
        }
    }
}