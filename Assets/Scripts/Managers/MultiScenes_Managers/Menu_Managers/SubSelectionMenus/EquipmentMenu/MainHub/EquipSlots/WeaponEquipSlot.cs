using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class WeaponEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Refs.")]
        [ReadOnlyInspector] public RuntimeWeapon _referingWeapon;
        [ReadOnlyInspector] public WeaponEquipSlotsDetail weaponEquipSlotDetail;

        #region Load / UnLoad
        public void LoadWeaponEquipSlot(RuntimeWeapon _runtimeWeapon)
        {
            _referingWeapon = _runtimeWeapon;
            ShowEquipSlot(_runtimeWeapon._referedWeaponItem.itemIcon);
        }

        public void UnLoadWeaponEquipSlot()
        {
            _referingWeapon = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            weaponEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            weaponEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Weapon_AlterDetails(_referingWeapon);
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowWeaponInfoDetails(_referingWeapon);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingWeapon.runtimeName;
        }
        #endregion

        #region Setup.
        public void Setup(WeaponEquipSlotsDetail _weaponEquipSlotDetail)
        {
            weaponEquipSlotDetail = _weaponEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = weaponEquipSlotDetail.equipmentMenuManager;
            itemHub = weaponEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}