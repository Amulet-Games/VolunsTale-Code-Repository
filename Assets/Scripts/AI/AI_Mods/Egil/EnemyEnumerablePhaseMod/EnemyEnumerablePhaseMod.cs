using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EnemyEnumerablePhaseMod : AIMod
    {
        [HideInInspector] public bool showEnemyEnumerablePhaseMod;

        /// Second Phase Config.
        [Range(0, 1)] public float _phaseChangeRatio = 0.6f;
        public AnimStateVariable _phaseChangeAnim;
        public AI_ActionHolder _nextPhaseActionHolder;
        public int _nextPhaseCrossFadeLayer;
        public NextPhaseIndex _nextPhaseIndex;

        /// AI Status Enumberable Phase Datas.
        public EnumerablePhaseData[] _aiStatus_EP_Datas;

        /// Mod Enumerable Phase Datas.
        public EnumerablePhaseData[] _mods_EP_Datas;
        public EP_AI_PassiveAction _EP_PassiveAction_1;
        
        /// Status.
        [ReadOnlyInspector] public bool _isInNewPhase;

        #region Non Serialized.
        [NonSerialized] public AIManager _ai;
        #endregion

        #region Init.
        public void EnemyEnumerablePhaseModInit(AIManager _ai)
        {
            this._ai = _ai;

            if (_EP_PassiveAction_1 != null)
                _EP_PassiveAction_1.Init(_ai);
        }

        public void EnemyEnumerablePhaseGoesAggroReset()
        {
            _isInNewPhase = false;
        }
        #endregion

        #region On Hit.
        public void OnHitCheckIsPhaseChangeReady()
        {
            if (!_isInNewPhase && _ai.currentEnemyHealth < _ai.totalEnemyHealth * _phaseChangeRatio)
            {
                OnChangePhase();
            }
        }
        #endregion

        #region On Change Phase.
        void OnChangePhase()
        {
            OnPhaseChangeSetStatus();
            OnPhaseChangeAIStatus();
            OnPhaseChangeModStatus();

            EnemyChangePhaseByIndex();
        }

        void OnPhaseChangeSetStatus()
        {
            _isInNewPhase = true;
            SetPhaseChangedAnimParaByIndex();
            _ai.currentCrossFadeLayer = _nextPhaseCrossFadeLayer;
            _ai.currentActionHolder = _nextPhaseActionHolder;
        }

        void SetPhaseChangedAnimParaByIndex()
        {
            switch (_nextPhaseIndex)
            {
                case NextPhaseIndex.Phase_2:
                    _ai.ChangeTo2ndPhaseSetPara();
                    break;
                case NextPhaseIndex.Phase_3:
                    _ai.ChangeTo3rdPhaseSetPara();
                    break;
                case NextPhaseIndex.Phase_4:
                    _ai.ChangeTo4thPhaseSetPara();
                    break;
            }
        }

        void OnPhaseChangeAIStatus()
        {
            for (int i = 0; i < _aiStatus_EP_Datas.Length; i++)
            {
                _aiStatus_EP_Datas[i].SetNewPhaseData(_ai);
            }
        }

        void OnPhaseChangeModStatus()
        {
            for (int i = 0; i < _mods_EP_Datas.Length; i++)
            {
                _mods_EP_Datas[i].SetNewPhaseData(_ai);
            }
        }

        void EnemyChangePhaseByIndex()
        {
            switch (_nextPhaseIndex)
            {
                case NextPhaseIndex.Phase_2:
                    _ai.EnemyChangeTo2ndPhase();
                    break;
                case NextPhaseIndex.Phase_3:
                    _ai.EnemyChangeTo3rdPhase();
                    break;
                case NextPhaseIndex.Phase_4:
                    _ai.EnemyChangeTo4thPhase();
                    break;
            }
        }
        #endregion
        
        #region EP AI Passive Actions.
        public void Execute_EP_AI_PassiveAction_1()
        {
            _EP_PassiveAction_1.Execute(_ai);
        }
        #endregion

        public enum NextPhaseIndex
        {
            Phase_2,
            Phase_3,
            Phase_4
        }
    }
}