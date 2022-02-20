﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableStrengthPreview : AlterableDefinableAttributePreview, IPointerEnterHandler
    {
        [Header("R_Weapons Definable Stats.")]
        public DefinableRWeapon1Preview _rhWeapon1Preview;
        public DefinableRWeapon2Preview _rhWeapon2Preview;
        public DefinableRWeapon3Preview _rhWeapon3Preview;

        [Header("L_Weapons Definable Stats.")]
        public DefinableLWeapon1Preview _lhWeapon1Preview;
        public DefinableLWeapon2Preview _lhWeapon2Preview;
        public DefinableLWeapon3Preview _lhWeapon3Preview;

        [Header("Fire Reduct Definable Stats.")]
        public DefinableFirePreview _firePreview;

        #region On / Off / Tick Attribute Preview.
        public override void Tick()
        {
            HighlighterTick();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                // R Weapons.
                _rhWeapon1Preview.HighlighterTick();
                _rhWeapon2Preview.HighlighterTick();
                _rhWeapon3Preview.HighlighterTick();

                // L Weapons.
                _lhWeapon1Preview.HighlighterTick();
                _lhWeapon2Preview.HighlighterTick();
                _lhWeapon3Preview.HighlighterTick();

                // Fire Reducts.
                _firePreview.HighlighterTick();
            }
        }

        public override void OnAttributePreview()
        {
            OnHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OnBothButtons();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                // R Weapons.
                _rhWeapon1Preview.OnHighlighter();
                _rhWeapon2Preview.OnHighlighter();
                _rhWeapon3Preview.OnHighlighter();

                // L Weapons.
                _lhWeapon1Preview.OnHighlighter();
                _lhWeapon2Preview.OnHighlighter();
                _lhWeapon3Preview.OnHighlighter();

                // Fire Reducts.
                _firePreview.OnHighlighter();
            }
        }

        public override void OffAttributePreview()
        {
            OffHighlighter();
            CancelHighlighter();

            if (!_levelingHub._isLackingVolunWhenMenuOpen)
                OffBothButtons();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                // R Weapons.
                _rhWeapon1Preview.OffHighlighter();
                _rhWeapon2Preview.OffHighlighter();
                _rhWeapon3Preview.OffHighlighter();

                // L Weapons.
                _lhWeapon1Preview.OffHighlighter();
                _lhWeapon2Preview.OffHighlighter();
                _lhWeapon3Preview.OffHighlighter();

                // Fire Reducts.
                _firePreview.OffHighlighter();
            }
        }
        #endregion

        #region Reset On Menu Close.
        public override void ResetOnMenuClose()
        {
            BaseResetOnMenuClose();
            ReverseAttributeTextSize();

            if (LevelingMenuManager.singleton.isSwitchPreviewGroup)
            {
                // R Weapons.
                _rhWeapon1Preview.ResetOnMenuClose();
                _rhWeapon2Preview.ResetOnMenuClose();
                _rhWeapon3Preview.ResetOnMenuClose();

                // L Weapons.
                _lhWeapon1Preview.ResetOnMenuClose();
                _lhWeapon2Preview.ResetOnMenuClose();
                _lhWeapon3Preview.ResetOnMenuClose();

                // Fire Reducts.
                _firePreview.ResetOnMenuClose();
            }
        }
        #endregion

        #region Reset On Preview Group Switch.
        public override void ResetOnPreviewGroupSwitch(bool isSwitchPreviewGroup)
        {
            if (isSwitchPreviewGroup)
            {
                // R Weapons.
                _rhWeapon1Preview.OnHighlighter();
                _rhWeapon2Preview.OnHighlighter();
                _rhWeapon3Preview.OnHighlighter();

                // L Weapons.
                _lhWeapon1Preview.OnHighlighter();
                _lhWeapon2Preview.OnHighlighter();
                _lhWeapon3Preview.OnHighlighter();

                // Fire Reducts.
                _firePreview.OnHighlighter();
            }
            else
            {
                // R Weapons.
                _rhWeapon1Preview.ResetOnMenuClose();
                _rhWeapon2Preview.ResetOnMenuClose();
                _rhWeapon3Preview.ResetOnMenuClose();

                // L Weapons.
                _lhWeapon1Preview.ResetOnMenuClose();
                _lhWeapon2Preview.ResetOnMenuClose();
                _lhWeapon3Preview.ResetOnMenuClose();

                // Fire Reducts.
                _firePreview.ResetOnMenuClose();
            }
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _levelingHub.GetCur_AlterDefinablePreview_ByCursor(this);
        }
        #endregion

        public override void IncrementAttributePreview()
        {
            _volunPreview.IncrementAttributePreview();

            _statsHandler._prev_strength++;

            RedrawIncrementPreview();
            RedrawIncrementButton();
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_FireReduct();
            if (_statsHandler._prev_fire_reduction - _statsHandler.b_fire_reduction > 0.5f)
            {
                _firePreview.RedrawIncrementPreview();
            }
            
            _rhWeapon1Preview.RedrawIncrementPreview();
            _rhWeapon2Preview.RedrawIncrementPreview();
            _rhWeapon3Preview.RedrawIncrementPreview();

            _lhWeapon1Preview.RedrawIncrementPreview();
            _lhWeapon2Preview.RedrawIncrementPreview();
            _lhWeapon3Preview.RedrawIncrementPreview();

            _playerLevelPreview.IncrementAttributePreview();
        }

        public override void DecrementAttributePreview()
        {
            _volunPreview.DecrementAttributePreview();

            _statsHandler._prev_strength--;

            bool _hasBackToInitialValue = _statsHandler.strength == _statsHandler._prev_strength ? true : false;
            RedrawDecrementPreview(_hasBackToInitialValue);
            RedrawDecrementButton(_hasBackToInitialValue);
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_FireReduct();
            _firePreview.RedrawDecrementPreview();

            _rhWeapon1Preview.RedrawDecrementPreview();
            _rhWeapon2Preview.RedrawDecrementPreview();
            _rhWeapon3Preview.RedrawDecrementPreview();

            _lhWeapon1Preview.RedrawDecrementPreview();
            _lhWeapon2Preview.RedrawDecrementPreview();
            _lhWeapon3Preview.RedrawDecrementPreview();

            _playerLevelPreview.DecrementAttributePreview();
        }

        protected override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_strength.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            _afterChangesText.text = _statsHandler._prev_strength.ToString();

            if (_hasBackToInitialValue)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            string _strengthText = _statsHandler.strength.ToString();

            _beforeChangesText.text = _strengthText;
            _afterChangesText.text = _strengthText;
            _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawDefinitionDetail()
        {
            LevelingMenuManager.singleton.definitionHub.OnDefineDetail(_referedDefineDetail);
        }

        public override void AttributePreviewSetup(LevelingHub _levelingHub)
        {
            this._levelingHub = _levelingHub;
            _statsHandler = _levelingHub._statsHandler;
            AlterableSetup();
        }
    }
}