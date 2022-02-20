using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LoadSystemDetail : BaseSystemDetail
    {
        #region On Detail Open.
        public override void OnDetailOpen()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region On Detail Close
        public override void OnDetailClose()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
        }

        void GetCurrentSlotByInput()
        {
        }
        #endregion

        #region Set Current Slot.
        public override void SetCurrentSlot(BaseOptionSlot _optionSlot)
        {
        }

        public override void OnCurrentSlot()
        {
        }

        public override void OffCurrentSlot()
        {
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
            //this._systemMenuManager = _systemMenuManager;
            //BaseSetup();
            //SetupSlots();
        }

        public override void SetupSlots()
        {
        }
        #endregion
    }
}