using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/Egil_KMA_Mod_EP_Data")]
    public class Egil_KMA_Mod_EP_Data : EnumerablePhaseData
    {
        [Header("Egil KMA Mod EP Data")]
        public float _nextPhaseFallGravity = -60;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.Egil_KMA_Mod_SetNewPhaseData(this);
        }
    }
}