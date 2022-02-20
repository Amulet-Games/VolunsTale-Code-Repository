using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/AI_AgentLocomotion_EP_Data")]
    public class AI_AgentLocomotion_EP_Data : EnumerablePhaseData
    {
        [Header("Agent Locomotion EP Data.")]
        public float _nextPhaseLocoAnimSwitchDistance;
        public float _nextPhaseAIAccelSpeed;
        public float _nextPhaseAIMoveSpeed;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.locoAnimSwitchDistance = _nextPhaseLocoAnimSwitchDistance;
            _ai._currentAgentMoveSpeed = _nextPhaseAIMoveSpeed;
            _ai._currentAgentAccelSpeed = _nextPhaseAIAccelSpeed;
        }
    }
}