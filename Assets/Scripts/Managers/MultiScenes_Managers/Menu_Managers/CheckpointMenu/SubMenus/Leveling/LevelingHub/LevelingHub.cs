using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class LevelingHub : MonoBehaviour
    {
        [Header("Definable Attributes.")]
        public AlterableDefinableAttributePreview[] _attributePreviews;

        [Header("General Attributes.")]
        public DefinablePlayerLevelPreview _playerLevelPreview;
        public DefinableVolunPreview _volunPointPreview;
        public DefinableAcceptButtonPreview _acceptButtonPreview;

        [Header("Next Level Require Text.")]
        public Text _nextLevelRequiresText;

        [Header("Accept Buttons.")]
        public Image _acceptButtonImage;
        public Sprite _disabledButtonSprite;
        public Sprite _enabledButtonSprite;
        public Sprite _hoveringButtonSprite;
        
        [Header("Highlighter Status.")]
        [ReadOnlyInspector]public bool _isLoopComplete;
        [ReadOnlyInspector] public int pingPongTweenId;
        
        [Header("Status.")]
        [ReadOnlyInspector] public AlterableDefinableAttributePreview _currentAttributePreview;
        [ReadOnlyInspector] public int _previewIndex;
        [ReadOnlyInspector] public bool _isInAcceptButton;
        [ReadOnlyInspector] public bool _isNotEnoughVolun;
        [ReadOnlyInspector] public bool _isLackingVolunWhenMenuOpen;
        [ReadOnlyInspector] public bool _hasChangeSpriteToHovering;
        [ReadOnlyInspector] public bool _hasAlteredLevel;

        [Header("Managers Refs.")]
        [ReadOnlyInspector, SerializeField] InputManager _inp;
        [ReadOnlyInspector] public LevelingMenuManager _levelingMenuManager; 
        [NonSerialized] public StatsAttributeHandler _statsHandler;

        [Header("Private.")]
        int _attributePreviewLength;

        #region Tick.
        public void Tick()
        {
            GetCurrentAttributePreviewByInput();
            
            AlterCurrentAttributePreviewByInput();

            SelectAcceptButtonByInput();

            SelectAcceptButtonByCursor();

            _currentAttributePreview.Tick();
        }

        void GetCurrentAttributePreviewByInput()
        {
            if (_inp.menu_down_input)
            {
                _previewIndex++;
                if (_hasAlteredLevel)
                {
                    _previewIndex = _previewIndex > _attributePreviewLength - 1 ? 0 : _previewIndex;
                }
                else
                {
                    _previewIndex = _previewIndex > _attributePreviewLength - 2 ? 0 : _previewIndex;
                }

                SetCurrentAttributePreviewByInput();
            }
            else if (_inp.menu_up_input)
            {
                _previewIndex--;

                if (_hasAlteredLevel)
                {
                    _previewIndex = _previewIndex < 0 ? _attributePreviewLength - 1 : _previewIndex;
                }
                else
                {
                    _previewIndex = _previewIndex < 0 ? _attributePreviewLength - 2 : _previewIndex;
                }

                SetCurrentAttributePreviewByInput();
            }
        }
        
        /// Accept button won't call this function.
        public void GetCur_AlterDefinablePreview_ByCursor(AlterableDefinableAttributePreview _alterDefinablePreview)
        {
            if (_currentAttributePreview != _alterDefinablePreview)
            {
                _currentAttributePreview.OffAttributePreview();
                _currentAttributePreview = _alterDefinablePreview;
                _currentAttributePreview.OnAttributePreview();

                _previewIndex = _currentAttributePreview._alterDefinablePreviewIndex;
                _currentAttributePreview.RedrawDefinitionDetail();

                _levelingMenuManager.previewHub.CheckIsShowingPreviewSelector();
            }
        }

        void SetCurrentAttributePreviewByInput()
        {
            _currentAttributePreview.OffAttributePreview();
            _currentAttributePreview = _attributePreviews[_previewIndex];
            _currentAttributePreview.OnAttributePreview();
        }
        
        void SetLevelingMenuCurrentDefiniable(DefinableAttributePreview _definableAttribute)
        {
            _levelingMenuManager.previewHub.CheckIsShowingPreviewSelector();
            _definableAttribute.RedrawDefinitionDetail();
        }

        void SelectAcceptButtonByInput()
        {
            if (_inp.menu_select_input)
            {
                if (_isInAcceptButton)
                {
                    _levelingMenuManager.SetIsInConfirmingStateToTrue();
                    OnAcceptButtonPressed();
                }
            }
        }

        void SelectAcceptButtonByCursor()
        {
            if (_inp.menu_select_mouse)
            {
                if (_isInAcceptButton)
                {
                    _levelingMenuManager.SetIsInConfirmingStateToTrue();
                    OnAcceptButtonPressed();
                }
            }
        }

        void AlterCurrentAttributePreviewByInput()
        {
            if (_isInAcceptButton)
                return;

            if (_inp.menu_right_input)
            {
                OnIncrementAttribute();
            }
            else if (_inp.menu_left_input)
            {
                OnDecrementAttribute();
            }
        }
        #endregion

        #region On Increment Attribute.
        public void OnIncrementAttribute()
        {
            if (!_isNotEnoughVolun)
            {
                SetHasAlteredPlayerLevelStatus(true);
                _currentAttributePreview.IncrementAttributePreview();
            }
        }
        #endregion

        #region On Decrement Attribute.
        public void OnDecrementAttribute()
        {
            if (!_currentAttributePreview._hasAlterAttributes)
                return;

            _currentAttributePreview.DecrementAttributePreview();
            if (_statsHandler.playerLevel == _statsHandler._prev_playerLevel && _hasAlteredLevel)
            {
                SetHasAlteredPlayerLevelStatus(false);
            }
        }
        #endregion

        #region Reset Hub OnMenuOpen.
        public void ResetHubOnMenuOpen()
        {
            RefreshStatsHandlerPreviews_OnMenuOpen();
            RefreshNextLevelRequiresText_OnMenuOpen();
            SetCurrentAttributePreview_OnMenuOpen();
            RefreshConfirmedAttributePreviews_OnMenuOpen();
        }

        void RefreshStatsHandlerPreviews_OnMenuOpen()
        {
            _statsHandler.OnLevelingMenu_RefreshPreviewAttributes();
        }

        void RefreshNextLevelRequiresText_OnMenuOpen()
        {
            int _nextLevelRequire = _statsHandler.GetNextPreviewLevelupRequireFromTable();
            if (_statsHandler._prev_voluns < _nextLevelRequire)
            {
                SetIsNotEnoughVolunToTrue();
                _isLackingVolunWhenMenuOpen = true;
            }
            else
            {
                _isLackingVolunWhenMenuOpen = false;
            }

            /// + 1 because we draw the next level of the level that we're previewing. e.g. If the previewing level is 1, we draw lvl 2 of volun's requirement.
            _nextLevelRequiresText.text = _nextLevelRequire.ToString();
        }

        void SetCurrentAttributePreview_OnMenuOpen()
        {
            _previewIndex = 0;
            _isInAcceptButton = false;

            _currentAttributePreview = _attributePreviews[_previewIndex];
            _currentAttributePreview.OnAttributePreview();

            SetLevelingMenuCurrentDefiniable(_currentAttributePreview);
        }

        void RefreshConfirmedAttributePreviews_OnMenuOpen()
        {
            for (int i = 0; i < _attributePreviewLength; i++)
            {
                _attributePreviews[i].RedrawConfirmedAttributePreview();
            }

            _playerLevelPreview.RedrawConfirmedAttributePreview();
            _volunPointPreview.RedrawConfirmedAttributePreview();
        }
        #endregion

        #region Reset Hub OnMenuClose.
        public void ResetHubOnMenuClose()
        {
            _currentAttributePreview.ResetOnMenuClose();

            _isNotEnoughVolun = false;
        }
        #endregion

        #region Reset Hub OnLevelup.
        public void ResetHubOnLevelup()
        {
            RefreshNextLevelRequiresText_OnMenuOpen();
            SetCurrentAttributePreview_OnMenuOpen();
            RefreshConfirmedAttributePreviews_OnMenuOpen();
            ResetAttributePreviews_OnLevelup();
        }

        void ResetAttributePreviews_OnLevelup()
        {
            for (int i = 0; i < _attributePreviewLength; i++)
            {
                _attributePreviews[i].ResetOnLevelup();
            }
        }
        #endregion

        #region Volun Require.
        public void SetIsNotEnoughVolunToTrue()
        {
            _isNotEnoughVolun = true;
            _volunPointPreview.ChangeToInsufficientColor();
        }

        public void SetIsNotEnoughVolunToFalse()
        {
            _isNotEnoughVolun = false;
            _volunPointPreview.ChangeBackToNormalColor();
        }
        #endregion

        #region Accept Button.
        void EnableAcceptButton()
        {
            _acceptButtonImage.raycastTarget = true;
            ChangeAcceptSpriteToEnable();
        }

        void DisableAcceptButton()
        {
            _acceptButtonImage.raycastTarget = false;
            ChangeAcceptSpriteToDisable();
        }
        
        public void OnAcceptButton()
        {
            _isInAcceptButton = true;

            if (_isNotEnoughVolun && !_hasAlteredLevel)
                return;

            ChangeAcceptSpriteToHovering();
            _hasChangeSpriteToHovering = true;
        }

        public void OffAcceptButton()
        {
            _isInAcceptButton = false;

            if (_hasChangeSpriteToHovering)
            {
                ChangeAcceptSpriteToEnable();
                _hasChangeSpriteToHovering = false;
            }
        }
        
        void OnAcceptButtonPressed()
        {
        }

        #region Sprite.
        void ChangeAcceptSpriteToEnable()
        {
            _acceptButtonImage.sprite = _enabledButtonSprite;
        }

        void ChangeAcceptSpriteToDisable()
        {
            _acceptButtonImage.sprite = _disabledButtonSprite;
        }

        void ChangeAcceptSpriteToHovering()
        {
            _acceptButtonImage.sprite = _hoveringButtonSprite;
        }
        #endregion

        #endregion

        #region Set Status.
        public void SetHasAlteredPlayerLevelStatus(bool _hasAlteredLevel)
        {
            if (_hasAlteredLevel)
            {
                if (!this._hasAlteredLevel)
                {
                    this._hasAlteredLevel = true;
                    EnableAcceptButton();
                }
            }
            else
            {
                this._hasAlteredLevel = false;
                DisableAcceptButton();
            }
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupReferences();
            SetupAttributePreviews();
            SetupDefinitionHubCurrentDefinable();
        }

        void SetupReferences()
        {
            _levelingMenuManager = LevelingMenuManager.singleton;
            _statsHandler = _levelingMenuManager._inp._states.statsHandler;
            _inp = _levelingMenuManager._inp;
        }
        
        void SetupAttributePreviews()
        {
            _attributePreviewLength = _attributePreviews.Length;
            for (int i = 0; i < _attributePreviewLength; i++)
            {
                _attributePreviews[i].AttributePreviewSetup(this);
                _attributePreviews[i]._alterDefinablePreviewIndex = i;
            }

            _playerLevelPreview.AttributePreviewSetup(this);
            _volunPointPreview.AttributePreviewSetup(this);
        }

        void SetupDefinitionHubCurrentDefinable()
        {
            _levelingMenuManager.definitionHub.SetupDefineDetail(_playerLevelPreview._referedDefineDetail);
        }
        #endregion
    }
}