using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class HeadArmorEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Head Equip Refs.")]
        [ReadOnlyInspector] public RuntimeHeadArmor _referingHeadArmor;
        [ReadOnlyInspector] public HeadArmorEquipSlotsDetail headEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadHeadEquipSlot(RuntimeHeadArmor _runtimeHeadArmor)
        {
            _referingHeadArmor = _runtimeHeadArmor;
            ShowEquipSlot(_runtimeHeadArmor._referedArmorItem.itemIcon);
        }

        public void UnLoadHeadEquipSlot()
        {
            _referingHeadArmor = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            headEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            headEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Armor_AlterDetails(_referingHeadArmor);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingHeadArmor.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowArmorInfoDetails(_referingHeadArmor);
        }
        #endregion

        #region Setup.
        public void Setup(HeadArmorEquipSlotsDetail _headEquipSlotDetail)
        {
            headEquipSlotDetail = _headEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = headEquipSlotDetail.equipmentMenuManager;
            itemHub = headEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}