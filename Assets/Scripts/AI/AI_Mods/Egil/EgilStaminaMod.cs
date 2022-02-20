using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EgilStaminaMod : AIMod
    {
        [HideInInspector]
        public bool showEgilStaminaMod;
        public bool showIsTiredSection;
        public bool showIsInjuredSection;
        public bool showModStatusSection;

        #region Is Tired Section.
        [Header("Anim_Hash"), NonSerialized]
        public int e_mod_IsTired_hash = 0;

        [SerializeField]
        [Tooltip("Max speed of Egil recover his stamina.")]
        private float staminaReturnMaxSpeed = 1.3f;

        [SerializeField]
        [Tooltip("Min speed of Egil recover his stamina.")]
        private float staminaReturnMinSpeed = 1f;

        [SerializeField]
        [Tooltip("The maximum amount of times Egil can attack player within one full stamina cycle.")]
        private int maxActionAmounts = 6;

        [SerializeField]
        [Tooltip("The Least amount of times Egil can attack player within one full stamina cycle.")]
        private int leastActionAmounts = 2;

        [SerializeField]
        [Tooltip("The amount stamina will cost Egil for one attack. (All attacks needs to cost the same amount of stamina).")]
        private float staminaCostPerAction = 60f;

        [ReadOnlyInspector]
        [Tooltip("If this is true, enemy will executed action that is specific to \"IsTired\" return score. ")]
        public bool isEgilTired = false;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The current amount of enemy stamina in the moment.")]
        private float currentEgilStamina;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of stamina that this enemy has.")]
        private float finalizedStaminaAmount;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized speed of Egil recover his stamina.")]
        private float finalizedStaminaRecoverSpeed;
        #endregion

        #region Is Injured Section.
        public float injuredStaminaRecoverSpeed;

        public ParticleSystem egilInjuredLoopingFx;
        public State egilInjuredState;
        public AIPassiveAction recoveredAIPassiveAction;
        #endregion

        #region Non Serialized.
        [NonSerialized]
        public float _delta;
        private AIManager _ai;
        #endregion

        /// TICK

        public void MonitorEgilStamina()
        {
            if (isEgilTired)
            {
                currentEgilStamina += _delta * finalizedStaminaRecoverSpeed;
                if (currentEgilStamina >= finalizedStaminaAmount)
                {
                    currentEgilStamina = finalizedStaminaAmount;
                    SetIsEgilTiredBool(false);
                }
            }
        }

        public void MonitorEgilInjuredRecovery()
        {
            if (isEgilTired)
            {
                currentEgilStamina += _delta * injuredStaminaRecoverSpeed;
                if (currentEgilStamina >= finalizedStaminaAmount)
                {
                    currentEgilStamina = finalizedStaminaAmount;
                    isEgilTired = false;

                    _ai.EgilInjuryRecovered();
                }
            }
        }

        /// INIT

        public void StaminaModInit(AIManager ai)
        {
            _ai = ai;
            e_mod_IsTired_hash = ai.hashManager.e_mod_IsTired_hash;
        }

        public void StaminaUsageGoesAggroReset()
        {
            RandomizeRecoverSpeed();
            RandomizeStaminaAmount();
            currentEgilStamina = finalizedStaminaAmount;
        }

        public void StaminaUsageExitAggroReset()
        {
            SetIsEgilTiredBool(false);
            HideInjuredLoopingFxImmediately();
        }

        public void HideInjuredLoopingFxImmediately()
        {
            if (egilInjuredLoopingFx.isPlaying)
            {
                egilInjuredLoopingFx.Stop();
                egilInjuredLoopingFx.gameObject.SetActive(false);
            }
        }

        /// SET METHODS

        public void SetIsEgilTiredBool(bool isEnemyTired)
        {
            isEgilTired = isEnemyTired;
            _ai.anim.SetBool(e_mod_IsTired_hash, isEnemyTired);
        }

        /// AI ACTION METHODS

        public void DepleteEgilStamina(float staminaUsage)
        {
            currentEgilStamina -= staminaUsage;
            if (currentEgilStamina <= 0)
            {
                if (_ai.GetIsIn3rdPhaseBool())
                {
                    SetIsEgilInjuredToTrue();
                }
                else
                {
                    SetIsEgilTiredToTrue();
                }
            }

            void SetIsEgilTiredToTrue()
            {
                SetIsEgilTiredBool(true);

                RandomizeStaminaAmount();
                RandomizeRecoverSpeed();
            }

            void SetIsEgilInjuredToTrue()
            {
                SetIsEgilTiredBool(true);
                _ai.ZeroOutLocomotionValue();
            }
        }
        
        /// 2nd PHASE CHANGE.

        public void SetNewPhaseData_2ndPhase(EgilStaminaMod_2ndPhase_EP_Data _egilStaminaMod_2P_EP_Data)
        {
            staminaReturnMaxSpeed = _egilStaminaMod_2P_EP_Data._nextPhaseEgilStaminaMaxSpeed;
            staminaReturnMinSpeed = _egilStaminaMod_2P_EP_Data._nextPhaseEgilStaminaMinSpeed;
            maxActionAmounts = _egilStaminaMod_2P_EP_Data._nextPhaseEgilStaminaMaxActionAmount;
            leastActionAmounts = _egilStaminaMod_2P_EP_Data._nextPhaseEgilStaminaLeastActionAmount;

            if (isEgilTired)
            {
                SetIsEgilTiredBool(false);
            }

            RandomizeRecoverSpeed();
            RandomizeStaminaAmount();
            currentEgilStamina = finalizedStaminaAmount;
        }

        /// 3rd PHASE CHANGE.

        public void SetNewPhaseData_3rdPhase(EgilStaminaMod_3rdPhase_EP_Data _egilStaminaMod_3P_EP_Data)
        {
            maxActionAmounts = _egilStaminaMod_3P_EP_Data._lastPhaseEgilStaminaActionAmount;

            if (isEgilTired)
            {
                SetIsEgilTiredBool(false);
            }

            finalizedStaminaAmount = maxActionAmounts * staminaCostPerAction;
            currentEgilStamina = finalizedStaminaAmount;

            finalizedStaminaRecoverSpeed = injuredStaminaRecoverSpeed;
        }

        /// ON INJURED RECOVERED.

        public void OnInjuredRecoveredCounterComplete()
        {
            egilInjuredLoopingFx.Stop();
            egilInjuredLoopingFx.transform.parent = _ai.aISessionManager.transform;
            egilInjuredLoopingFx.gameObject.SetActive(false);

            recoveredAIPassiveAction.Execute(_ai);
        }

        /// Randomize Methods.
        void RandomizeStaminaAmount()
        {
            finalizedStaminaAmount = staminaCostPerAction * Random.Range(leastActionAmounts, maxActionAmounts + 1);
        }

        void RandomizeRecoverSpeed()
        {
            finalizedStaminaRecoverSpeed = Random.Range(staminaReturnMinSpeed, staminaReturnMaxSpeed);
        }
    }
}