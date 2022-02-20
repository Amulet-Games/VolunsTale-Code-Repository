using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SA
{
    [Serializable]
    public class ParryPlayerMod : AIMod
    {
        [HideInInspector]
        public bool showParryPlayerMod;

        [Header("Anim_Hash")]
        [HideInInspector]
        public int e_mod_isWaitingToParry_hash;

        [SerializeField]
        [Tooltip("The maximum amount of time enemy will need to wait before he can parry again.")]
        private float maxParryCooldownRate = 10;
        [SerializeField]
        [Tooltip("The minimum amount of time enemy will need to wait before he can parry again.")]
        private float minParryCooldownRate = 5f;

        [SerializeField]
        [Tooltip("The maximum amount of time enemy will wait for player attacks.")]
        private float maxParryWaitTime = 3f;
        [SerializeField]
        [Tooltip("The minimum amount of time enemy will wait for player attacks.")]
        private float minParryWaitTime = 1f;

        //[Tooltip("The FX Particle that will spawn when player hit the enemy while he is parrying.")]
        //public MetalSlash_S_ImpactEffectPool metalImpactSmallPool = null;

        [Tooltip("When parry enemy will disable inplace turing animation, so turning will be determine by upper body rotate max speed.")]
        public float maxUpperBodyIKParryTurnSpeed = 3;

        public ParryAttackReady rSParryAttack1ReadyAction;
        public ParryAttackReady lSParryAttack1ReadyAction;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will need to wait before he can parry again.")]
        private float finalizedParryCooldownRate;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will wait for player attacks.")]
        private float finalizedParryWaitTime = 0;

        [ReadOnlyInspector]
        [Tooltip("Turns true when enemy executed parry player AI Action, note that it doesn't nessary need to be a sucessful parry or parry ran out to mark this bool to true.")]
        public bool isInParryCooldown = false;

        [ReadOnlyInspector]
        [Tooltip("Is enemy currently in the state of waiting player attacks to parry.")]
        public bool isParryWaiting = false;

        [ReadOnlyInspector]
        public bool isPlayParryFxNeeded;
        [ReadOnlyInspector]
        public bool isPlayerAttackBaited;

        [NonSerialized] AIManager _ai;
        [NonSerialized] public float _delta;

        /// INIT

        public void ParryModInit(AIManager ai)
        {
            _ai = ai;
            e_mod_isWaitingToParry_hash = _ai.hashManager.e_mod_isWaitingToParry_hash;
        }

        public void ParryPlayerGoesAggroReset()
        {
            isInParryCooldown = true;

            RandomizeParryCooldownRate();
            RandomizeParryWaitTime();
        }

        public void ParryPlayerExitAggroReset()
        {
            isParryWaiting = false;
            isInParryCooldown = false;
            isPlayParryFxNeeded = false;
            isPlayerAttackBaited = false;

            _ai.anim.SetBool(e_mod_isWaitingToParry_hash, false);
        }

        /// TICK

        public void MonitorParryPlayerBools()
        {
            /* If enemy is currently waiting for player's attacks,
             * then decreases it's parryable time, also check if no more time left,
             * quit the waiting and allows ai manager to find a new ai action. 
             */
            if (isParryWaiting && !isPlayerAttackBaited)
            {
                finalizedParryWaitTime -= _delta;
                if (finalizedParryWaitTime <= 0)
                {
                    isParryWaiting = false;
                    finalizedParryWaitTime = 0;

                    OffWaitingToParry_TimesOut();
                }
            }

            /* If enemy is currently waiting to execute parry ai action again,
             * decreases it's wait time and if waiting is over reset updateLockOnPos bool,
             * so that enemy can update the position immediately in parry ai action.
             */
            if (isInParryCooldown)
            {
                finalizedParryCooldownRate -= _delta;
                if (finalizedParryCooldownRate <= 0)
                {
                    finalizedParryCooldownRate = 0;
                    OffParryCoolDown();
                }
            }
        }

        /// SET

        public void SetIsWaitingToParryBool(bool _isParryWaiting)
        {
            if (!_isParryWaiting)
            {
                if (isParryWaiting)
                {
                    OffWaitingToParry_Parryable();
                }
            }
            else
            {
                if (!isParryWaiting)
                {
                    isParryWaiting = true;
                    _ai.OnWaitingToParry();
                }
            }
        }

        void OffWaitingToParry_TimesOut()
        {
            OffParryWaiting();
            _ai.OffWaitingToParry_TimesOut();
        }

        public void OffWaitingToParry_Parryable()
        {
            isParryWaiting = false;
            isPlayerAttackBaited = false;
            finalizedParryWaitTime = 0;

            OffParryWaiting();
            _ai.OffWaitingToParry_Parryable();
        }

        public void OffWaitingToParry_HitByChargeAttack()
        {
            isParryWaiting = false;
            isPlayerAttackBaited = false;
            finalizedParryWaitTime = 0;

            OffParryWaiting();
            _ai.OffWaitingToParry_HitByChargeAttack();
        }

        void OffParryWaiting()
        {
            isInParryCooldown = true;
            RandomizeParryCooldownRate();
        }

        void OffParryCoolDown()
        {
            isInParryCooldown = false;
            RandomizeParryWaitTime();
        }

        /// AI MANAGER - On Hit

        public void OnHitAIMods()
        {
            if (isParryWaiting)
            {
                if (_ai._isHitByChargeAttack)
                {
                    HitByChargeAttack_CancelParryWaiting();
                }
                else
                {
                    HitByNormalAttacks_ProceedToParryPlayer();
                }
            }

            void HitByChargeAttack_CancelParryWaiting()
            {
                OffWaitingToParry_HitByChargeAttack();
            }

            void HitByNormalAttacks_ProceedToParryPlayer()
            {
                if (_ai.GetIsRightStanceBool())
                {
                    rSParryAttack1ReadyAction.Execute(_ai);
                }
                else
                {
                    lSParryAttack1ReadyAction.Execute(_ai);
                }

                isPlayParryFxNeeded = true;
                isPlayerAttackBaited = true;
            }
        }

        public void SpawnOnHitEffect()
        {
            if (isPlayParryFxNeeded)
            {
                isPlayParryFxNeeded = false;

                WorldImpactEffect _effect = GameManager.singleton._parryImpactEffect;

                _effect.mTransform.parent = null;
                _effect.mTransform.position = _ai.currentWeapon.transform.position;
                _effect.mTransform.eulerAngles = _ai.mTransform.eulerAngles;

                _effect.AI_ParryHit_SpawnEffect();
            }
            else
            {
                _ai.Spawn_Regular_BloodFx();
            }
        }

        /// RANDOMIZE.

        public void RandomizeParryWaitTime()
        {
            finalizedParryWaitTime = Random.Range(minParryWaitTime, maxParryWaitTime);
        }

        public void RandomizeParryCooldownRate()
        {
            finalizedParryCooldownRate = Random.Range(minParryCooldownRate, maxParryCooldownRate);
        }
    }
}