using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/Enumerable Phase Data/AI_IKHelper_Transform_EP_Data")]
    public class AI_IKHelper_Transform_EP_Data : EnumerablePhaseData
    {
        [Header("Enemy IK Helper Transform EP Data.")]
        public Vector3 _ikHelper_localPos;
        public Vector3 _ikHelper_localEulers;

        public override void SetNewPhaseData(AIManager _ai)
        {
            Transform _IKTargetTransform = _ai.iKHandler.iKTarget.transform;

            _IKTargetTransform.localPosition = _ikHelper_localPos;
            _IKTargetTransform.localEulerAngles = _ikHelper_localEulers;

            _ai.targetPos = _ai.mTransform.forward;
        }
    }
}