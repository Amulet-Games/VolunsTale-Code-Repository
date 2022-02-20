using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class CharmEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Charm Equip Refs.")]
        [ReadOnlyInspector] public RuntimeCharm _referingCharm;
        [ReadOnlyInspector] public CharmEquipSlotsDetail charmEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadCharmEquipSlot(RuntimeCharm _runtimeCharm)
        {
            _referingCharm = _runtimeCharm;
            ShowEquipSlot(_runtimeCharm._referedCharmItem.itemIcon);
        }

        public void UnLoadCharmEquipSlot()
        {
            _referingCharm = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            charmEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            charmEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Charm_AlterDetails(_referingCharm);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingCharm.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowCharmInfoDetails(_referingCharm);
        }
        #endregion

        #region Setup.
        public void Setup(CharmEquipSlotsDetail _charmEquipSlotDetail)
        {
            charmEquipSlotDetail = _charmEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = charmEquipSlotDetail.equipmentMenuManager;
            itemHub = charmEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}