using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EgilExecutionMod : AIMod
    {
        [HideInInspector]
        public bool showEgilExecutionMod;

        public float executionWaitMaxTime;
        public float executionWaitMinTime;

        public Transform _execution1_ParentPoint;
        public Transform _execution2_ParentPoint;

        public AI_BaseExecution_Profile _executionProfile_1;
        public AI_BaseExecution_Profile _executionProfile_2;

        [Tooltip("Used in 'Execution Standard Damage Collider' to differentiate normal attack or execution attack.")]
        [ReadOnlyInspector] public bool _isExecutePresentAttack;
        [ReadOnlyInspector] public bool _isExecuteWait;

        [ReadOnlyInspector] public float _finalizedExecutionWaitTime;

        #region Non Serialized.
        [NonSerialized]
        public AIManager _ai;
        [NonSerialized]
        public float _delta;
        #endregion

        /// TICK.

        public void EgilExecutionTimeCount()
        {
            if (_isExecuteWait)
            {
                _finalizedExecutionWaitTime -= _delta;
                if (_finalizedExecutionWaitTime <= 0)
                {
                    OffIsExecuteWait();
                }
            }
        }

        /// INIT
        public void EgilExecutionModInit(AIManager _ai)
        {
            this._ai = _ai;
        }

        public void EgilExecutionGoesAggroReset()
        {
            _isExecuteWait = false;
            RandomizeExecutionWaitTime();
        }

        public void EgilExecutionExitAggroReset()
        {
            _isExecutePresentAttack = false;
            _isExecuteWait = false;
        }

        /// AI ACTIONS.
         
        public void TryCatchPlayerToExecute()
        {
            _isExecuteWait = true;
            _ai.PlayExecutionOpeningAnim();
        }

        public void MSA_TryCatchPlayerToExecute()
        {
            _isExecuteWait = true;
            _ai.Play_MSA_ExecutionOpeningAnim();
        }

        /// ANIM EVENTS.
        
        public void OnSucessfulCaughtPlayer()
        {
            ExecutePlayerWithPhase1Profiles();

            void ExecutePlayerWithPhase1Profiles()
            {
                if (Random.Range(1, 3) == 1)
                {
                    _ai.PlayExecutionPresentAnim(_executionProfile_1._presentAnimState.animStateHash);

                    _executionProfile_1._executionParentPoint = _execution1_ParentPoint;
                    _executionProfile_1._executionerAI = _ai;

                    _ai.playerStates.OnExecutionHit(_executionProfile_1);
                }
                else
                {
                    _ai.PlayExecutionPresentAnim(_executionProfile_2._presentAnimState.animStateHash);

                    _executionProfile_2._executionParentPoint = _execution2_ParentPoint;
                    _executionProfile_2._executionerAI = _ai;

                    _ai.playerStates.OnExecutionHit(_executionProfile_2);
                }
            }
        }

        /// SET STATUS.
        public void OnExecutePresentAttack()
        {
            _isExecutePresentAttack = true;
        }

        public void OffExecutePresentAttack()
        {
            _isExecutePresentAttack = false;
        }
        
        void OffIsExecuteWait()
        {
            _isExecuteWait = false;
            RandomizeExecutionWaitTime();
        }

        /// RANDOMIZE.
        
        void RandomizeExecutionWaitTime()
        {
            _finalizedExecutionWaitTime = Random.Range(executionWaitMinTime, executionWaitMaxTime);
        }
    }
}