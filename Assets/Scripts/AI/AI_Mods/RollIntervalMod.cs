using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class RollIntervalMod : AIMod
    {
        [HideInInspector]
        public bool showRollIntervalMod;
        
        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy will need to wait before execute another roll action.")]
        private float stdRollIntervalRate = 0;

        [SerializeField]
        [Tooltip("If this is true, \"stdRollIntervalRate\" will change each time after roll action is performed.")]
        private bool isRandomized = false;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdRollIntervalRate\".")]
        private float rollIntervalRandomizeAmount = 0;

        [SerializeField]
        [Tooltip("Set \"enemyRolled\" to true when enemy goes aggro, prevent enemy do rolling whenever they first seen player.")]
        private bool checkEnemyRolledInit = false;

        [ReadOnlyInspector]
        [Tooltip("True when enemy execute roll action, after \"finalizedRollIntervalRate\" of time passed this will switch back to false.")]
        public bool enemyRolled = false;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will need to wait to execute roll action again.")]
        public float finalizedRollIntervalTime = 0;

        [NonSerialized]
        public float _delta;

        /// TICK

        public void RollIntervalTimeCount()
        {
            if (enemyRolled)
            {
                finalizedRollIntervalTime -= _delta;
                if (finalizedRollIntervalTime <= 0)
                {
                    finalizedRollIntervalTime = 0;
                    enemyRolled = false;
                }
            }
        }

        /// INIT
        
        public void RollIntervalGoesAggroReset()
        {
            if (checkEnemyRolledInit)
                enemyRolled = true;

            RandomizeRollRate();
        }

        public void RollIntervalExitAggroReset()
        {
            enemyRolled = false;
        }

        /// SET METHODS

        public void SetEnemyRolledBoolToTrue()
        {
            enemyRolled = true;
            RandomizeRollRate();
        }

        /// 2nd PHASE CHANGE.

        public void SetNewPhaseData(RollIntervalMod_EP_Data _rollInterval_ep_data)
        {
            stdRollIntervalRate = _rollInterval_ep_data._nextPhaseStdRollIntervalRate;
            rollIntervalRandomizeAmount = _rollInterval_ep_data._nextPhaseRollIntervalRandomizeAmount;
        }

        void RandomizeRollRate()
        {
            if (isRandomized)
            {
                RandomizeWithAddonValue(rollIntervalRandomizeAmount, stdRollIntervalRate, ref finalizedRollIntervalTime);
            }
            else
            {
                finalizedRollIntervalTime = stdRollIntervalRate;
            }
        }
    }
}