using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Armors/Head Armor")]
    public class HeadArmorItem : ArmorItem
    {
        [Header("Head Armor Config.")]
        public RuntimeHeadArmor.HeadArmorTypeEnum headArmorType;
        public RuntimeHeadArmor modelPrefab;

        public RuntimeHeadArmor GetNewRuntimeHeadInstance(SavableInventory _inventory)
        {
            RuntimeHeadArmor _runtimeHeadArmor = Instantiate(modelPrefab);
            _runtimeHeadArmor.InitRuntimeHead(this);

            _inventory.ReturnHeadArmorToBackpack(_runtimeHeadArmor);
            _inventory.AddHeadArmorToCarrying(_runtimeHeadArmor);
            return _runtimeHeadArmor;
        }

        public RuntimeHeadArmor GetSaveFileRuntimeHeadInstance(SavableHeadState _savableHeadState, SavableInventory _inventory)
        {
            RuntimeHeadArmor _runtimeHeadArmor = Instantiate(modelPrefab);
            _runtimeHeadArmor.InitRuntimeHeadFromSave(_savableHeadState, this);

            _inventory.AddHeadArmorToCarrying(_runtimeHeadArmor);
            return _runtimeHeadArmor;
        }

        public void InitDeprivedHead(SavableInventory _inventory)
        {
            RuntimeHeadArmor _runtimeDeprivedHead = Instantiate(modelPrefab);
            _runtimeDeprivedHead.InitDeprivedHead(this);

            _inventory.ReturnHeadArmorToBackpack(_runtimeDeprivedHead);
            _inventory.runtimeDeprivedHead = _runtimeDeprivedHead;
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            _inventory.PreviewHeadArmor(GetNewRuntimeHeadInstance(_inventory));
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimeHeadInstance(_inventory);
        }
        #endregion

        public override HeadArmorItem GetHeadArmorItem()
        {
            return this;
        }
    }
}
