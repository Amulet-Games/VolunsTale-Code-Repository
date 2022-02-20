using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace SA
{
    public class LoadGameProfile : MonoBehaviour, IPointerClickHandler
    {
        [Header("TMP Text (Drops).")]
        public TMP_Text profile_p_Name;
        public TMP_Text profile_p_Lv;
        public TMP_Text profile_p_Date;
        public TMP_Text profile_p_Voluns;
        public Image profile_p_Avatar;

        [Header("Profile (Drops).")]
        public RectTransform profileRect;
        public Canvas profileCanvas;

        [Header("Highlighter (Drops).")]
        public Image highlighterImage;
        public Canvas highlighterCanvas;

        [Header("Raycaster Image (Drop).")]
        public Image raycasterImage;

        [Header("Status.")]
        [Tooltip("Only the template that is going to be used will be set to active")]
        [ReadOnlyInspector] public int profileId;

        [Header("Refs.")]
        [ReadOnlyInspector] public ManSaveList_MainMenuMsg _manSaveListMsg;

        public void EnableProfile()
        {
            profileCanvas.enabled = true;
        }

        public void DisableProfile()
        {
            profileCanvas.enabled = false;
        }

        #region On Select Profile.
        public void OnPointerClick(PointerEventData eventData)
        {
            _manSaveListMsg.SetCurProfile_ByPointer(this);
            OnProfile_ByClick();
        }
        #endregion

        #region On Profile.
        public void OnProfile_ByInput()
        {
            ShowHighlighter_Input();
            DisableRaycaster();
        }

        public void OnProfile_ByClick()
        {
            ShowHighlighter_Click();
            DisableRaycaster();
        }
        #endregion

        #region Off Profile.
        public void OffProfile()
        {
            _manSaveListMsg.CancelHighlighterTween();
            HideHighlighter();
            EnableRaycaster();
        }
        #endregion

        #region Show / Hide Highlighter.
        void ShowHighlighter_Input()
        {
            highlighterCanvas.enabled = true;
            highlighterImage.color = _manSaveListMsg.highMidValueColor;
        }

        void ShowHighlighter_Click()
        {
            highlighterCanvas.enabled = true;
            _manSaveListMsg.TweenHighlighter_FullAlpha();
        }

        void HideHighlighter()
        {
            highlighterCanvas.enabled = false;
            highlighterImage.color = _manSaveListMsg.highNoAlphaColor;
        }
        #endregion

        #region Enable / Disable Raycaster.
        void EnableRaycaster()
        {
            raycasterImage.raycastTarget = true;
        }

        void DisableRaycaster()
        {
            raycasterImage.raycastTarget = false;
        }
        #endregion
        
        #region Setup.
        public void Setup(ManSaveList_MainMenuMsg manSaveListMsg)
        {
            _manSaveListMsg = manSaveListMsg;
            ResetStatus();
            DisableProfile();
        }

        void ResetStatus()
        {
            profileId = 0;
        }
        #endregion
    }
}