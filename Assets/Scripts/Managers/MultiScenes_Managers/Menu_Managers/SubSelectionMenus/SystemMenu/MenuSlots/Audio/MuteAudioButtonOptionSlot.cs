using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MuteAudioButtonOptionSlot : ButtonOptionSlot
    {
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isMutingAudio;

        #region On Button Select.
        public override void OnButtonSelect()
        {
            base.OnButtonSelect();
            SwitchMuteAudio();
        }

        void SwitchMuteAudio()
        {
            if (_isMutingAudio)
            {
                _isMutingAudio = false;
                _checkerImage.SetActive(false);
            }
            else
            {
                _isMutingAudio = true;
                _checkerImage.SetActive(true);
            }
        }
        #endregion
    }
}