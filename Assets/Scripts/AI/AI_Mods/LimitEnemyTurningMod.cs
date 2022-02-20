using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class LimitEnemyTurningMod : AIMod
    {
        [HideInInspector]
        public bool showLimitEnemyTurningMod;

        public int _neglectIdleAnimTurningThershold = 2;
        [ReadOnlyInspector] public int _currentIdleAnimTurningCount;

        public float _countingIdleAnimTurnRate = 2;
        [ReadOnlyInspector] public float _countingIdleAnimTurnTimer;
        [ReadOnlyInspector] public bool _isCountingIdleAnimTurn;

        public float _neglectingIdleAnimTurningRate = 1;
        [ReadOnlyInspector] public bool _isNeglectingIdleAnimTurning;

        [NonSerialized] public float _delta;

        /// TICK
        
        public void TrackIdleAnimTurning()
        {
            if (_isCountingIdleAnimTurn)
            {
                _countingIdleAnimTurnTimer += _delta;
                if (_currentIdleAnimTurningCount == _neglectIdleAnimTurningThershold)
                {
                    _countingIdleAnimTurnTimer = 0;
                    _currentIdleAnimTurningCount = 0;

                    _isCountingIdleAnimTurn = false;
                    _isNeglectingIdleAnimTurning = true;
                }
                else if (_countingIdleAnimTurnTimer >= _countingIdleAnimTurnRate)
                {
                    _countingIdleAnimTurnTimer = 0;
                    _currentIdleAnimTurningCount = 0;

                    _isCountingIdleAnimTurn = false;
                }
            }
            else if (_isNeglectingIdleAnimTurning)
            {
                _countingIdleAnimTurnTimer += _delta;
                if (_countingIdleAnimTurnTimer >= _neglectingIdleAnimTurningRate)
                {
                    _countingIdleAnimTurnTimer = 0;
                    _isNeglectingIdleAnimTurning = false;
                }
            }
        }

        /// INIT
        
        public void LimitEnemyTurningModGoesAggroReset()
        {
            _currentIdleAnimTurningCount = 0;
            _isCountingIdleAnimTurn = false;

            _countingIdleAnimTurnTimer = 0;

            _isNeglectingIdleAnimTurning = false;
        }

        /// SET METHODS

        public void SetIsCountingAnimTurningToTrue()
        {
            if (!_isCountingIdleAnimTurn)
            {
                _isCountingIdleAnimTurn = true;
                _currentIdleAnimTurningCount++;
            }
            else
            {
                _currentIdleAnimTurningCount++;
            }
        }
    }
}