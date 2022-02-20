using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class StatsEffectConsumable : RuntimeConsumable
    {
        [Header("Stats Effect Config.")]
        public int _fortifiedLevel;

        [Header("Duration status.")]
        [ReadOnlyInspector] public bool _isDestroyAfterStatsEffectJob;

        [ReadOnlyInspector]
        public StatsEffectConsumableItem _referedStatsEffectItem;
        
        #region Vanilla Init.
        public void InitStatsEffectConsumable(StatsEffectConsumableItem _referedStatsEffectItem)
        {
            this._referedStatsEffectItem = _referedStatsEffectItem;
            _baseConsumableItem = _referedStatsEffectItem;

            InitRuntimeItem();

            InitStatsEffectDefaultStatus();
            InitStatsEffectModifiableStats();
            InitStatsEffectRuntimeStackId();
            InitStatsEffectRuntimeName();

            InitConsumableEffects();
        }

        void InitStatsEffectDefaultStatus()
        {
            gameObject.SetActive(false);
            isStatsEffectConsumable = true;
        }

        void InitStatsEffectModifiableStats()
        {
            _fortifiedLevel = 0;
        }

        void InitStatsEffectRuntimeStackId()
        {
            _stackId = _referedStatsEffectItem.statsEffectStackId;
        }

        void InitStatsEffectRuntimeName()
        {
            runtimeName = _inventory.GetFortifiedConsumableName(this);
        }
        #endregion

        #region Vanilla Vessel Init.
        public void InitVessel(StatsEffectConsumableItem _referedStatsEffectItem)
        {
            this._referedStatsEffectItem = _referedStatsEffectItem;
            _baseConsumableItem = _referedStatsEffectItem;

            InitRuntimeItem();

            InitVesselDefaultStatus();
            InitStatsEffectModifiableStats();
            InitStatsEffectRuntimeStackId();
            InitStatsEffectRuntimeName();
            InitConsumableEffects();

            SetupVessels_NewInstance();
        }

        void InitVesselDefaultStatus()
        {
            InitStatsEffectDefaultStatus();
        }

        protected virtual void SetupVessels_NewInstance()
        {
        }
        #endregion

        #region Loaded Save Init.
        public void InitStatsEffectConsumableFromSave(SavableStatsEffectConsumableState _savableConsumableState, StatsEffectConsumableItem _referedStatsEffectItem)
        {
            this._referedStatsEffectItem = _referedStatsEffectItem;
            _baseConsumableItem = _referedStatsEffectItem;

            InitStatsEffectDefaultStatus();
            LoadStatsFromSavable(_savableConsumableState);
            InitStatsEffectRuntimeName();

            InitConsumableEffects();
        }

        public SavableStatsEffectConsumableState SaveConsumableStateToSave()
        {
            SavableStatsEffectConsumableState _savableStatsEffectState = new SavableStatsEffectConsumableState();

            /// Runtime General.
            _savableStatsEffectState.savableStatsEffectName = _referedStatsEffectItem.itemName;
            _savableStatsEffectState.savableStatsEffectUniqueId = uniqueId;

            /// Consumable General.
            _savableStatsEffectState.savableStatsEffectSlotSide = (int)currentSlotSide;
            _savableStatsEffectState.savableStatsEffectSlotNumber = slotNumber;
            _savableStatsEffectState.savableStatsEffectIsCurrent = _isCurrentConsumable;

            /// Consumable Modifiable Stats.
            _savableStatsEffectState.savableStatsEffectFortifiedLevel = _fortifiedLevel;
            _savableStatsEffectState.savableStatsEffectCurrentCarryingAmount = curCarryingAmount;
            _savableStatsEffectState.savableStatsEffectCurrentStoredAmount = curStoredAmount;
            _savableStatsEffectState.savableStatsEffectIsCarryingEmpty = isCarryingEmpty;

            bool _isVessel = _referedStatsEffectItem.GetIsVessel();
            if (_isVessel)
            {
                _savableStatsEffectState.savableStatsEffectIsVessel = true;
                _savableStatsEffectState.savableStatsEffectIsVolun = _referedStatsEffectItem.GetIsVolun();
            }
            else
            {
                _savableStatsEffectState.savableStatsEffectIsVessel = false;
                _savableStatsEffectState.savableStatsEffectIsVolun = false;
            }

            /// Runtime Dictionary TKey.
            _savableStatsEffectState.savableStatsEffectStackId = _stackId;
            return _savableStatsEffectState;
        }

        void LoadStatsFromSavable(SavableStatsEffectConsumableState _savableConsumableState)
        {
            /// Runtime General.
            uniqueId = _savableConsumableState.savableStatsEffectUniqueId;

            /// Consumable General.
            currentSlotSide = (ConsumableSlotSideTypeEnum)_savableConsumableState.savableStatsEffectSlotSide;
            slotNumber = _savableConsumableState.savableStatsEffectSlotNumber;
            _isCurrentConsumable = _savableConsumableState.savableStatsEffectIsCurrent;

            /// Consumable Modifiable Stats.
            _fortifiedLevel = _savableConsumableState.savableStatsEffectFortifiedLevel;
            curCarryingAmount = _savableConsumableState.savableStatsEffectCurrentCarryingAmount;
            curStoredAmount = _savableConsumableState.savableStatsEffectCurrentStoredAmount;
            isCarryingEmpty = _savableConsumableState.savableStatsEffectIsCarryingEmpty;

            /// Runtime Dictionary TKey.
            _stackId = _savableConsumableState.savableStatsEffectStackId;
        }
        #endregion

        #region Loaded Save Vessel Init.
        public void InitVesselFromSave(SavableStatsEffectConsumableState _savableConsumableState, StatsEffectConsumableItem _referedStatsEffectItem)
        {
            this._referedStatsEffectItem = _referedStatsEffectItem;
            _baseConsumableItem = _referedStatsEffectItem;

            InitVesselDefaultStatus();
            LoadStatsFromSavable(_savableConsumableState);
            InitStatsEffectRuntimeName();
            InitConsumableEffects();

            SetupVessel_LoadedSave();
        }

        protected virtual void SetupVessel_LoadedSave()
        {
        }
        #endregion

        #region Reverse Effect.
        public virtual void OnCompleteReverseEffect(StatsAttributeHandler _statsHandler)
        {
        }
        #endregion

        #region Override / Abstract.
        public override StatsEffectConsumable GetStatsEffectConsumable()
        {
            return this;
        }

        public override Item GetReferedItem()
        {
            return _referedStatsEffectItem;
        }

        public abstract void ExecuteStatsEffect(StatsAttributeHandler _statsHandler);
        #endregion

        #region Check Empty.
        public override void CheckIsCarryingEmpty()
        {
            if (curCarryingAmount == 0)
            {
                isCarryingEmpty = true;
                if (!_referedStatsEffectItem.GetIsVessel())
                {
                    if (curStoredAmount > 0)
                    {
                        _inventory.EmptyConsumableSlot(slotNumber);
                        _inventory.TransferConsumableToStorage(this);
                    }
                    else
                    {
                        if (!_referedStatsEffectItem.isDurational)
                        {
                            DestroyNonDurationalConsumable();
                        }
                        else
                        {
                            DestroyDurationalConsumable();
                        }
                    }
                }
                else
                {
                    /// Only Vessel would be able to change to empty material.
                    ChangeVesselToEmpty();
                }
            }
        }

        public virtual void ChangeVesselToEmpty()
        {
        }
        #endregion

        #region On Destroy.
        void DestroyNonDurationalConsumable()
        {
            _inventory.OnDestroyConsumable(this);
            OnDestroyConsumable();
            GameObject.Destroy(this);
        }

        void DestroyDurationalConsumable()
        {
            _inventory.OnDestroyConsumable(this);
            _isDestroyAfterStatsEffectJob = true;
        }

        public void DestroyConsumableAfterJob()
        {
            if (_isDestroyAfterStatsEffectJob)
            {
                OnDestroyConsumable();
                GameObject.Destroy(gameObject);
            }
        }

        protected override void OnDestroyConsumable()
        {
            // Clear refs inside runtimeConsumable.
            _baseConsumableItem = null;
            _referedStatsEffectItem = null;
            _inventory = null;
        }
        #endregion

        #region Info / Alter Details.
        public override void UpdateStatsEffectInfoDetails(StatsEffectInfoDetails _statsEffectInfoDetails)
        {
            /// General Info
            _statsEffectInfoDetails.itemTitle_Text.text = runtimeName;

            _statsEffectInfoDetails.itemIcon_Image.sprite = _referedStatsEffectItem.itemIcon;
            
            /// Top Info Text
            _statsEffectInfoDetails.maxCarryingAmount_Text.text = _referedStatsEffectItem.maxCarryingAmount.ToString();
            _statsEffectInfoDetails.curCarryingAmount_Text.text = curCarryingAmount.ToString();

            _statsEffectInfoDetails.maxStoredAmount_Text.font = _statsEffectInfoDetails.century_normal_asset;
            _statsEffectInfoDetails.curStoredAmount_Text.font = _statsEffectInfoDetails.century_normal_asset;

            _statsEffectInfoDetails.maxStoredAmount_Text.text = _referedStatsEffectItem.maxStoredAmount.ToString();
            _statsEffectInfoDetails.curStoredAmount_Text.text = curStoredAmount.ToString();

            /// Bottom Desc Text
            _statsEffectInfoDetails.consumableEffect_Text.text = _referedStatsEffectItem.consumableEffectText.ToString();
        }

        public override void UpdateStatsEffectAlterDetails(StatsEffectAlterDetails _statsEffectAlterDetails)
        {
            /// General Info
            _statsEffectAlterDetails.itemTitle_Text.text = runtimeName;

            _statsEffectAlterDetails.itemIcon_Image.sprite = _referedStatsEffectItem.itemIcon;
            
            /// Top Desc Text
            _statsEffectAlterDetails.consumableEffect_Text.text = _referedStatsEffectItem.consumableEffectText.ToString();

            /// Bottom Info Text
            _statsEffectAlterDetails.maxCarryingAmount_Text.text = _referedStatsEffectItem.maxCarryingAmount.ToString();
            _statsEffectAlterDetails.curCarryingAmount_Text.text = curCarryingAmount.ToString();
        }
        #endregion
    }
}