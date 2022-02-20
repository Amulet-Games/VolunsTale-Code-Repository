using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class QuitSystemDetail : BaseSystemDetail
    {
        #region On Detail Open.
        public override void OnDetailOpen()
        {
            OnDetailOpenSetCurrentSlot();
            OnDetailOpenEnableExtraInputs();
            OnDetailOpenChangeDualOptionSprite();
            SetDetailAsCurrentUpdatable();
            EnableSlotsRaycastable();
        }

        void OnDetailOpenChangeDualOptionSprite()
        {
            _currentDualOptionSlot.OnQuitMenuOpenSetSlot();
        }
        #endregion

        #region On Detail Close
        public override void OnDetailClose()
        {
            OnDetailCloseResetSlot();
            OnDetailCloseDisableExtraInputs();
            OnDetailCloseChangeDualOptionSprite();
            DisableSlotsRaycastable();
        }

        void OnDetailCloseChangeDualOptionSprite()
        {
            _currentDualOptionSlot.ChangeAllSpriteToNotAvabilable();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            _systemMenuManager.CloseMenuByInput();

            _currentSlot.Tick();
        }
        #endregion

        #region Set Current Slot.
        public override void SetCurrentSlot(BaseOptionSlot _optionSlot)
        {
            OffCurrentSlot();
            _currentSlot = _optionSlot;
            OnCurrentSlot();
        }

        public override void OnCurrentSlot()
        {
            _currentSlot.OnCurrentSlot();
        }

        public override void OffCurrentSlot()
        {
            _currentSlot.OffCurrentSlot();
        }
        #endregion

        #region On Select Change Shadow Color.
        public override void OnSelectChangeShadowColor()
        {
        }
        #endregion

        #region Setup.
        public override void Setup(SystemMenuManager _systemMenuManager)
        {
            this._systemMenuManager = _systemMenuManager;
            BaseSetup();
            SetupSlots();
        }

        public override void SetupSlots()
        {
            _slotsLength = 1;
            _optionSlots[0].Setup(0, this);

            _hasChangedStats = true;
            _currentDualOptionSlot.ChangeAllImageRaycastable();
        }
        #endregion
    }
}