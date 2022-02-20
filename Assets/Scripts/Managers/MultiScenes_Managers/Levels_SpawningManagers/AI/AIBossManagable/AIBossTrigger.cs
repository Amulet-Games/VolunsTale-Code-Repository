using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AIBossTrigger : MonoBehaviour
    {
        [Header("Refs.")]
        [ReadOnlyInspector] public AIBossManagable _aiBossManagable;
        [ReadOnlyInspector] public int _playerLayer;

        public void Setup(AIBossManagable _aiBossManagable)
        {
            this._aiBossManagable = _aiBossManagable;

            _playerLayer = _aiBossManagable._aiSessionManager._playerState.gameObject.layer;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _playerLayer)
            {
                _aiBossManagable.TriggerBossSequence();
                gameObject.SetActive(false);
            }
        }
    }
}