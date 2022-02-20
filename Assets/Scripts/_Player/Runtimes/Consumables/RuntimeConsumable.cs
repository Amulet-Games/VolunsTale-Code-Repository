using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class RuntimeConsumable : RuntimeItem
    {
        /* Pickup item, if consumable --> search thr the Dictionary, 
         * if key exist then just increase the current amount from runtime */
        [Header("Stack Id.")]
        [ReadOnlyInspector] public int _stackId;

        [Header("Consumable Modifiable Stats.")]
        [ReadOnlyInspector] public int curCarryingAmount;
        [ReadOnlyInspector] public int curStoredAmount;
        [ReadOnlyInspector] public bool isCarryingEmpty;

        [Header("Status.")]
        [ReadOnlyInspector] public ConsumableSlotSideTypeEnum currentSlotSide;
        [ReadOnlyInspector] public bool _isCurrentConsumable;
        [ReadOnlyInspector] public bool isStatsEffectConsumable;

        [Header("Refs.")]
        [ReadOnlyInspector] public SavableInventory _inventory;
        [ReadOnlyInspector] public ConsumableItem _baseConsumableItem;
        [ReadOnlyInspector] public RuntimeConsumableEffect _referedEffect;

        public void IncrementCarryingAmount(int _amount)
        {
            curCarryingAmount += _amount;

            int _maxCarryingAmount = _baseConsumableItem.maxCarryingAmount;
            int _maxStoredAmount = _baseConsumableItem.maxStoredAmount;

            if (curCarryingAmount > _maxCarryingAmount)
            {
                curStoredAmount += curCarryingAmount - _maxCarryingAmount;
                if (curStoredAmount > _maxStoredAmount)
                    curStoredAmount = _maxStoredAmount;

                curCarryingAmount = _maxCarryingAmount;
            }

            isCarryingEmpty = false;
        }

        #region Get Variants.
        public virtual StatsEffectConsumable GetStatsEffectConsumable()
        {
            return null;
        }

        public virtual ThrowableConsumable GetThrowableConsumable()
        {
            return null;
        }
        #endregion

        #region Check Empty.
        public abstract void CheckIsCarryingEmpty();
        #endregion
        
        #region On Destroy.
        protected abstract void OnDestroyConsumable();
        #endregion

        #region Info / Alter Details.
        public virtual void UpdateStatsEffectInfoDetails(StatsEffectInfoDetails _statsEffectInfoDetails)
        {
        }

        public virtual void UpdateStatsEffectAlterDetails(StatsEffectAlterDetails _statsEffectAlterDetails)
        {
        }
        #endregion

        #region Consumable Effects Init.
        protected void InitConsumableEffects()
        {
            _referedEffect = Instantiate(_baseConsumableItem.effectPrefab);
            _referedEffect.Init(_inventory);
        }
        #endregion

        public enum ConsumableSlotSideTypeEnum
        {
            Slot,
            Backpack,
            Storage
        }
    }
}