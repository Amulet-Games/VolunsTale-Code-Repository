using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinableIntelligencePreview : AlterableDefinableAttributePreview, IPointerEnterHandler
    {
        [Header("R_Weapons Definable Stats.")]
        public DefinableRWeapon1Preview _rhWeapon1Preview;
        public DefinableRWeapon2Preview _rhWeapon2Preview;
        public DefinableRWeapon3Preview _rhWeapon3Preview;

        [Header("L_Weapons Definable Stats.")]
        public DefinableLWeapon1Preview _lhWeapon1Preview;
        public DefinableLWeapon2Preview _lhWeapon2Preview;
        public DefinableLWeapon3Preview _lhWeapon3Preview;

        [Header("Magic Reduct Definable Stats.")]
        public DefinableMagicPreview _magicPreview;

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

                // Magic Reducts.
                _magicPreview.HighlighterTick();
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

                // Magic Reducts.
                _magicPreview.OnHighlighter();
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

                // Magic Reducts.
                _magicPreview.OffHighlighter();
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

                // Magic Reducts.
                _magicPreview.ResetOnMenuClose();
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

                // Magic Reducts.
                _magicPreview.OnHighlighter();
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

                // Magic Reducts.
                _magicPreview.ResetOnMenuClose();
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

            _statsHandler._prev_intelligence++;

            RedrawIncrementPreview();
            RedrawIncrementButton();
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_MagicReduct();
            if (_statsHandler._prev_magic_reduction - _statsHandler.b_magic_reduction > 0.5f)
            {
                _magicPreview.RedrawIncrementPreview();
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

            _statsHandler._prev_intelligence--;

            bool _hasBackToInitialValue = _statsHandler.intelligence == _statsHandler._prev_intelligence ? true : false;
            RedrawDecrementPreview(_hasBackToInitialValue);
            RedrawDecrementButton(_hasBackToInitialValue);
            PingPongAttributeTextSize();

            _statsHandler.Modifly_Prev_MagicReduct();
            _magicPreview.RedrawDecrementPreview();

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
            _afterChangesText.text = _statsHandler._prev_intelligence.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            _afterChangesText.text = _statsHandler._prev_intelligence.ToString();

            if (_hasBackToInitialValue)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            string _intelligenceText = _statsHandler.intelligence.ToString();

            _beforeChangesText.text = _intelligenceText;
            _afterChangesText.text = _intelligenceText;
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