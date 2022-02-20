using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/EgilStaminaMod_3rdPhase_EP_Data")]
    public class EgilStaminaMod_3rdPhase_EP_Data : EnumerablePhaseData
    {
        [Header("Egil Stamina Mod EP Data.")]
        public int _lastPhaseEgilStaminaActionAmount = 32;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.EgilStaminaMod_SetNewPhaseData_3rdPhase(this);
        }
    }
}