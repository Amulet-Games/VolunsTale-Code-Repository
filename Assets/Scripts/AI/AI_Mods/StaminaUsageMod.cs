using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class StaminaUsageMod : AIMod
    {
        [HideInInspector]
        public bool showStaminaUsageMod;

        [Header("Anim_Hash")]
        private int e_mod_IsTired_hash = 0;
        
        [SerializeField]
        [Tooltip("How fast enemy recover his stamina.")]
        private float staminaReturnSpeed = 0f;

        [SerializeField]
        [Tooltip("The amount stamina without randomized that this enemy has.")]
        private float stdStaminaAmount = 0f;
        
        [SerializeField]
        [Tooltip("The amount of stamina to cut or add on \"stdStaminaAmount\".")]
        private float staminaRandomizeAmount = 0f;

        [ReadOnlyInspector]
        [Tooltip("If this is true, enemy will executed action that is specific to \"IsTired\" return score. ")]
        public bool isEnemyTired = false;
        
        [SerializeField, ReadOnlyInspector]
        [Tooltip("The current amount of enemy stamina in the moment.")]
        private float currentEnemyStamina = 0f;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of stamina that this enemy has.")]
        private float finalizedStaminaAmount = 0f;

        [NonSerialized]
        public float _delta;
        private Animator _anim;

        /// TICK

        public void MonitorEnemyStamina()
        {
            if (currentEnemyStamina >= finalizedStaminaAmount)
            {
                currentEnemyStamina = finalizedStaminaAmount;
                SetIsEnemyTiredBool(false);
            }

            if (currentEnemyStamina <= finalizedStaminaAmount)
            {
                currentEnemyStamina += _delta * staminaReturnSpeed;
            }

            if (currentEnemyStamina < 0)
                currentEnemyStamina = 0;
        }

        /// INIT

        public void StaminaModInit(AIManager ai)
        {
            _anim = ai.anim;
            e_mod_IsTired_hash = ai.hashManager.e_mod_IsTired_hash;
        }

        public void StaminaUsageGoesAggroReset()
        {
            RandomizeWithAddonValue(staminaRandomizeAmount, stdStaminaAmount, ref finalizedStaminaAmount);
            currentEnemyStamina = finalizedStaminaAmount;
        }

        public void StaminaUsageExitAggroReset()
        {
            SetIsEnemyTiredBool(false);
        }

        /// SET METHODS

        void SetIsEnemyTiredBool(bool isEnemyTired)
        {
            this.isEnemyTired = isEnemyTired;
            _anim.SetBool(e_mod_IsTired_hash, isEnemyTired);
        }

        /// AI ACTION METHODS

        public void DepleteEnemyStamina(float staminaUsage)
        {
            currentEnemyStamina -= staminaUsage;
            if (currentEnemyStamina <= 0)
            {
                SetIsEnemyTiredBool(true);

                RandomizeWithAddonValue(staminaRandomizeAmount, stdStaminaAmount, ref finalizedStaminaAmount);
            }
        }

        public void RefillEnemyStamina()
        {
            RandomizeWithAddonValue(staminaRandomizeAmount, stdStaminaAmount, ref finalizedStaminaAmount);
            currentEnemyStamina = finalizedStaminaAmount;

            SetIsEnemyTiredBool(false);
        }
    }
}