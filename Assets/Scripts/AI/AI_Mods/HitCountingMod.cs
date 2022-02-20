using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class HitCountingMod : AIMod
    {
        [HideInInspector]
        public bool showHitCountingMod;

        [SerializeField]
        [Tooltip("How fast the hit counter will cancel 1 hit count.")]
        private float cancelHitRate = 0;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("Timer that counting when will be the next hit count that would be canceled.")]
        private float countHitTimer;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("If this value exceeded to certain number, enemy will perform hit count triggered action on next AI Action.")]
        private int hitCount;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("This will be true when hit Count value exceeded to certain number, once hit count triggered action executed this will turn to false.")]
        private bool isHitCountEventTriggered;

        /// TICK

        public void MonitorHitCounting(float delta)
        {
            if (hitCount > 0)
            {
                countHitTimer += delta;
                if (countHitTimer >= cancelHitRate)
                {
                    countHitTimer = 0;
                    hitCount--;
                }
            }

            if (hitCount >= 2)
                isHitCountEventTriggered = true;
        }

        /// INIT

        public void HitCountingExitAggroReset()
        {
            hitCount = 0;
            countHitTimer = 0;
            isHitCountEventTriggered = false;
        }

        public void ResetHitCountingStates()
        {
            hitCount = 0;
            countHitTimer = 0;
            isHitCountEventTriggered = false;
        }

        /// GET METHODS

        public bool GetIsHitCountEventTriggeredBool()
        {
            return isHitCountEventTriggered;
        }

        /// AI MANAGER - DealDamageToEnemy
        
        public void IncreaseHitCount(int increaseAmount)
        {
            hitCount += increaseAmount;
        }
    }
}