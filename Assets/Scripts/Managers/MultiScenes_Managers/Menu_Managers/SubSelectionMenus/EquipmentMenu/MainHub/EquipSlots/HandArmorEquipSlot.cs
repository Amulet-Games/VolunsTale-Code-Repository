using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class HandArmorEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Hand Equip Refs.")]
        [ReadOnlyInspector] public RuntimeHandArmor _referingHandArmor;
        [ReadOnlyInspector] public HandArmorEquipSlotsDetail handEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadHandEquipSlot(RuntimeHandArmor _runtimeHandArmor)
        {
            _referingHandArmor = _runtimeHandArmor;
            ShowEquipSlot(_runtimeHandArmor._referedArmorItem.itemIcon);
        }

        public void UnLoadHandEquipSlot()
        {
            _referingHandArmor = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            handEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            handEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Armor_AlterDetails(_referingHandArmor);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingHandArmor.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowArmorInfoDetails(_referingHandArmor);
        }
        #endregion

        #region Setup.
        public void Setup(HandArmorEquipSlotsDetail _handEquipSlotDetail)
        {
            handEquipSlotDetail = _handEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = handEquipSlotDetail.equipmentMenuManager;
            itemHub = handEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}