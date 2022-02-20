using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class EnemyInteractableMod : AIMod
    {
        [HideInInspector]
        public bool showEnemyInteractableMod;

        [Tooltip("The amount of time without randomized that enemy will need to wait before searching interactables again.")]
        public float stdSearchWaitRate = 6.5f;

        [Tooltip("If this is true, \"stdSearchWaitRate\" will change each time after interactable is used.")]
        public float interactableSearchRandomizeAmount = 1;

        [Tooltip("How big is the interactable search range .")]
        public float interactableSearchRange = 7;

        [Tooltip("How high the enemy or interactable can be on top of each other .")]
        public float interactableHeightLimit = 3;

        [Tooltip("How close to the desired_interactable that enemy will execute it.")]
        public float interactableExecuteDistance;
        
        [ReadOnlyInspector]
        [Tooltip("The list of interactables that has been found in current searched.")]
        public List<AI_Interactable> searched_interactables;

        [ReadOnlyInspector]
        public bool switchTargetToInteractable = false;

        [ReadOnlyInspector]
        public bool isPickingUpPowerWeapon = false;

        [ReadOnlyInspector]
        public AI_Interactable desired_interactable = null;

        [ReadOnlyInspector]
        public bool canSearchInteractable = false;

        [ReadOnlyInspector]
        public float finalizedSearchWaitRate = 0;

        [ReadOnlyInspector]
        public PowerWeapon_Interactable currentPowerWeaponInteractable;

        [ReadOnlyInspector]
        public ThrowableEnemyRuntimePowerWeapon currentPowerWeapon;

        [ReadOnlyInspector]
        public bool isCurrentPowerWeaponBroke;
        
        private Collider[] hitColliders = new Collider[3];

        [NonSerialized] public float _delta;
        [NonSerialized] AIManager _aiManager;

        /// INIT
        public void EnemyInteractableInit(AIManager _ai)
        {
            _aiManager = _ai;

            searched_interactables = new List<AI_Interactable>();
        }

        /// On / Off Aggro.
        public void EnemyInteractableGoesAggroReset()
        {
            canSearchInteractable = true;
            RandomizeWithAddonValue(interactableSearchRandomizeAmount, stdSearchWaitRate, ref finalizedSearchWaitRate);
        }

        public void EnemyInteractableExitAggroReset()
        {
            desired_interactable = null;
            switchTargetToInteractable = false;
        }
        
        #region Tick.
        public void MonitorInteractables()
        {
            // if enemy can search for interactables and he hasn't interact with any interactables yet.
            if (canSearchInteractable)
            {
                if (!desired_interactable)
                {
                    finalizedSearchWaitRate -= _delta;
                    if (finalizedSearchWaitRate <= 0)
                    {
                        SearchInteractables();
                    }
                }
            }
        }

        public void ExecuteInteractable()
        {
            _aiManager.isLockOnMoveAround = false;
            _aiManager.isMovingToward = true;

            float disToDesiredInteractableNonSqr = (desired_interactable.transform.position - _aiManager.mTransform.position).sqrMagnitude;
            if (disToDesiredInteractableNonSqr <= interactableExecuteDistance)
            {
                desired_interactable.Execute(_aiManager);
            }
        }

        void SearchInteractables()
        {
            // Clear Previous List 
            ClearSearchedInteractableListArray();

            int healthRecoveryCount = 0;
            int powerWeaponCount = 0;

            Vector3 _cur_AI_Pos = _aiManager.mTransform.position;
            Vector3 _found_Inter_Pos;

            int totalHitNum = Physics.OverlapSphereNonAlloc(_cur_AI_Pos, interactableSearchRange, hitColliders, LayerManager.singleton._enemyInteractableMask);
            for (int i = 0; i < totalHitNum; i++)
            {
                AI_Interactable foundInteractable = hitColliders[i].GetComponent<AI_Interactable>();
                _found_Inter_Pos = foundInteractable.transform.position;

                #region Height Check.
                if (_cur_AI_Pos.y > _found_Inter_Pos.y)
                {
                    if (_cur_AI_Pos.y - _found_Inter_Pos.y > interactableHeightLimit)
                    {
                        continue;
                    }
                }
                else
                {
                    if (_found_Inter_Pos.y - _cur_AI_Pos.y > interactableHeightLimit)
                    {
                        continue;
                    }
                }
                #endregion

                if (foundInteractable.interactableType == AI_InteractableTypeEnum.Health_Inter)
                {
                    healthRecoveryCount++;
                }
                else
                {
                    powerWeaponCount++;
                }

                searched_interactables.Add(foundInteractable);
            }

            ChooseSuitableInteractables();

            void ClearSearchedInteractableListArray()
            {
                searched_interactables.Clear();
            }

            void ChooseSuitableInteractables()
            {
                AI_InteractableTypeEnum _desireInterType;
                int _eligiableIntersCount = 0;

                // if there is health recovery interactable nearby
                if (healthRecoveryCount > 0)
                {
                    // and this enemy is low on health, health recovery will be the priority.
                    if (_aiManager.currentEnemyHealth <= _aiManager.totalEnemyHealth * 0.35f)
                    {
                        _desireInterType = AI_InteractableTypeEnum.Health_Inter;
                        RemoveNonEligibleInterFromList();
                    }
                    // otherwise power weapon will be the choice.
                    else
                    {
                        _desireInterType = AI_InteractableTypeEnum.PW_Inter;
                        RemoveNonEligibleInterFromList();
                    }
                }
                // if there is no health recovery interactable nearby but there is power Weapon nearby
                else if (powerWeaponCount > 0)
                {
                    _desireInterType = AI_InteractableTypeEnum.PW_Inter;
                    RemoveNonEligibleInterFromList();
                }

                // if there is still searched interactables left...
                if (_eligiableIntersCount > 0)
                {
                    if (_eligiableIntersCount == 1)
                    {
                        desired_interactable = searched_interactables[0];
                    }
                    else
                    {
                        desired_interactable = searched_interactables[Random.Range(0, searched_interactables.Count)];
                    }

                    SetSwitchTargetToInteractableToTrue();
                }

                void RemoveNonEligibleInterFromList()
                {
                    int _temp_searchedCount = searched_interactables.Count;

                    if (_temp_searchedCount == 1)
                    {
                        if (searched_interactables[0].interactableType != _desireInterType)
                        {
                            searched_interactables.Remove(searched_interactables[0]);
                            _eligiableIntersCount--;
                        }
                        else
                        {
                            _eligiableIntersCount = 1;
                        }
                    }
                    else
                    {
                        _eligiableIntersCount = _temp_searchedCount;

                        for (int i = 0; i < _temp_searchedCount; i++)
                        {
                            if (searched_interactables[i].interactableType != _desireInterType)
                            {
                                searched_interactables.Remove(searched_interactables[i]);
                                _eligiableIntersCount--;
                            }
                        }
                    }
                }

                void SetSwitchTargetToInteractableToTrue()
                {
                    switchTargetToInteractable = true;

                    canSearchInteractable = false;
                    RandomizeWithAddonValue(interactableSearchRandomizeAmount, stdSearchWaitRate, ref finalizedSearchWaitRate);
                }
            }
        }
        #endregion

        #region On Hit.
        public void OnChargedAttackHit()
        {
            if (isPickingUpPowerWeapon)
            {
                GetPowerWeapon();
                SetSwitchTargetToInteractableToFalse();
            }
            else
            {
                if (currentPowerWeapon)
                {
                    HitByChargeAttack_Deplete_PW_Duability();
                }
            }
        }
        #endregion
        
        #region Anim Events.

        #region Get Power Weapon.
        public void GetPowerWeapon()
        {
            if (!isPickingUpPowerWeapon)    /// Prevent when AI get Knocked Down This Anim Event Get Invoked.
                return;

            SetIsInGettingInterAnimToFalse();
            SetupPowerWeaponAnimBeforeUse();
            SetIsCurrentPowerWeaponBrokeToFalse();
            ChangeToUsePowerWeaponActionHolder();
            SetupPowerWeaponFromPool();

            void SetupPowerWeaponFromPool()
            {
                ThrowableEnemyRuntimePowerWeapon newPowerWeapon = currentPowerWeaponInteractable.powerWeaponPool.Get();

                _aiManager.currentWeapon = newPowerWeapon;
                _aiManager.currentThrowableWeapon = newPowerWeapon;
                currentPowerWeapon = newPowerWeapon;

                newPowerWeapon._ai = _aiManager;

                if (newPowerWeapon.isThrowableInited)
                {
                    newPowerWeapon.ReSetupRuntimePowerWeapon();
                }
                else
                {
                    newPowerWeapon.SetupRuntimePowerWeapon(currentPowerWeaponInteractable.powerWeaponPool);
                }
            }
        }
        
        void SetIsInGettingInterAnimToFalse()
        {
            isPickingUpPowerWeapon = false;
        }

        void SetupPowerWeaponAnimBeforeUse()
        {
            _aiManager.currentCrossFadeLayer = currentPowerWeaponInteractable.pw_profile.pw_AnimStateLayer;
            _aiManager.anim.SetBool(_aiManager.hashManager.e_javelin_isEquiped_hash, true);
        }

        void SetIsCurrentPowerWeaponBrokeToFalse()
        {
            isCurrentPowerWeaponBroke = false;
        }

        void ChangeToUsePowerWeaponActionHolder()
        {
            _aiManager.currentActionHolder = currentPowerWeaponInteractable.pw_profile.pw_ActionHolder;
        }

        public void SetIsInGettingInterAnimToTrue()
        {
            isPickingUpPowerWeapon = true;
        }

        public void SetSwitchTargetToInteractableToFalse()
        {
            if (!switchTargetToInteractable)
                return;

            desired_interactable = null;
            switchTargetToInteractable = false;
            _aiManager.SetTargetPosToPlayer();
        }
        #endregion

        #region Break Power Weapon.

        #region When Idle.
        public void BreakPowerWeaponWhenIdle()
        {
            if (isCurrentPowerWeaponBroke)
                ReactToPowerWeaponBroken();
        }
        
        public void ReactToPowerWeaponBroken()
        {
            _aiManager.PlayPowerWeaponBrokenReaction();

            JavelinBroken_AIGeneralEffect _effect = _aiManager.aISessionManager._javelinBroken_pool.Get();
            _effect.SpawnEffect_AfterDuabilityEmpty(currentPowerWeapon.transform);

            ClearPowerWeaponRefsAfterBroke();
        }
        #endregion

        #region When Knocked Down.
        public void BreakPowerWeaponWhenKnockedDown()
        {
            if (isCurrentPowerWeaponBroke)
            {
                JavelinBroken_AIGeneralEffect _effect = _aiManager.aISessionManager._javelinBroken_pool.Get();
                _effect.SpawnEffect_AfterDuabilityEmpty(currentPowerWeapon.transform);

                ClearPowerWeaponRefsAfterBroke();
            }
        }
        #endregion

        void ClearPowerWeaponRefsAfterBroke()
        {
            isCurrentPowerWeaponBroke = true;
            currentPowerWeapon.transform.parent = null;
            currentPowerWeapon.ReturnToPool();
            ReturnFromThrowablePowerWeapon();
        }

        public void ClearPowerWeaponRefsAfterThrown()
        {
            _aiManager.skippingScoreCalculation = false;
            ReturnFromThrowablePowerWeapon();
        }
        #endregion

        #region PW Attacks.
        public void SetPowerWeaponMSA_Available()
        {
            if (!isCurrentPowerWeaponBroke)
            {
                _aiManager.isMultiStageAttackAvailable = true;
            }
        }

        public void PowerWeaponDamageColliderStatus(int value)
        {
            if (currentPowerWeapon != null)
            {
                if (value == 1)
                {
                    currentPowerWeapon.e_hook.SetColliderStatusToTrue();
                    _aiManager._isParryable = true;
                }
                else
                {
                    currentPowerWeapon.e_hook.SetColliderStatusToFalse();
                }
            }
        }
        #endregion

        #endregion
        
        #region Resets After Broke / Thrown.
        void ReturnFromThrowablePowerWeapon()
        {
            SetCurrentPowerWeaponToNull();
            ReturnFromPowerWeaponSetStatus();
            SetEquipFirstWeaponPassiveAction();

            _aiManager.ReturnFromThrowablePowerWeapon();

            void SetCurrentPowerWeaponToNull()
            {
                currentPowerWeapon = null;
                currentPowerWeaponInteractable = null;
            }

            void ReturnFromPowerWeaponSetStatus()
            {
                canSearchInteractable = true;
            }
            
            void SetEquipFirstWeaponPassiveAction()
            {
                _aiManager.Set_EquipFirstWeaponAfterPW_PassiveAction();
            }
        }
        #endregion

        #region Deplete PowerWeapon Duability.
        public void Deplete_PW_Duability()
        {
            currentPowerWeapon.DepletePowerWeaponDuability();
        }

        public void HitByChargeAttack_Deplete_PW_Duability()
        {
            currentPowerWeapon.DepletePowerWeaponDuabiltiyByAmount(4);
        }
        #endregion
        
        /// On Checkpoint.
        public void EnemyInteractable_HandleWeaponSheath()
        {
            if (currentPowerWeapon)
                ClearPowerWeaponRefsAfterBroke();
        }
        
        /// Enum.
        public enum AI_InteractableTypeEnum
        {
            Health_Inter,
            PW_Inter
        }
    }
}