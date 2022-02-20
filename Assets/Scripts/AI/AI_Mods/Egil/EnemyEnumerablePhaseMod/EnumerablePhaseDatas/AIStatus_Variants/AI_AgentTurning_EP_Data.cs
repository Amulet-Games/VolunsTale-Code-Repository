using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/AI_AgentTurning_EP_Data")]
    public class AI_AgentTurning_EP_Data : EnumerablePhaseData
    {
        [Header("Agent Turing EP Data.")]
        public float _nextPhaseInplaceTurnSpeed = 5;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.maxUpperBodyIKTurningSpeed = _nextPhaseInplaceTurnSpeed;
        }
    }
}