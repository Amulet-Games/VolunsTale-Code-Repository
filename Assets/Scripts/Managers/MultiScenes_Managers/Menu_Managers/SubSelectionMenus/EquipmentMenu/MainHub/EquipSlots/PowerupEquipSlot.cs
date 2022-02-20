using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class PowerupEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Powerup Equip Refs.")]
        [ReadOnlyInspector] public RuntimePowerup _referingPowerup;
        [ReadOnlyInspector] public PowerupEquipSlotsDetail powerupEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadPowerupEquipSlot(RuntimePowerup _runtimePowerup)
        {
            _referingPowerup = _runtimePowerup;
            ShowEquipSlot(_runtimePowerup._referedPowerupItem.itemIcon);
        }

        public void UnLoadPowerupEquipSlot()
        {
            _referingPowerup = null;
            HideEquipSlot();
        }
        #endregion
        
        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            powerupEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            powerupEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Powerup_AlterDetails(_referingPowerup);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingPowerup.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowPowerupInfoDetails(_referingPowerup);
        }
        #endregion
        
        #region Setup.
        public void Setup(PowerupEquipSlotsDetail _powerupEquipSlotDetail)
        {
            powerupEquipSlotDetail = _powerupEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = powerupEquipSlotDetail.equipmentMenuManager;
            itemHub = powerupEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}