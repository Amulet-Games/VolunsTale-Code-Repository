using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SaveAmtExcd_SingleButtonMsg_Button : Base_SingleButtonMsg_Button
    {
        public override void OnSelectButton()
        {
            CancelCurrentButtonTween();
            ChangeSpriteToPressed();
            WaitToFadeOutMessage();
        }

        void WaitToFadeOutMessage()
        {
            LeanTween.value(0, 1, 0.1f).setOnComplete(OnCompleteWait);

            void OnCompleteWait()
            {
                _referedMsg._mainMenuManager.Off_SaveAmtExcd_byOK();
            }
        }
    }
}