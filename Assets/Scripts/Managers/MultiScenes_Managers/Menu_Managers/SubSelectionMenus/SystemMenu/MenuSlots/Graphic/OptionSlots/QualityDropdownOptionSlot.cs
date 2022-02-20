using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class QualityDropdownOptionSlot : BaseOptionSlot
    {
        [Header("Graphic System Detail.")]
        [ReadOnlyInspector] public GraphicSystemDetail _graphicSystemDetail;

        [Header("Drag and Drop Refs.")]
        public QualityDropdownDetail _qualityDropdownDetail;
        public TMP_Text _dropDownTitleText;

        public override void Tick()
        {
            if (_systemDetail._systemMenuManager.menu_select_input)
            {
                OnQualityDropdownSelect();
            }
        }

        public override void OnCurrentSlot()
        {
            ChangeShadowToHovering();

            _qualityDropdownDetail.OnDetailPreview();
            _graphicSystemDetail.FadeInChoiceDetail_Half();
        }

        public override void OffCurrentSlot()
        {
            ChangeShadowToNormal();
            _graphicSystemDetail.FadeOutChoiceDetail_Full_Single();
        }

        public override void OnDetailCloseResetSlot()
        {
            ChangeShadowToNormal();
            _graphicSystemDetail.FadeOutChoiceDetail_Full_Single();
        }

        public override void DropdownsConnected_OnCurrentSlot_Part1()
        {
            ChangeShadowToHovering();
        }

        public override void DropdownsConnected_OnCurrentSlot_Part2()
        {
            _qualityDropdownDetail.OnDetailPreview();
            _graphicSystemDetail.FadeInChoiceDetail_Half();
        }

        public override void DropdownConnected_OffCurrentSlot()
        {
            ChangeShadowToNormal();
        }

        #region On Dropdown Select.
        public void OnQualityDropdownSelect()
        {
            OnDropdownSelectChangeShadowColor();
            
            _qualityDropdownDetail.OnDetailOpen();
            _graphicSystemDetail.OnDropDownSelect();
        }

        void OnDropdownSelectChangeShadowColor()
        {
            _systemDetail.OnSelectChangeShadowColor();
        }
        
        void OnDropdownSelectDisableDetailRaycastable()
        {
            _systemDetail.DisableSlotsRaycastable();
        }
        #endregion

        #region Get Dropdown Detail Canvas.
        public override Canvas GetDropdownDetailCanvas()
        {
            return _qualityDropdownDetail._choiceDetailCanvas;
        }
        #endregion

        #region Setup.
        public override void Setup(int _slotIndex, BaseSystemDetail _systemDetail)
        {
            base.Setup(_slotIndex, _systemDetail);

            SetupQualityDropdownDetail();
            SetupGetGraphicDetail();
            SetupStatus();
        }

        void SetupGetGraphicDetail()
        {
            _graphicSystemDetail = _systemDetail.GetGraphicSystemDetail();
            _graphicSystemDetail._dropDownCanvases[1] = _qualityDropdownDetail._choiceDetailCanvas;
        }

        void SetupQualityDropdownDetail()
        {
            _qualityDropdownDetail.Setup(_systemDetail._systemMenuManager, this);
        }

        void SetupStatus()
        {
            _isDropdownOptionSlot = true;
        }
        #endregion
    }
}