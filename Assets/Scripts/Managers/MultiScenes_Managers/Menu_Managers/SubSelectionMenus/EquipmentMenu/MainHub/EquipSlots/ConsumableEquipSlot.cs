using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class ConsumableEquipSlot : ItemEquipSlot, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Consumable Equip Refs.")]
        [ReadOnlyInspector] public RuntimeConsumable _referingConsumable;
        [ReadOnlyInspector] public ConsumableEquipSlotsDetail consumEquipSlotDetail;

        #region Load / UnLoad.
        public void LoadConsumableEquipSlot(RuntimeConsumable _runtimeConsumable)
        {
            _referingConsumable = _runtimeConsumable;

            if (_runtimeConsumable.isCarryingEmpty)
            {
                ShowEquipSlot(_runtimeConsumable._baseConsumableItem.GetEmptyConsumableSprite());
            }
            else
            {
                ShowEquipSlot(_runtimeConsumable._baseConsumableItem.itemIcon);
            }
        }

        public void UnLoadConsumableEquipSlot()
        {
            _referingConsumable = null;
            HideEquipSlot();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            consumEquipSlotDetail.GetCurrentSlotByPointerEvent(this);
        }
        #endregion

        #region On Pointer Exit.
        public void OnPointerExit(PointerEventData eventData)
        {
            consumEquipSlotDetail.isCursorOverSelection = false;
        }
        #endregion

        #region Redraws.
        public override void Redraw_Post_AlterDetails()
        {
            itemHub.Redraw_Post_Consumable_AlterDetails(_referingConsumable);
        }

        protected override void Redraw_MainHub_TopText()
        {
            mainHub.currentItemNameText.text = _referingConsumable.runtimeName;
        }

        public override void Redraw_EquipSlot_InfoDetails()
        {
            itemHub.ShowConsumableInfoDetails(_referingConsumable);
        }
        #endregion

        #region Setup.
        public void Setup(ConsumableEquipSlotsDetail _consumEquipSlotDetail)
        {
            consumEquipSlotDetail = _consumEquipSlotDetail;

            GetBaseReferences();
            
            HideEquipSlot();
        }

        public void GetBaseReferences()
        {
            equipmentMenuManager = consumEquipSlotDetail.equipmentMenuManager;
            itemHub = consumEquipSlotDetail.itemHub;
            mainHub = equipmentMenuManager.mainHub;
        }
        #endregion
    }
}