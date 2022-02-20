using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AudioDualOptionSlot : DualOptionSlot
    {
        #region Tick.
        public override void Tick()
        {
            GetCurrentOptionByInput();
            SelectCurrentOptionByInput();
        }
        #endregion

        public override void On_1st_OptionSelected()
        {
            Debug.Log("Audio First Selected");
        }

        public override void On_2nd_OptionSelected()
        {
            Debug.Log("Audio Second Selected");
        }
    }
}