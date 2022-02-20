using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Player IK Goal")]
    public class PlayerIKGoal : ScriptableObject
    {
        public Vector3 _goalLocalPosition;
        public Vector3 _goalLocalEulers;
        [Range(0f, 1f)] public float _goalWeight;

        public void OverwriteHelperTransform(Transform _referedHelper)
        {
            _referedHelper.localPosition = _goalLocalPosition;
            _referedHelper.localEulerAngles = _goalLocalEulers;
        }
    }
}