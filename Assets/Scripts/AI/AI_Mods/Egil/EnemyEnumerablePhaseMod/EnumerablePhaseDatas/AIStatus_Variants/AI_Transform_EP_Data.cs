using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/AI_Transform_EP_Data")]
    public class AI_Transform_EP_Data : EnumerablePhaseData
    {
        [Header("Enemy Transform EP Data.")]
        public Vector3 _aiWorldPos;
        public Vector3 _aiWorldEulers;

        public override void SetNewPhaseData(AIManager _ai)
        {
            _ai.mTransform.position = _aiWorldPos;
            _ai.mTransform.eulerAngles = _aiWorldEulers;
        }
    }
}