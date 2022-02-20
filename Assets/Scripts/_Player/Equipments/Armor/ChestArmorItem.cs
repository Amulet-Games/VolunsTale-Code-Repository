using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Armors/Chest Armor")]
    public class ChestArmorItem : ArmorItem
    {
        [Header("Chest Armor Config.")]
        public RuntimeChestArmor modelPrefab;

        public RuntimeChestArmor GetNewRuntimeChestInstance(SavableInventory _inventory)
        {
            RuntimeChestArmor _runtimeChestArmor = Instantiate(modelPrefab);
            _runtimeChestArmor.InitRuntimeChest(this);

            _inventory.ReturnChestArmorToBackpack(_runtimeChestArmor);
            _inventory.AddChestArmorToCarrying(_runtimeChestArmor);
            return _runtimeChestArmor;
        }

        public RuntimeChestArmor GetSaveFileRuntimeChestInstance(SavableChestState _savableChestState, SavableInventory _inventory)
        {
            RuntimeChestArmor _runtimeChestArmor = Instantiate(modelPrefab);
            _runtimeChestArmor.InitRuntimeChestFromSave(_savableChestState, this);

            _inventory.AddChestArmorToCarrying(_runtimeChestArmor);
            return _runtimeChestArmor;
        }

        public void InitDeprivedChest(SavableInventory _inventory)
        {
            RuntimeChestArmor _runtimeDeprivedChest = Instantiate(modelPrefab);
            _runtimeDeprivedChest.InitDeprivedChest(this);

            _inventory.ReturnChestArmorToBackpack(_runtimeDeprivedChest);
            _inventory.runtimeDeprivedChest = _runtimeDeprivedChest;
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            _inventory.PreviewChestArmor(GetNewRuntimeChestInstance(_inventory));
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimeChestInstance(_inventory);
        }
        #endregion

        public override ChestArmorItem GetChestArmorItem()
        {
            return this;
        }
    }
}
