using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Consumables/StatsEffect Consumable Item")]
    public class StatsEffectConsumableItem : ConsumableItem
    {
        [Header("Stack Id.")]
        public int statsEffectStackId;
        public int statsEffectJobId;

        [Header("StatsEffect Stats.")]
        public float effectAmount;

        [Header("Duration.")]
        public bool isDurational;
        public bool isRepeatable;
        public float durationalRate;

        [Header("States Effect Prefab.")]
        public StatsEffectConsumable modelPrefab;
        public Sprite statsEffectIcon;

        /// Vanilla Init.
        public StatsEffectConsumable GetNewStatsEffectConsumableInstance(SavableInventory _inventory)
        {
            StatsEffectConsumable _statsEffectConsumable = Instantiate(modelPrefab);

            _statsEffectConsumable._inventory = _inventory;
            _statsEffectConsumable.InitStatsEffectConsumable(this);

            _inventory.ParentConsumableUnderBackpack(_statsEffectConsumable.transform);
            _inventory.SetConsumableSlotSideToBackpack(_statsEffectConsumable);

            return _statsEffectConsumable;
        }

        public void GetNewVesselsInstance(SavableInventory _inventory)
        {
            StatsEffectConsumable _statsEffectConsumable = Instantiate(modelPrefab);

            _statsEffectConsumable._inventory = _inventory;
            _statsEffectConsumable.InitVessel(this);

            _inventory.ParentConsumableUnderBackpack(_statsEffectConsumable.transform);
        }

        /// Loaded Saves Init.
        public StatsEffectConsumable GetSaveFileStatsEffectConsumableInstance(SavableStatsEffectConsumableState _savableConsumableState, SavableInventory _inventory)
        {
            StatsEffectConsumable _statsEffectConsumable = Instantiate(modelPrefab);

            _statsEffectConsumable._inventory = _inventory;
            _statsEffectConsumable.InitStatsEffectConsumableFromSave(_savableConsumableState, this);

            return _statsEffectConsumable;
        }

        public void GetSaveFileVesselsIntance(SavableStatsEffectConsumableState _savableConsumableState, SavableInventory _inventory)
        {
            StatsEffectConsumable _statsEffectConsumable = Instantiate(modelPrefab);

            _statsEffectConsumable._inventory = _inventory;
            _statsEffectConsumable.InitVesselFromSave(_savableConsumableState, this);
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            RuntimeConsumable _consumable = _inventory.GetConsumableFromDict(statsEffectStackId);
            if (_consumable)
            {
                _consumable.IncrementCarryingAmount(1);
            }
            else
            {
                StatsEffectConsumable _statsEffectConsumable = GetNewStatsEffectConsumableInstance(_inventory);
                _statsEffectConsumable.IncrementCarryingAmount(1);
                _inventory.AddCarryConsumableToDictionary(_statsEffectConsumable);
            }
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            RuntimeConsumable _consumable = _inventory.GetConsumableFromDict(statsEffectStackId);
            if (_consumable)
            {
                _consumable.IncrementCarryingAmount(_amount);
            }
            else
            {
                StatsEffectConsumable _statsEffectConsumable = GetNewStatsEffectConsumableInstance(_inventory);
                _statsEffectConsumable.IncrementCarryingAmount(_amount);
                _inventory.AddCarryConsumableToDictionary(_statsEffectConsumable);
            }
        }
        #endregion

        public override StatsEffectConsumableItem GetStatsEffectConsumableItem()
        {
            return this;
        }
    }
}