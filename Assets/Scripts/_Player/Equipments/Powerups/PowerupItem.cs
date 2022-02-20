using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Powerups/Powerup Item")]
    public class PowerupItem : Item
    {
        [Header("Powerup Item Stats.")]
        [TextArea(1, 30)]
        public string powerupEffectText;

        #region Dissolve Values.
        [Header("Preview Dissolve Config.")]
        public float _cutOffFullOpaqueValue;
        public float _cutOffTransparentValue;
        public float _fullTransparentValue;

        [Header("On Death Dissolve Config.")]
        public float _onDeath_cutOffFullOpaqueValue;
        public float _onDeath_cutOffTransparentValue;
        #endregion

        [Header("Drag and Drop Refs.")]
        public RuntimePowerup modelPrefab;

        public RuntimePowerup GetNewRuntimePowerupInstance(SavableInventory _inventory)
        {
            RuntimePowerup _runtimePowerup = Instantiate(modelPrefab);
            _runtimePowerup.InitRuntimePowerup(this);

            _inventory.ReturnPowerupToBackpack(_runtimePowerup);
            _inventory.AddPowerupToCarrying(_runtimePowerup);
            return _runtimePowerup;
        }

        public RuntimePowerup GetSaveFileRuntimePowerupInstance(SavablePowerupState _savablePowerupState, SavableInventory _inventory)
        {
            RuntimePowerup _runtimePowerup = Instantiate(modelPrefab);
            _runtimePowerup.InitRuntimePowerupFromSave(_savablePowerupState, this);

            _inventory.AddPowerupToCarrying(_runtimePowerup);
            return _runtimePowerup;
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            _inventory._pickedUpReadyDissolvePowerup = GetNewRuntimePowerupInstance(_inventory);
            _inventory.PreviewPowerup();
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimePowerupInstance(_inventory);
        }
        #endregion

        public override PowerupItem GetPowerupItem()
        {
            return this;
        }
    }
}