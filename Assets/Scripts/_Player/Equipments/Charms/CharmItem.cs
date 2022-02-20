using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Charms/Charm Item")]
    public class CharmItem : Item
    {
        #region Item Stats.
        [Header("Charm Item Stats.")]
        [TextArea(1, 30)]
        public string charmEffectText;
        #endregion

        [Header("Drag and Drop Refs.")]
        public RuntimeCharm modelPrefab;

        public RuntimeCharm GetNewRuntimeCharmInstance(SavableInventory _inventory)
        {
            RuntimeCharm _runtimeCharm = Instantiate(modelPrefab);
            _runtimeCharm.InitRuntimeCharm(this);

            _inventory.ReturnCharmToBackpack(_runtimeCharm);
            _inventory.AddCharmToCarrying(_runtimeCharm);
            return _runtimeCharm;
        }

        public RuntimeCharm GetSaveFileRuntimeCharmInstance(SavableCharmState _savableCharmState, SavableInventory _inventory)
        {
            RuntimeCharm _runtimeCharm = Instantiate(modelPrefab);
            _runtimeCharm.InitRuntimeCharmFromSave(_savableCharmState, this);

            _inventory.AddCharmToCarrying(_runtimeCharm);
            return _runtimeCharm;
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            GetNewRuntimeCharmInstance(_inventory);
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimeCharmInstance(_inventory);
        }
        #endregion

        public override CharmItem GetCharmItem()
        {
            return this;
        }
    }
}