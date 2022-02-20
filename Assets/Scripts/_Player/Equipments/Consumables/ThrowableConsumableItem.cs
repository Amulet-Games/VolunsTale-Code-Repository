using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Consumables/Throwable Consumable Item")]
    public class ThrowableConsumableItem : ConsumableItem
    {
        [Header("Stack Id.")]
        public int throwableStackId;

        [Header("Throwable Attack Power Type.")]
        public P_Weapon_ElementTypeEnum throwableAttackPowerType;

        [Header("Attribute Bonus% .")]
        /// One of the major factor for BNS DMG caculation.
        /// S : 101%
        /// A : 81% - 100%
        /// B : 61% - 80%
        /// C : 45% - 60%
        /// D : 1% - 44%
        /// - : 0%
        [Range(0, 101)] public int throwableAtkAttriScaling;

        [Header("Throwable Prefab.")]
        public ThrowableConsumable modelPrefab;

        /// INIT METHODS
        public ThrowableConsumable GetNewThrowableConsumableInstance(SavableInventory _inventory)
        {
            ThrowableConsumable _throwableConsumable = Instantiate(modelPrefab);
            _throwableConsumable.InitThrowableConsumable(this);

            _inventory.ParentConsumableUnderBackpack(_throwableConsumable.transform);
            _inventory.SetConsumableSlotSideToBackpack(_throwableConsumable);
            return _throwableConsumable;
        }

        public ThrowableConsumable GetSaveFileThrowableConsumableInstance(SavableThrowableConsumableState _savableConsumableState, SavableInventory _inventory)
        {
            ThrowableConsumable _throwableConsumable = Instantiate(modelPrefab);
            _throwableConsumable.InitThrowableConsumableFromSave(_savableConsumableState ,this);
            return _throwableConsumable;
        }
        
        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            RuntimeConsumable _consumable = _inventory.GetConsumableFromDict(throwableStackId);
            if (_consumable)
            {
                _consumable.IncrementCarryingAmount(1);
            }
            else
            {
                GetNewThrowableConsumableInstance(_inventory);
            }
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            RuntimeConsumable _consumable = _inventory.GetConsumableFromDict(throwableStackId);
            if (_consumable)
            {
                _consumable.IncrementCarryingAmount(_amount);
            }
            else
            {
                GetNewThrowableConsumableInstance(_inventory);
            }
        }
        #endregion

        #region Throwable Info Detail.
        public string GetThrowableAtkAttriScalingText()
        {
            if (throwableAtkAttriScaling > 61)
            {
                if (throwableAtkAttriScaling > 81)
                {
                    if (throwableAtkAttriScaling > 101)
                    {
                        return "S";
                    }
                    else
                    {
                        return "A";
                    }
                }
                else
                {
                    return "B";
                }
            }
            else
            {
                if (throwableAtkAttriScaling > 45)
                {
                    return "C";
                }
                else
                {
                    return "D";
                }
            }
        }
        #endregion

        public override ThrowableConsumableItem GetThrowableConsumableItem()
        {
            return this;
        }
    }
}