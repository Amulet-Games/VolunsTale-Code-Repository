using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SA
{
    [Serializable]
    public class EnemyBlockingMod : AIMod
    {
        [HideInInspector]
        public bool showEnemyBlockingMod;

        /// Anim Hash
        [NonSerialized] public int e_block_react_hash = 0;
        [NonSerialized] public int e_block_break_hash = 0;

        [SerializeField]
        [Tooltip("Enemy will block right after stamina is fully recovered. You can uncheck this to use blocking as AIAction")]
        private bool isBlockingWithoutAction = true;

        [SerializeField]
        [Tooltip("Set \"enemyBlocked\" to true when enemy goes aggro, prevent enemy do blocking whenever they first seen player.")]
        private bool checkEnemyBlockedInit = false;

        [SerializeField]
        [Tooltip("The amount of blocking stamina without randomized.")]
        private float stdBlockingStaminaAmount = 0;

        [SerializeField]
        [Tooltip("The amount of stamina to cut or add on \"stdBlockingStaminaAmount\".")]
        private float blockingStaminaRandomizeAmount = 0;
        
        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy will need to wait before execute blocking action again.")]
        private float maxBlockingWaitRate = 5f;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdBlockingWaitRate\".")]
        private float minBlockingWaitRate = 4;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdBlockingWaitRate\".")]
        private float initialBlockingWaitRate = 7;

        [Tooltip("AI Rotate Turning Speed When blocking (TurnWithAgent - Fixed Tick)")]
        public float maxUpperBodyIKBlockingTurnSpeed = 10f;

        public float blockingAttackIntervalRate;

        public float blockingAttackIntervalRandomizeAmount;
        
        [Tooltip("The amount of time to cut or add on \"stdBlockingWaitRate\".")]
        public float damageReductionWhenBlocking = 0.3f;

        public float blockingDuabilityValue;

        [ReadOnlyInspector]
        public double currentBlockingDuabilityValue;

        [ReadOnlyInspector]
        [Tooltip("Shows if currently enemy is blocking or not, Note that if \"isBlockingAllTheTime\" is false, enemy will only block when blocking AIAction is executing.")]
        public bool isEnemyBlocking = false;

        [ReadOnlyInspector] public bool isInAttackBlocking;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("Determine if player previous hit on enemy when he is blocking has broken hit defense, enemy will play blocking break animation.")]
        bool isOnHitBlockingBreak;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("Determine if player hit on enemy was when he was blocking, enemy should play blocking react animation.")]
        bool isOnHitBlockingReact;

        [ReadOnlyInspector]
        [Tooltip("True when enemy's \"isBlockingAllTheTime\" is false and enemy finished blocking action, after \"finalizedBlockingWaitTime\" of time passed this will switch back to false.")]
        public bool enemyBlocked = false;
        
        [SerializeField, ReadOnlyInspector]
        private float currentBlockingStamina = 0;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of stamina that enemy will have.")]
        private float finalizedBlockingStaminaAmount = 0;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy will need to wait to execute blocking action again.")]
        private float finalizedBlockingWaitTime = 0;

        [NonSerialized] public float _defaultIKRetargetStoppingDistance;
        [NonSerialized] public float _defaultAttackIntervalRate;
        [NonSerialized] public float _defaultAttackIntervalRandomizeAmount;

        [NonSerialized] AIManager _ai;
        [NonSerialized] Animator _anim;
        [NonSerialized] public float _delta;

        /// TICK

        public void EnemyBlockingTick()
        {
            if (isEnemyBlocking)
            {
                if (_ai.isLockOnMoveAround)
                {
                    if (isBlockingWithoutAction)
                    {
                        _ai.SetNewLockonPositionToAgent();
                    }

                    finalizedBlockingStaminaAmount -= _delta;
                }
                else
                {
                    if (!_ai.isMovingToward)
                    {
                        _ai.SetNewLockonPositionImmediately();
                    }
                }

                if (finalizedBlockingStaminaAmount <= 0)
                {
                    finalizedBlockingStaminaAmount = 0;
                    OnHitBlockingBreak();
                }
            }
            else if (enemyBlocked)
            {
                finalizedBlockingWaitTime -= _delta;
                if (finalizedBlockingWaitTime <= 0)
                {
                    finalizedBlockingWaitTime = 0;
                    OffEnemyBlocked();

                    if (isBlockingWithoutAction)
                    {
                        OnIsEnemyBlocking();

                        /// If you want to move arond when blocking.
                        _ai.SetNewLockonPositionImmediately();
                    }
                }
            }
        }
        
        /// INIT

        public void EnemyBlockingModInit(AIManager _ai)
        {
            this._ai = _ai;
            _anim = _ai.anim;

            HashManager hashManager = _ai.hashManager;
            e_block_react_hash = hashManager.e_block_react_hash;
            e_block_break_hash = hashManager.e_block_break_hash;

            _defaultIKRetargetStoppingDistance = _ai.iKHandler.iKRetargetStoppingDistance;
            _defaultAttackIntervalRate = _ai.maxAttackIntervalRate;
            _defaultAttackIntervalRandomizeAmount = _ai.minAttackIntervalRate;
        }

        public void EnemyBlockingGoesAggroReset()
        {
            if (isBlockingWithoutAction)
            {
                enemyBlocked = true;
                finalizedBlockingWaitTime = initialBlockingWaitRate;
            }
            else
            {
                if (checkEnemyBlockedInit)
                {
                    enemyBlocked = true;
                    finalizedBlockingWaitTime = initialBlockingWaitRate;
                }
            }

            RefillBlockingDuabilityValue();
        }

        public void EnemyBlockingExitAggroReset()
        {
            if (enemyBlocked)
                OffEnemyBlocked();

            if (isEnemyBlocking)
            {
                OnHitBlockingBreak();
            }
            else
            {
                RandomizeWithAddonValue(blockingStaminaRandomizeAmount, stdBlockingStaminaAmount, ref finalizedBlockingStaminaAmount);
                currentBlockingStamina = finalizedBlockingStaminaAmount;
            }
        }

        /// SET METHODS
        
        public void OnHitBlockingBreak()
        {
            isEnemyBlocking = false;
            _ai.OffBlockingReverseStatus();

            enemyBlocked = true;
            RandomizeBlockingWaitRate();
            RefillBlockingDuabilityValue();
        }

        public void OnIsEnemyBlocking()
        {
            if (!isEnemyBlocking)
            {
                _ai.OnBlockingChangeStatus();
                isEnemyBlocking = true;
            }
        }

        void OffEnemyBlocked()
        {
            enemyBlocked = false;
            RandomizeWithAddonValue(blockingStaminaRandomizeAmount, stdBlockingStaminaAmount, ref finalizedBlockingStaminaAmount);
            currentBlockingStamina = finalizedBlockingStaminaAmount;
        }

        /// AI MANAGER - ON HIT

        void RefillBlockingDuabilityValue()
        {
            currentBlockingDuabilityValue = blockingDuabilityValue;
        }

        public void DepleteHealth_Blocking()
        {
            if (isEnemyBlocking)
            {
                /// If Player is in front of enemy when enemy get hit.
                if (_ai.GetIsWithinBlockingAngle())
                {
                    DepleteCurrentBlockingStamina();
                    DepleteAIHealthFromDeductedHitDamage();
                }
                else
                {
                    _ai.DepleteHealth_Regular();
                }
            }
            else
            {
                _ai.DepleteHealth_Regular();
            }
        }
        
        public void SpawnOnHitEffect()
        {
            if (isEnemyBlocking)
            {
                SpawnBlockingHitEffect();
            }
            else
            {
                _ai.Spawn_Regular_BloodFx();
            }
        }
        
        public void PlayBlockingOnHitAnimation()
        {
            if (isOnHitBlockingReact)
            {
                isOnHitBlockingReact = false;
                _ai.PlayAnimation_NoNeglect(e_block_react_hash, false);
            }
            else if (isOnHitBlockingBreak)
            {
                isOnHitBlockingBreak = false;
                _ai.PlayFallBackAnim(e_block_break_hash);
            }
            else
            {
                _ai.HandleArmedGetHitAnimation();
            }
        }

        void DepleteCurrentBlockingStamina()
        {
            currentBlockingDuabilityValue -= _ai._previousHitDamage;

            if (currentBlockingDuabilityValue <= 0)
            {
                isOnHitBlockingBreak = true;
            }
            else
            {
                isOnHitBlockingReact = true;
            }
        }

        void DepleteAIHealthFromDeductedHitDamage()
        {
            if (isOnHitBlockingReact)
            {
                _ai.currentEnemyHealth -= _ai._previousHitDamage * damageReductionWhenBlocking;
            }
            else
            {
                _ai.currentEnemyHealth -= _ai._previousHitDamage * 1.5f;
            }
        }

        void SpawnBlockingHitEffect()
        {
            WorldImpactEffect _effect = GameManager.singleton._blockImpactEffect;

            _effect.mTransform.parent = null;
            _effect.mTransform.position = _ai.firstWeapon.GetSidearmTransform().position;
            _effect.mTransform.eulerAngles = _ai.mTransform.eulerAngles;

            _effect.BlockingHit_SpawnEffect();
        }

        /// ATTCKING BLOCKING

        public void SetIsInAttackBlockingToTrue()
        {
            if (isEnemyBlocking)
            {
                isEnemyBlocking = false;
                isInAttackBlocking = true;
            }
        }

        public void SetIsInAttackBlockingToFalse()
        {
            if (isInAttackBlocking)
            {
                isInAttackBlocking = false;
                isEnemyBlocking = true;
            }
        }

        /// RANDOMIZE BLOCKING WAIT RATE.
        
        void RandomizeBlockingWaitRate()
        {
            finalizedBlockingWaitTime = Random.Range(maxBlockingWaitRate, minBlockingWaitRate);
        }
    }
}
