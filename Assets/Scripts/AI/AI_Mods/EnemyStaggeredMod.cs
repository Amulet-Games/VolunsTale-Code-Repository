using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class EnemyStaggeredMod : AIMod
    {
        [HideInInspector]
        public bool showEnemyStaggeredMod;

        [SerializeField]
        [Tooltip("The total stagged value of enemy in the beginning.")]
        private float totalStaggedValue = 0;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("Once this value is equal or below 0 enemy will play damage animation on the next hit he get.")]
        private float currentStaggedValue = 0;

        /// INIT

        public void EnemyStaggeredExitAggroReset()
        {
            RefillStaggeredValue();
        }

        /// GET METHODS

        public float GetCurrentStaggedValue()
        {
            return currentStaggedValue;
        }

        /// DAMAGES METHODS

        public void DepleteStaggeredValue(float depleteAmount)
        {
            currentStaggedValue -= depleteAmount;
        }

        public void RefillStaggeredValue()
        {
            currentStaggedValue = totalStaggedValue;
        }
    }
}