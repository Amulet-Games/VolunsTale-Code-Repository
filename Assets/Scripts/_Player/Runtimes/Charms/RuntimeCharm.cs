using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class RuntimeCharm : RuntimeItem
    {
        [Header("Status.")]
        [ReadOnlyInspector] public CharmSlotSideTypeEnum currentSlotSide;

        [Header("Refs.")]
        [ReadOnlyInspector] public CharmItem _referedCharmItem;

        /// INIT

        #region Vanilla Init.
        public void InitRuntimeCharm(CharmItem _referedCharmItem)
        {
            this._referedCharmItem = _referedCharmItem;

            InitRuntimeItem();

            InitDefaultStatus();
            InitModifiableStats();
        }

        void InitDefaultStatus()
        {
            slotNumber = 0;
        }

        void InitModifiableStats()
        {
            runtimeName = _referedCharmItem.itemName;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimeCharmFromSave(SavableCharmState _savableCharmState, CharmItem _referedCharmItem)
        {
            this._referedCharmItem = _referedCharmItem;

            InitDefaultStatus();
            InitModifiableStats();
            LoadStatsFromSavable(_savableCharmState);
        }

        public SavableCharmState SaveCharmStateToSave()
        {
            SavableCharmState _savableCharmState = new SavableCharmState();
            _savableCharmState.savableCharmName = _referedCharmItem.itemName;
            _savableCharmState.savableCharmUniqueId = uniqueId;
            _savableCharmState.savableCharmSlotSide = (int)currentSlotSide;
            return _savableCharmState;
        }

        void LoadStatsFromSavable(SavableCharmState _savableCharmState)
        {
            uniqueId = _savableCharmState.savableCharmUniqueId;
            currentSlotSide = (CharmSlotSideTypeEnum)_savableCharmState.savableCharmSlotSide;
        }
        #endregion

        /// CHANGE PLAYER STATS METHODS

        public abstract void ChangeStatsWithCharm(StateManager _states);

        public abstract void UndoCharmStatsChanges(StateManager _states);

        /// OVERRIDE

        public override Item GetReferedItem()
        {
            return _referedCharmItem;
        }
        
        public enum CharmSlotSideTypeEnum
        {
            Slot,
            Backpack
        }
    }
}