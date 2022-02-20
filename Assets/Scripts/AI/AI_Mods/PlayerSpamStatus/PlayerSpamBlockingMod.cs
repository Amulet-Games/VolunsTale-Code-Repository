using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class PlayerSpamBlockingMod : AIMod
    {
        [HideInInspector]
        public bool showPlayerSpamBlockingMod;

        // Blocking
        [SerializeField]
        [Tooltip("The amount of time that passed will count as player spamming blocking.")]
        private float markAsSpammedBlockingRate = 5f;

        [SerializeField]
        [Tooltip("The maximum amount of time \"playerBlockingCounter\" can store.")]
        private float maxBlockingCounterAmount = 7.5f;

        [SerializeField]
        [Tooltip("The value controls how fast \"playerBlockingCounter\" can drop back to 0.")]
        private float blockingCounterDelepteRate = 1.5f;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("A timer that start counting the moment player used blocking, counter goes down once player stop blocking.")]
        private float playerBlockingCounter = 0;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("A bool that will turn true if player were blocking over \"markAsSpammedBlockingRate\" of time. ")]
        private bool hasSpammedBlocking = false;
        
        [NonSerialized]
        public float _delta;
        [NonSerialized]
        public StateManager _playerStates;

        /// INIT
        
        public void ObservePlayerModInit(StateManager _states)
        {
            _playerStates = _states;
        }

        /// TICK
        
        public void UpdateSpammingCounter()
        {
            if (_playerStates._isBlocking)
            {
                if (playerBlockingCounter < maxBlockingCounterAmount)
                {
                    playerBlockingCounter += _delta;
                }
                else
                {
                    playerBlockingCounter = maxBlockingCounterAmount;
                }
            }
            else
            {
                if (playerBlockingCounter > 0)
                {
                    playerBlockingCounter -= _delta * blockingCounterDelepteRate;
                }
                else
                {
                    playerBlockingCounter = 0;
                    hasSpammedBlocking = false;
                }
            }

            if (playerBlockingCounter >= markAsSpammedBlockingRate)
            {
                hasSpammedBlocking = true;
            }
        }

        /// INIT
        
        public void PlayerSpamBlockingExitAggroReset()
        {
            // Blocking
            playerBlockingCounter = 0;
            hasSpammedBlocking = false;
        }

        /// AI ACTION METHODS

        public void ResetSpammedBlockingStatus()
        {
            playerBlockingCounter = 0;
            hasSpammedBlocking = false;
        }
        
        /// GET METHODS
        
        public bool GetHasSpammedBlockingBool()
        {
            return hasSpammedBlocking;
        }
    }
}