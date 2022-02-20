using System;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class KnockDownPlayerMod : AIMod
    {
        [HideInInspector]
        public bool showKnockDownPlayerMod;

        public float knockDownWaitRate = 1;

        [ReadOnlyInspector] public float _knockDownWaitTimer;
        [ReadOnlyInspector] public bool _isKnockDownPlayerWait;

        #region Non Serialized.
        public float _delta;
        #endregion

        /// TICK.

        public void KnockedDownPlayerTimeCount()
        {
            if (_isKnockDownPlayerWait)
            {
                _knockDownWaitTimer += _delta;
                if (_knockDownWaitTimer >= knockDownWaitRate)
                {
                    _knockDownWaitTimer = 0;
                    SetIsKnockedDownPlayerToFalse();
                }
            }
        }

        /// INIT.
         
        public void KnockedDownPlayerExitAggroReset()
        {
            _isKnockDownPlayerWait = false;
            _knockDownWaitTimer = 0;
        }

        /// ANIM EVENTS.

        public void SetIsKnockedDownPlayerToTrue()
        {
            _isKnockDownPlayerWait = true;
        }

        void SetIsKnockedDownPlayerToFalse()
        {
            _isKnockDownPlayerWait = false;
        }
    }
}