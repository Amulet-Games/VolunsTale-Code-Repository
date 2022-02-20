using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class LegArmorEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Leg Equip Refs.")]
        [ReadOnlyInspector] public RuntimeLegArmor _referingLegArmor;
        [ReadOnlyInspector] public LegArmorEquipSlotsDetail legEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadLegEquipSlot(RuntimeLegArmor _runtimeLegArmor)
        {
            _referingLegArmor = _runtimeLegArmor;
            ShowEquipSlot(_runtimeLegArmor._referedArmorItem.itemIcon);
        }

        public void UnLoadLegEquipSlot()
        {
            _referingLegArmor = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            legEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            legEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Armor_AlterDetails(_referingLegArmor);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingLegArmor.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowArmorInfoDetails(_referingLegArmor);
        }
        #endregion

        #region Setup.
        public void Setup(LegArmorEquipSlotsDetail _legEquipSlotDetail)
        {
            legEquipSlotDetail = _legEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = legEquipSlotDetail.equipmentMenuManager;
            itemHub = legEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}