using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class PerilousAttackMod : AIMod
    {
        [HideInInspector]
        public bool showPerilousAttackMod;
        
        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy will need to wait before execute another perilous attack.")]
        private float stdPerilousAttackRate = 0;

        [SerializeField]
        [Tooltip("If this is true, \"stdPerilousAttackRate\" will change each time after perilous attack is executed.")]
        private bool isRandomized = false;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdPerilousAttackRate\".")]
        private float perilousAttackRandomizeAmount = 0;
        
        [ReadOnlyInspector]
        [Tooltip("True when enemy used a perilous attack, after \"finalizedPerilousAttackRate\" of time passed this will switch back to false.")]
        public bool usedPerilousAttack = false;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will need to wait to peform perilous attack again.")]
        public float finalizedPerilousAttackTime = 0;

        [Tooltip("The Indicator FX that will play when perilous attack happened.")]
        public GameObject perilousATKIndicator;

        [NonSerialized]
        public float _delta;

        /// TICK

        public void PerilousAttackTimeCount()
        {
            if (usedPerilousAttack)
            {
                finalizedPerilousAttackTime -= _delta;
                if (finalizedPerilousAttackTime <= 0)
                {
                    finalizedPerilousAttackTime = 0;
                    usedPerilousAttack = false;
                }
            }
        }

        /// INIT

        public void PerilousAttackGoesAggroReset()
        {
            usedPerilousAttack = true;
            RandomizePerilousAttackRate();
        }

        public void PerilousAttackExitAggroReset()
        {
            usedPerilousAttack = false;
        }

        /// SET METHODS

        public void SetUsedPerilousAttackToTrue()
        {
            usedPerilousAttack = true;
            RandomizePerilousAttackRate();
        }

        void RandomizePerilousAttackRate()
        {
            if (isRandomized)
            {
                RandomizeWithAddonValue(perilousAttackRandomizeAmount, stdPerilousAttackRate, ref finalizedPerilousAttackTime);
            }
            else
            {
                finalizedPerilousAttackTime = stdPerilousAttackRate;
            }
        }

        /// 2nd PHASE CHANGE.
        
        public void SetNewPhaseData(PerliousAttackMod_EP_Data _perliousAttackMod_EP_Data)
        {
            stdPerilousAttackRate = _perliousAttackMod_EP_Data._nextPhasePerliousAttackRate;
            perilousAttackRandomizeAmount = _perliousAttackMod_EP_Data._nextPhasePerliousAttackRandomizedRate;
        }
    }
}