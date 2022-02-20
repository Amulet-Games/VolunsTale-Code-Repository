using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_BaseExecution_Profile : ScriptableObject
    {
        [Header("Execution Type.")]
        public AnimStateVariable _presentAnimState;
        public AnimStateVariable _receiveAnimState;
        public bool _isGetupFromFaceUp;
        public Vector3 _playerLocalEulers;

        [Header("Damages In Parts.")]
        public float _1st_executionDamage;
        public float _2nd_executionDamage;
        public float _3rd_executionDamage;
        public float _4th_executionDamage;
        public float _5th_executionDamage;

        [Header("Temp Refs.")]
        [NonSerialized] public Transform _executionParentPoint;
        [NonSerialized] public AIManager _executionerAI;

        public abstract void KnockBackFromExecution(Rigidbody _rb);
    }
}