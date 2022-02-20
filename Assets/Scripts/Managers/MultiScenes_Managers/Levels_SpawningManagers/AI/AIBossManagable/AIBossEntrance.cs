using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AIBossEntrance : MonoBehaviour
    {
        [Header("Refs.")]
        [ReadOnlyInspector] public AIGroupManagable _aiGroupManagable;
        [ReadOnlyInspector] public StateManager _playerStates;

        public void Setup(AIGroupManagable _aiGroupManagable)
        {
            this._aiGroupManagable = _aiGroupManagable;

            _playerStates = _aiGroupManagable._aiSessionManager._playerState;
        }

        /// Check if player world position x is bigger than this entrance value.
        /// If it's true then means player has enter the boss zone.
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == _playerStates.gameObject.layer)
            {
                if (_playerStates.mTransform.position.x > transform.position.x)
                {
                    Debug.Log("Has Enter Boss Zone!");
                    _aiGroupManagable.ForcedAggrosReturnToPatrol();
                }
                else
                {
                    Debug.Log("Has Exit Boss Zone!");
                    _aiGroupManagable.AllowedToTurnAggroAgain();
                }
            }
        }
    }
}