using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EnemyTauntMod : AIMod
    {
        [HideInInspector]
        public bool showEnemyTauntMod;
        
        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy will need to wait before taunting player again.")]
        private float stdTauntRate = 0;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdTauntRate\".")]
        private float tauntRateRandomizeAmount = 0;

        [SerializeField]
        [Tooltip("Set \"tauntedPlayer\" to true when enemy goes aggro, prevent enemy taunt player whenever they first seen player.")]
        private bool checkTauntedPlayerInit = false;
        
        [ReadOnlyInspector]
        [Tooltip("Has Enemy taunted player?")]
        public bool tauntedPlayer = false;
        
        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will need to wait before he can taunt again")]
        private float finalizedTauntTime = 0;

        [NonSerialized]
        public float _delta;

        /// TICK
        
        public void EnemyTauntTimeCount()
        {
            if (tauntedPlayer)
            {
                finalizedTauntTime -= _delta;
                if (finalizedTauntTime <= 0)
                {
                    finalizedTauntTime = 0;
                    tauntedPlayer = false;
                }
            }
        }

        /// INIT
        public void EnemyTauntGoesAggroReset()
        {
            if (checkTauntedPlayerInit)
                tauntedPlayer = true;

            RandomizeWithAddonValue(tauntRateRandomizeAmount, stdTauntRate, ref finalizedTauntTime);
        }


        public void EnemyTauntExitAggroReset()
        {
            tauntedPlayer = false;
        }

        /// SET METHODS

        public void SetTauntedPlayerToTrue()
        {
            tauntedPlayer = true;
            RandomizeWithAddonValue(tauntRateRandomizeAmount, stdTauntRate, ref finalizedTauntTime);
        }
    }
}