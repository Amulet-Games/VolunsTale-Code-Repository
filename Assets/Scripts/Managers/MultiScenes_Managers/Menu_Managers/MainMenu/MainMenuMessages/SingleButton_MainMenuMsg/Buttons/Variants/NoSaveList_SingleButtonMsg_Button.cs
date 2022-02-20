using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class NoSaveList_SingleButtonMsg_Button : Base_SingleButtonMsg_Button
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
                _referedMsg._mainMenuManager.Off_NoSaveList_byOK();
            }
        }
    }
}