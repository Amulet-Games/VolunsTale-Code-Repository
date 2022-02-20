using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class RingEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Ring Equip Refs.")]
        [ReadOnlyInspector] public RuntimeRing _referingRing;
        [ReadOnlyInspector] public RingEquipSlotsDetail ringEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadRingEquipSlot(RuntimeRing _runtimeRing)
        {
            _referingRing = _runtimeRing;
            ShowEquipSlot(_runtimeRing._referedRingItem.itemIcon);
        }

        public void UnLoadRingEquipSlot()
        {
            _referingRing = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            ringEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            ringEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Ring_AlterDetails(_referingRing);
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowRingInfoDetails(_referingRing);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingRing.runtimeName;
        }
        #endregion

        #region Setup.
        public void Setup(RingEquipSlotsDetail _ringEquipSlotDetail)
        {
            ringEquipSlotDetail = _ringEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = ringEquipSlotDetail.equipmentMenuManager;
            itemHub = ringEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}