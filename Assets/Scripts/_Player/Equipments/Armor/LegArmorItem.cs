using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Armors/Leg Armor")]
    public class LegArmorItem : ArmorItem
    {
        [Header("Leg Armor Config.")]
        public RuntimeLegArmor.LegArmorTypeEnum legArmorTypeEnum;
        public RuntimeLegArmor modelPrefab;

        public RuntimeLegArmor GetNewRuntimeLegInstance(SavableInventory _inventory)
        {
            RuntimeLegArmor _runtimeLegArmor = Instantiate(modelPrefab);
            _runtimeLegArmor.InitRuntimeLegVanilla(this);

            _inventory.ReturnLegArmorToBackpack(_runtimeLegArmor);
            _inventory.AddLegArmorToCarrying(_runtimeLegArmor);
            return _runtimeLegArmor;
        }

        public RuntimeLegArmor GetSaveFileRuntimeLegInstance(SavableLegState _savableLegState, SavableInventory _inventory)
        {
            RuntimeLegArmor _runtimeLegArmor = Instantiate(modelPrefab);
            _runtimeLegArmor.InitRuntimeLegFromSave(_savableLegState, this);

            _inventory.AddLegArmorToCarrying(_runtimeLegArmor);
            return _runtimeLegArmor;
        }

        public void InitDeprivedLeg(SavableInventory _inventory)
        {
            RuntimeLegArmor _runtimeDeprivedLeg = Instantiate(modelPrefab);
            _runtimeDeprivedLeg.InitDeprivedLeg(this);
            
            _inventory.ReturnLegArmorToBackpack(_runtimeDeprivedLeg);
            _inventory.runtimeDeprivedLeg = _runtimeDeprivedLeg;
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            _inventory.PreviewLegArmor(GetNewRuntimeLegInstance(_inventory));
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimeLegInstance(_inventory);
        }
        #endregion

        public override LegArmorItem GetLegArmorItem()
        {
            return this;
        }
    }
}