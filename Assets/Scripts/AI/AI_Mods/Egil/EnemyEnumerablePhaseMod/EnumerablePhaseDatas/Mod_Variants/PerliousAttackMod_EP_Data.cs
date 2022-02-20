using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/PerliousAttackMod_EP_Data")]
    public class PerliousAttackMod_EP_Data : EnumerablePhaseData
    {
        [Header("Egil Perlious Attack Mod EP Data.")]
        public float _nextPhasePerliousAttackRate;
        public float _nextPhasePerliousAttackRandomizedRate;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.PerliousAttackMod_SetNewPhaseData(this);
        }
    }
}