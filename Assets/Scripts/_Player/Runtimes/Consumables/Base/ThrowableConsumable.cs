using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ThrowableConsumable : RuntimeConsumable
    {
        [ReadOnlyInspector]
        public ThrowableConsumableItem _referedThrowableItem;

        #region Vanilla Init.
        public void InitThrowableConsumable(ThrowableConsumableItem _referedThrowableItem)
        {
            this._referedThrowableItem = _referedThrowableItem;
            _baseConsumableItem = _referedThrowableItem;

            InitRuntimeItem();

            InitThrowableDefaultStatus();
            InitThrowableModifiableStats();
            InitThrowableRuntimeStackId();
        }

        void InitThrowableDefaultStatus()
        {
            gameObject.SetActive(false);
            isStatsEffectConsumable = false;
        }

        void InitThrowableModifiableStats()
        {
            runtimeName = _referedThrowableItem.itemName;
        }
        
        void InitThrowableRuntimeStackId()
        {
            _stackId = _referedThrowableItem.throwableStackId;
        }
        #endregion

        #region Loaded Save Init.
        public void InitThrowableConsumableFromSave(SavableThrowableConsumableState _savableConsumableState, ThrowableConsumableItem _referedThrowableItem)
        {
            this._referedThrowableItem = _referedThrowableItem;
            _baseConsumableItem = _referedThrowableItem;

            InitThrowableDefaultStatus();
            InitThrowableModifiableStats();
            LoadStatsFromSavable(_savableConsumableState);
        }

        public SavableThrowableConsumableState SaveConsumableStateToSave()
        {
            SavableThrowableConsumableState _savableThrowableState = new SavableThrowableConsumableState();

            /// Runtime General.
            _savableThrowableState.savableThrowableName = _referedThrowableItem.itemName;
            _savableThrowableState.savableThrowableUniqueId = uniqueId;

            /// Consumable General.
            _savableThrowableState.savableThrowableSlotSide = (int)currentSlotSide;
            _savableThrowableState.savableThrowableSlotNumber = slotNumber;
            _savableThrowableState.savableThrowableIsCurrent = _isCurrentConsumable;

            /// Consumable Modifiable Stats.
            _savableThrowableState.savableThrowableCurrentCarryingAmount = curCarryingAmount;
            _savableThrowableState.savableThrowableCurrentStoredAmount = curStoredAmount;
            _savableThrowableState.savableThrowableIsCarryingEmpty = isCarryingEmpty;

            /// Runtime Dictionary TKey.
            _savableThrowableState.savableThrowableStackId = _stackId;
            return _savableThrowableState;
        }

        void LoadStatsFromSavable(SavableThrowableConsumableState _savableConsumableState)
        {
            /// Runtime General.
            uniqueId = _savableConsumableState.savableThrowableUniqueId;

            /// Consumable General.
            currentSlotSide = (ConsumableSlotSideTypeEnum)_savableConsumableState.savableThrowableSlotSide;
            slotNumber = _savableConsumableState.savableThrowableSlotNumber;
            _isCurrentConsumable = _savableConsumableState.savableThrowableIsCurrent;

            /// Consumable Modifiable Stats.
            curCarryingAmount = _savableConsumableState.savableThrowableCurrentCarryingAmount;
            curStoredAmount = _savableConsumableState.savableThrowableCurrentStoredAmount;
            isCarryingEmpty = _savableConsumableState.savableThrowableIsCarryingEmpty;

            /// Runtime Dictionary TKey.
            _stackId = _savableConsumableState.savableThrowableStackId;
        }
        #endregion

        #region Override / Abstract.
        public override Item GetReferedItem()
        {
            return _referedThrowableItem;
        }
        #endregion

        #region Check Empty.
        public override void CheckIsCarryingEmpty()
        {
            if (curCarryingAmount == 0)
            {
                if (curStoredAmount > 0)
                {
                    isCarryingEmpty = true;
                    _inventory.EmptyConsumableSlot(slotNumber);
                    _inventory.TransferConsumableToStorage(this);
                }
                else
                {
                    DestroyThrowableConsumable();
                }
            }
        }
        #endregion

        #region On Destroy.
        void DestroyThrowableConsumable()
        {
            _inventory.OnDestroyConsumable(this);
            OnDestroyConsumable();
            GameObject.Destroy(this);
        }

        protected override void OnDestroyConsumable()
        {
            // Clear refs inside runtimeConsumable.
            _baseConsumableItem = null;
            _referedThrowableItem = null;
            _inventory = null;
        }
        #endregion
    }
}