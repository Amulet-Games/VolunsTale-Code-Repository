using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class RuntimeRing : RuntimeItem
    {
        [Header("Status.")]
        [ReadOnlyInspector] public RingSlotSideTypeEnum currentSlotSide;

        [Header("Ring Modifiable Stats.")]
        [ReadOnlyInspector] public int _fortifiedLevel;

        [Header("Refs.")]
        [ReadOnlyInspector] public ParticleSystem runtimeRingParticle;
        [ReadOnlyInspector] public RingItem _referedRingItem;

        /// INIT

        #region Vanilla Init.
        public void InitRuntimeRing(RingItem _referedRingItem)
        {
            this._referedRingItem = _referedRingItem;

            InitRuntimeItem();

            InitRuntimeRingParticle();
            InitDefaultStatus();
            InitModifiableStats();
        }

        void InitRuntimeRingParticle()
        {
            runtimeRingParticle = GetComponentInChildren<ParticleSystem>();
            runtimeRingParticle.gameObject.SetActive(false);
        }

        void InitDefaultStatus()
        {
            slotNumber = 0;
        }

        void InitModifiableStats()
        {
            _fortifiedLevel = 0;
        }
        #endregion

        #region Loaded Save Init.
        public void InitRuntimeRingFromSave(SavableRingState _savableRingState, RingItem _referedRingItem)
        {
            this._referedRingItem = _referedRingItem;

            InitRuntimeRingParticle();
            InitDefaultStatus();
            LoadStatsFromSavable(_savableRingState);
        }
        
        public SavableRingState SaveRingStateToSave()
        {
            SavableRingState _savableRingState = new SavableRingState();
            _savableRingState.savableRingName = _referedRingItem.itemName;
            _savableRingState.savableRingUniqueId = uniqueId;
            _savableRingState.savableRingSlotSide = (int)currentSlotSide;
            _savableRingState.savableRingFortifiedLevel = _fortifiedLevel;
            return _savableRingState;
        }

        void LoadStatsFromSavable(SavableRingState _savableRingState)
        {
            uniqueId = _savableRingState.savableRingUniqueId;
            currentSlotSide = (RingSlotSideTypeEnum)_savableRingState.savableRingSlotSide;
            _fortifiedLevel = _savableRingState.savableRingFortifiedLevel;
        }
        #endregion

        /// CHANGE PLAYER STATS METHODS

        public abstract void ChangeStatsWithRing(StatsAttributeHandler _statsHandler);

        public abstract void UndoRingStatsChanges(StatsAttributeHandler _statsHandler);

        /// OVERRIDE
        
        public override Item GetReferedItem()
        {
            return _referedRingItem;
        }

        public enum RingSlotSideTypeEnum
        {
            Right,
            Left,
            Backpack
        }
    }
}