using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/AI_RootMotion_EP_Data")]
    public class AI_RootMotion_EP_Data : EnumerablePhaseData
    {
        [Header("Root Motion Velocity EP Data.")]
        public float _nextPhaseRollVelocity = 165;
        public float _nextPhaseFallbackVelocity = 400;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.currentFallbackVelocity = _nextPhaseFallbackVelocity;
            _ai.currentRollVelocity = _nextPhaseRollVelocity;
        }
    }
}