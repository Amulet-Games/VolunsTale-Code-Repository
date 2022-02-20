using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/AI_LockonMoveProfile_EP_Data")]
    public class AI_LockonMoveProfile_EP_Data : EnumerablePhaseData
    {
        [Header("Lockon Move Profile EP Data.")]
        public AI_LockonMoveProfile _nextPhaseLockonMoveProfile;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.currentAILockonMoveProfile = _nextPhaseLockonMoveProfile;
        }
    }
}