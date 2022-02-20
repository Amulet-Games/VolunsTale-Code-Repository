using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EnemyEvolveMod : AIMod
    {
        [HideInInspector]
        public bool showEnemyEvolveMod;

        [Tooltip("The amount of current health left in AI will trigger evolve.")]
        [SerializeField, Range(0, 1)]
        float evolveHealthRatioThershold;

        [Tooltip("The amount of time AI will take to finish charging.")]
        [SerializeField] public float evolveChargeRate;

        [Tooltip("The amount of turning prediction needed for enemy to face player.")]
        [SerializeField] public float evolveTurningPlayerPredict;

        [Tooltip("Counting the time used for charge.")]
        [ReadOnlyInspector] public float evolveChargeTimer;

        [Tooltip("Determine if this AI met the requirement to evolve.")]
        [ReadOnlyInspector] public bool isEvolvable;

        [Tooltip("Determine if this AI is currently evolving.")]
        [ReadOnlyInspector] public bool isEvolveStarted;

        [Tooltip("Determine if this AI has already evolved.")]
        [ReadOnlyInspector] public bool hasEvolved;

        [Tooltip("The new Action Holder which AI will use after evolved.")]
        [SerializeField] AI_ActionHolder evolvedActionHolder;

        [Tooltip("The GameObjects of particle system will be activated when AI Evolved.")]
        [SerializeField] GameObject[] evolvedAppearanceEffects;

        [Tooltip("The Effect that is used when AI start charging for evolve.")]
        [SerializeField] GameObject evolveChargeEffect;

        [Tooltip("The Effect that is used when AI finish charged.")]
        public int evolveRelease_area_dp_Id;
        
        [Tooltip("The references that included all the nesscary infos to deal damage to player.")]
        [SerializeField] protected AI_AttackRefs evolveReleaseAttackRefs;

        [NonSerialized]
        public AIManager _ai;
        [NonSerialized]
        public float _delta;
        [NonSerialized]
        int e_evolve_start_hash;
        [NonSerialized]
        int e_evolve_end_hash;
        [NonSerialized]
        int _evolveParticlesObjectsLength;

        /// INIT
        
        public void EnemyEvolveModInit(AIManager _ai)
        {
            this._ai = _ai;

            e_evolve_start_hash = _ai.hashManager.e_evolve_start_hash;
            e_evolve_end_hash = _ai.hashManager.e_evolve_end_hash;

            _evolveParticlesObjectsLength = evolvedAppearanceEffects.Length;
        }
        
        public void EnemyEvolveExitAggroReset()
        {
            evolveChargeTimer = 0;
            isEvolveStarted = false;
            isEvolvable = false;
        }

        public void HandleActionHolderChangeIfEvolve()
        {
            if (hasEvolved)
                _ai.currentActionHolder = evolvedActionHolder;
            else
                _ai.currentActionHolder = _ai.firstWeaponActionHolder;
        }

        /// Is Interacting Tick.
        
        public void UpdateEvolveChargeTimer()
        {
            if (isEvolveStarted)
            {
                evolveChargeTimer += _delta;
                if (evolveChargeTimer >= evolveChargeRate)
                {
                    SetIsEvolveStartedStatus(false);
                    EvolveEnemy();
                }
            }
        }

        /// On Hit.

        public void CheckIsEvolvable()
        {
            if ((_ai.currentEnemyHealth <= _ai.totalEnemyHealth * evolveHealthRatioThershold) && !hasEvolved)
            {
                isEvolvable = true;
            }
        }

        #region Start Evolve Enemy.
        public void SetIsEvolveStartedStatus(bool _isEvolveStarted)
        {
            if (_isEvolveStarted)
            {
                isEvolveStarted = true;
                OnEvolveStartChangeStats();
                OnEvolveChargedEffect();
                PlayEvolveChargeStartedAnim();
            }
            else
            {
                isEvolveStarted = false;
                _ai.Get_Set_CurrentDamageParticle(evolveRelease_area_dp_Id);
                OffEvolveChargedEffect();
            }
        }
        
        void OnEvolveStartChangeStats()
        {
            isEvolvable = false;
            _ai.isPausingTurnWithAgent = true;
            _ai.isTrackingPlayer = true;
            _ai._isSkippingOnHitAnim = true;
            _ai.currentPlayerPredictOffset = evolveTurningPlayerPredict;
            _ai.currentAttackRefs = evolveReleaseAttackRefs;
        }

        void PlayEvolveChargeStartedAnim()
        {
            _ai.PlayAnimationCrossFade(e_evolve_start_hash, 0.2f, true);
        }
        #endregion

        #region Evolve Enemy.
        public void EvolveEnemy()
        {
            PlayEvolveReleaseAnim();
            ChangeToEnemyEvolvedStats();
            ChangeToEnemyEvolvedAppearance();
            ChangeEnemyWeaponAppearance();
            SetBoolsAfterEnemyEvolved();

            void PlayEvolveReleaseAnim()
            {
                _ai.PlayAnimationCrossFade(e_evolve_end_hash, 0.2f, true);
            }

            void ChangeToEnemyEvolvedStats()
            {
                /// Change AI Manager / AI State Manager related Stats in here.
                _ai.currentActionHolder = evolvedActionHolder;
                _ai._isSkippingOnHitAnim = false;
            }

            void ChangeToEnemyEvolvedAppearance()
            {
                for (int i = 0; i < _evolveParticlesObjectsLength; i++)
                {
                    evolvedAppearanceEffects[i].SetActive(true);
                }
            }

            void ChangeEnemyWeaponAppearance()
            {
                _ai.currentWeapon.ActivateWeaponBuffEffect();
            }

            void SetBoolsAfterEnemyEvolved()
            {
                hasEvolved = true;
            }
        }
        #endregion

        #region Devolve Enemy.
        public void DevolveEnemy()
        {
            if (hasEvolved)
            {
                hasEvolved = false;

                ReturnToDevolvedStats();
                ReturnToDevolvedAppearance();
                ReturnEnemyWeaponAppearance();
                SetBoolsAfterEnemyDevolved();
                
                void ReturnToDevolvedStats()
                {
                    /// Change AI Manager / AI State Manager related Stats in here.
                    _ai.currentActionHolder = _ai.firstWeaponActionHolder;
                }

                void ReturnToDevolvedAppearance()
                {
                    for (int i = 0; i < _evolveParticlesObjectsLength; i++)
                    {
                        evolvedAppearanceEffects[i].SetActive(false);
                    }
                }

                void ReturnEnemyWeaponAppearance()
                {
                    _ai.firstWeapon.DeactivateWeaponBuffEffect();
                }

                void SetBoolsAfterEnemyDevolved()
                {
                    hasEvolved = false;
                }
            }
        }
        #endregion

        #region On / Off Effect.
        void OnEvolveChargedEffect()
        {
            evolveChargeEffect.SetActive(true);
        }

        void OffEvolveChargedEffect()
        {
            evolveChargeEffect.SetActive(false);
        }
        #endregion
    }
}