using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/EgilStaminaMod_2ndPhase_EP_Data")]
    public class EgilStaminaMod_2ndPhase_EP_Data : EnumerablePhaseData
    {
        [Header("Egil Stamina Mod EP Data.")]
        public float _nextPhaseEgilStaminaMaxSpeed = 60;
        public float _nextPhaseEgilStaminaMinSpeed = 56;
        public int _nextPhaseEgilStaminaMaxActionAmount = 12;
        public int _nextPhaseEgilStaminaLeastActionAmount = 6;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.EgilStaminaMod_SetNewPhaseData_2ndPhase(this);
        }
    }
}