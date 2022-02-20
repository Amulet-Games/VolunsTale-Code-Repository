using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class NoPrevSave_DualButtonMsg_2ndButton : Base_DualButtonMsg_Button
    {
        public override void OnSelectButton()
        {
            Base_OnSelectButton();

            LeanTween.value(0, 1, 0.12f).setOnComplete(OnCompleteWait);

            void OnCompleteWait()
            {
                _dualButtonMsg._mainMenuManager.Off_NoPrevSaveMsg_ByQuit();
            }
        }
    }
}
