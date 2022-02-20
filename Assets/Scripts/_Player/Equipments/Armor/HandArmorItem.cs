using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HandArmorItem
namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Armors/Hand Armor")]
    public class HandArmorItem : ArmorItem
    {
        [Header("Hand Armor Config.")]
        public RuntimeHandArmor.HandArmorTypeEnum handArmorType;
        public RuntimeHandArmor modelPrefab;

        public RuntimeHandArmor GetNewRuntimeHandInstance(SavableInventory _inventory)
        {
            RuntimeHandArmor _runtimeHandArmor = Instantiate(modelPrefab);
            _runtimeHandArmor.InitRuntimeHand(this);

            _inventory.ReturnHandArmorToBackpack(_runtimeHandArmor);
            _inventory.AddHandArmorToCarrying(_runtimeHandArmor);
            return _runtimeHandArmor;
        }

        public RuntimeHandArmor GetSaveFileRuntimeHandInstance(SavableHandState _savableHandState, SavableInventory _inventory)
        {
            RuntimeHandArmor _runtimeHandArmor = Instantiate(modelPrefab);
            _runtimeHandArmor.InitRuntimeHandFromSave(_savableHandState, this);

            _inventory.AddHandArmorToCarrying(_runtimeHandArmor);
            return _runtimeHandArmor;
        }

        public void InitDeprivedHand(SavableInventory _inventory)
        {
            RuntimeHandArmor _runtimeDeprivedHand = Instantiate(modelPrefab);
            _runtimeDeprivedHand.InitDeprivedHand(this);

            _inventory.ReturnHandArmorToBackpack(_runtimeDeprivedHand);
            _inventory.runtimeDeprivedHand = _runtimeDeprivedHand;
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            _inventory.PreviewHandArmor(GetNewRuntimeHandInstance(_inventory));
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimeHandInstance(_inventory);
        }
        #endregion

        public override HandArmorItem GetHandArmorItem()
        {
            return this;
        }
    }
}
