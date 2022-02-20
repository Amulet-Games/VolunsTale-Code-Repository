using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public abstract class ButtonOptionSlot : BaseOptionSlot
    {
        [Header("Drag and Drop Refs.")]
        public GameObject _checkerImage;

        public override void Tick()
        {
            if (_systemDetail._systemMenuManager.menu_select_input)
            {
                OnButtonSelect();
            }
        }

        public virtual void OnButtonSelect()
        {
            _systemDetail.SetHasChangedStatsStatusToTrue();
            _systemDetail.OnSelectChangeShadowColor();
        }

        public override void OnCurrentSlot()
        {
            ChangeShadowToHovering();
        }

        public override void OffCurrentSlot()
        {
            ChangeShadowToNormal();
        }

        public override void OnDetailCloseResetSlot()
        {
            ChangeShadowToNormal();
        }
    }
}