using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class RuntimePowerup : RuntimeItem
    {
        [Header("Status.")]
        [ReadOnlyInspector] public PowerupSlotSideTypeEnum currentSlotSide;

        [Header("Refs.")]
        [ReadOnlyInspector] public Mesh powerupMesh;
        [ReadOnlyInspector] public PowerupItem _referedPowerupItem;

        /// INIT

        #region Vanilla Init.
        public void InitRuntimePowerup(PowerupItem _referedPowerupItem)
        {
            this._referedPowerupItem = _referedPowerupItem;

            InitRuntimeItem();

            InitDefaultStatus();
            InitModifiableStats();
            InitPowerupMesh();
        }

        void InitPowerupMesh()
        {
            Transform _childTransform = transform.GetChild(0);
            powerupMesh = _childTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            gameObject.SetActive(false);
        }

        void InitDefaultStatus()
        {
            slotNumber = 0;
        }

        void InitModifiableStats()
        {
            runtimeName = _referedPowerupItem.itemName;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimePowerupFromSave(SavablePowerupState _savablePowerupState, PowerupItem _referedPowerupItem)
        {
            this._referedPowerupItem = _referedPowerupItem;

            InitDefaultStatus();
            InitModifiableStats();
            InitPowerupMesh();
            LoadStatsFromSavable(_savablePowerupState);
        }

        public SavablePowerupState SavePowerupStateToSave()
        {
            SavablePowerupState _savablePowerupState = new SavablePowerupState();
            _savablePowerupState.savablePowerupName = _referedPowerupItem.itemName;
            _savablePowerupState.savablePowerupUniqueId = uniqueId;
            _savablePowerupState.savablePowerupSlotSide = (int)currentSlotSide;
            return _savablePowerupState;
        }

        void LoadStatsFromSavable(SavablePowerupState _savablePowerupState)
        {
            uniqueId = _savablePowerupState.savablePowerupUniqueId;
            currentSlotSide = (PowerupSlotSideTypeEnum)_savablePowerupState.savablePowerupSlotSide;
        }
        #endregion

        /// CHANGE PLAYER STATS METHODS

        public abstract void ChangeStatsWithPowerup(StatsAttributeHandler _statsHandler);

        public abstract void UndoPowerupStatsChanges(StatsAttributeHandler _statsHandler);

        /// OVERRIDE
        public override Item GetReferedItem()
        {
            return _referedPowerupItem;
        }

        public enum PowerupSlotSideTypeEnum
        {
            Slot,
            Backpack
        }
    }
}