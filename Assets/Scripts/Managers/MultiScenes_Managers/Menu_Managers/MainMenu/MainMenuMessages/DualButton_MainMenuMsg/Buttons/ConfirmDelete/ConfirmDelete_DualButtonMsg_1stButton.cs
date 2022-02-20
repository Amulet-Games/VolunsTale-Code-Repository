using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ConfirmDelete_DualButtonMsg_1stButton : Base_DualButtonMsg_Button
    {
        public override void OnSelectButton()
        {
            Base_OnSelectButton();

            LeanTween.value(0, 1, 0.12f).setOnComplete(OnCompleteWait);

            void OnCompleteWait()
            {
                _dualButtonMsg._mainMenuManager.manSaveList_MainMenuMsg.Confirm_RemoveProfile();
            }
        }
    }
}