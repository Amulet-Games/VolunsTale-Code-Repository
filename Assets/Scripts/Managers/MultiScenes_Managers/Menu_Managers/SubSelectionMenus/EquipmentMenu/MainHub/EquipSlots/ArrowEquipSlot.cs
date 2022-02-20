using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class ArrowEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Arrow Equip Refs.")]
        [ReadOnlyInspector] public RuntimeArrow _referingArrow;
        [ReadOnlyInspector] public ArrowEquipSlotsDetail arrowEquipSlotDetail;

        #region Load / UnLoad
        public void LoadArrowEquipSlot(RuntimeArrow _runtimeArrow)
        {
            _referingArrow = _runtimeArrow;
            ShowEquipSlot(_runtimeArrow._referedArrowItem.itemIcon);
        }

        public void UnLoadArrowEquipSlot()
        {
            _referingArrow = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            arrowEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            arrowEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Arrow_AlterDetails(_referingArrow);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingArrow.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowArrowInfoDetails(_referingArrow);
        }
        #endregion

        #region Setup.
        public void Setup(ArrowEquipSlotsDetail _arrowEquipSlotDetail)
        {
            arrowEquipSlotDetail = _arrowEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = arrowEquipSlotDetail.equipmentMenuManager;
            itemHub = arrowEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}