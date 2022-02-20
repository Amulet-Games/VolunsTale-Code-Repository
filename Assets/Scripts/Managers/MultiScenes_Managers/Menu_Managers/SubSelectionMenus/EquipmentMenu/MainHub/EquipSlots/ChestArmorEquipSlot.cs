using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class ChestArmorEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Chest Equip Refs.")]
        [ReadOnlyInspector] public RuntimeChestArmor _referingChestArmor;
        [ReadOnlyInspector] public ChestArmorEquipSlotsDetail chestEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadChestEquipSlot(RuntimeChestArmor _runtimeChestArmor)
        {
            _referingChestArmor = _runtimeChestArmor;
            ShowEquipSlot(_runtimeChestArmor._referedArmorItem.itemIcon);
        }

        public void UnLoadChestEquipSlot()
        {
            _referingChestArmor = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            chestEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            chestEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Armor_AlterDetails(_referingChestArmor);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingChestArmor.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowArmorInfoDetails(_referingChestArmor);
        }
        #endregion

        #region Setup.
        public void Setup(ChestArmorEquipSlotsDetail _chestEquipSlotDetail)
        {
            chestEquipSlotDetail = _chestEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = chestEquipSlotDetail.equipmentMenuManager;
            itemHub = chestEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}