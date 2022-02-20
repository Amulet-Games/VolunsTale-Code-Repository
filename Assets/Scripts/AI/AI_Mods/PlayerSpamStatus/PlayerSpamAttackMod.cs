using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class PlayerSpamAttackMod : AIMod
    {
        [HideInInspector]
        public bool showPlayerSpamAttackMod;

        // Attacking
        [SerializeField]
        [Tooltip("The amount of time that passed will count as player spamming attacks.")]
        private float markAsSpammedAttackingRate = 5f;

        [SerializeField]
        [Tooltip("The maximum amount of time \"playerAttackingCounter\" can store.")]
        private float maxAttackingCounterAmount = 7.5f;

        [SerializeField]
        [Tooltip("The value controls how fast \"playerAttackingCounter\" can drop back to 0.")]
        private float attackingCounterDelepteRate = 1.5f;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("A timer that start counting the moment player used attacking, counter goes down once player stop attacking.")]
        private float playerAttackingCounter = 0;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("A bool that will turn true if player were attacking over \"markAsSpammedAttackingRate\" of time. ")]
        private bool hasSpammedAttacking = false;

        [NonSerialized]
        public float _delta;
        [NonSerialized]
        public StateManager _playerStates;

        /// INIT

        public void PlayerSpamAttackModInit(StateManager _states)
        {
            _playerStates = _states;
        }

        /// TICK

        public void UpdateSpamAttackCounter()
        {
            if (_playerStates._isAttacking)
            {
                if (playerAttackingCounter < maxAttackingCounterAmount)
                {
                    playerAttackingCounter += _delta;
                }
                else
                {
                    playerAttackingCounter = maxAttackingCounterAmount;
                }
            }
            else
            {
                if (playerAttackingCounter > 0)
                {
                    playerAttackingCounter -= _delta * attackingCounterDelepteRate;
                }
                else
                {
                    playerAttackingCounter = 0;
                    hasSpammedAttacking = false;
                }
            }

            if (playerAttackingCounter > markAsSpammedAttackingRate)
            {
                hasSpammedAttacking = true;
            }
        }
        
        /// INIT

        public void PlayerSpamAttackExitAggroReset()
        {
            // Attacking
            playerAttackingCounter = 0;
            hasSpammedAttacking = false;
        }

        /// AI ACTION METHODS
        
        public void ResetSpammedAttackingStatus()
        {
            playerAttackingCounter = 0;
            hasSpammedAttacking = false;
        }

        /// GET METHODS
        
        public bool GetHasSpammedAttackingBool()
        {
            return hasSpammedAttacking;
        }
    }
}