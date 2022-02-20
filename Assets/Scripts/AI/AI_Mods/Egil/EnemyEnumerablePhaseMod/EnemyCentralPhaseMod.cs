using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EnemyCentralPhaseMod : AIMod
    {
        [HideInInspector] public bool showEnemyCentralPhaseMod;

        /// AI Status 1st Phase Datas.
        public EnumerablePhaseData[] _enemy_1P_originalDatas;

        /// Mod 1st Phase Datas.
        public EnumerablePhaseData[] _mods_1P_originalDatas;
        
        /// Status.
        [ReadOnlyInspector] public bool _isChangingPhase;
        [ReadOnlyInspector] public int _currentEnemyPhaseIndex;

        [ReadOnlyInspector] public float _beforePhaseChange_animInplaceRotateThershold;
        [ReadOnlyInspector] public float _beforePhaseChange_animRootRotateThershold;

        #region Non Serialized.
        [NonSerialized] public AIManager _ai;
        #endregion

        #region Init.
        public void EnemyCentralPhaseModInit(AIManager _ai)
        {
            this._ai = _ai;
        }

        public void EnemyEnumerablePhaseGoesAggroReset()
        {
            _isChangingPhase = false;
            _currentEnemyPhaseIndex = 1;
        }
        #endregion

        #region Checkpoint Refresh.
        public void CheckpointRefresh_ReversePhaseChanges()
        {
            for (int i = 0; i < _enemy_1P_originalDatas.Length; i++)
            {
                _enemy_1P_originalDatas[i].SetNewPhaseData(_ai);
            }

            for (int i = 0; i < _mods_1P_originalDatas.Length; i++)
            {
                _mods_1P_originalDatas[i].SetNewPhaseData(_ai);
            }
        }
        #endregion

        #region Restrain Turning.
        public void PrePhaseChangedRestrainTurning()
        {
            _beforePhaseChange_animInplaceRotateThershold = _ai.animInplaceRotateThershold;
            _beforePhaseChange_animRootRotateThershold = _ai.animRootRotateThershold;

            _ai.animRootRotateThershold = 1000;
            _ai.animInplaceRotateThershold = 1000;
        }

        public void PostPhaseChangedRestoreTuring()
        {
            _ai.animInplaceRotateThershold = _beforePhaseChange_animInplaceRotateThershold;
            _ai.animRootRotateThershold = _beforePhaseChange_animRootRotateThershold;
        }
        #endregion

        #region Switch Phase.
        public void SwitchToPhase1()
        {
            _currentEnemyPhaseIndex = 1;
        }

        public void SwitchToPhase2()
        {
            _isChangingPhase = true;
            _currentEnemyPhaseIndex = 2;
        }

        public void SwitchToPhase3()
        {
            _isChangingPhase = true;
            _currentEnemyPhaseIndex = 3;
        }

        public void SwitchToPhase4()
        {
            _isChangingPhase = true;
            _currentEnemyPhaseIndex = 4;
        }
        #endregion
    }
}