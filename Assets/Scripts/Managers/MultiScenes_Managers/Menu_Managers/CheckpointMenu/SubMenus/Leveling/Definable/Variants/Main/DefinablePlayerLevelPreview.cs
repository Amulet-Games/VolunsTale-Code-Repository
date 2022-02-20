using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class DefinablePlayerLevelPreview : DefinableAttributePreview, IPointerEnterHandler
    {
        [Header("Definable Stats.")]
        // Damage Reduction
        public DefinablePhysicalPreview _physicalPreview;
        public DefinableStrikePreview _strikePreview;
        public DefinableThrustPreview _thrustPreview;
        public DefinableSlashPreview _slashPreview;
        public DefinableMagicPreview _magicPreview;
        public DefinableFirePreview _firePreview;
        public DefinableLightningPreview _lightningPreview;
        public DefinableDarkPreview _darkPreview;

        // Status Resistance
        public DefinableBleedPreview _bleedPreview;
        public DefinablePoisonPreview _poisonPreview;
        public DefinableFrostPreview _frostPreview;
        public DefinableCursePreview _cursePreview;

        public override void IncrementAttributePreview()
        {
            _statsHandler._prev_playerLevel++;
            RedrawIncrementPreview();

            int _remainder = (_statsHandler._prev_playerLevel) % 3;
            if (_remainder != 2)
            {
                if (_remainder != 1)
                {
                    IncrementThirdRemainderPreview();
                    RedrawThirdRemainderIncrementPreview();
                }
                else
                {
                    IncrementFirstRemainderPreview();
                    RedrawFirstRemainderIncrementPreview();
                }
            }
            else
            {
                IncrementSecondRemainderPreview();
                RedrawSecondRemainderIncrementPreview();
            }
        }

        public override void DecrementAttributePreview()
        {
            _statsHandler._prev_playerLevel--;
            RedrawDecrementPreview(_statsHandler.playerLevel == _statsHandler._prev_playerLevel);

            /// Only using this func on decrement because getting remainder and decrement value doesn't work.
            ModiflyAffectedStatsPreview();
            RedrawDecrementPreviews();
        }

        #region Increment Preview Stats.
        void IncrementFirstRemainderPreview()
        {
            _statsHandler.Increment_Prev_phyReduct();
        }

        void IncrementSecondRemainderPreview()
        {
            _statsHandler.Increment_Prev_magicReduct();
            _statsHandler.Increment_Prev_fireReduct();
            _statsHandler.Increment_Prev_lightningReduct();
        }

        void IncrementThirdRemainderPreview()
        {
            _statsHandler.Increment_Prev_darkReduct();
            _statsHandler.Increment_Prev_bleedResis();
            _statsHandler.Increment_Prev_poisonResis();
            _statsHandler.Increment_Prev_frostResis();
            _statsHandler.Increment_Prev_curseResis();
        }
        #endregion

        #region Decrement Preview Stats.
        void ModiflyAffectedStatsPreview()
        {
            _statsHandler.ModiflyLevelAffectedStatsPreview();
        }
        #endregion

        #region Redraw Increment Preview Stats.
        void RedrawFirstRemainderIncrementPreview()
        {
            _physicalPreview.RedrawIncrementPreview();
            _strikePreview.RedrawIncrementPreview();
            _thrustPreview.RedrawIncrementPreview();
            _slashPreview.RedrawIncrementPreview();
        }

        void RedrawSecondRemainderIncrementPreview()
        {
            _magicPreview.RedrawIncrementPreview();
            _firePreview.RedrawIncrementPreview();
            _lightningPreview.RedrawIncrementPreview();
        }

        void RedrawThirdRemainderIncrementPreview()
        {
            _darkPreview.RedrawIncrementPreview();
            _bleedPreview.RedrawIncrementPreview();
            _poisonPreview.RedrawIncrementPreview();
            _frostPreview.RedrawIncrementPreview();
            _cursePreview.RedrawIncrementPreview();
        }
        #endregion

        #region Redraw Decrement Preview Stats.
        void RedrawDecrementPreviews()
        {
            if (_statsHandler._prev_physical_reduction - _statsHandler.b_physical_reduction < 1)
            {
                _physicalPreview.RedrawDecrementPreview();
                _strikePreview.RedrawDecrementPreview();
                _thrustPreview.RedrawDecrementPreview();
                _slashPreview.RedrawDecrementPreview();
            }

            _magicPreview.RedrawDecrementPreview();
            _firePreview.RedrawDecrementPreview();
            _lightningPreview.RedrawDecrementPreview();

            _darkPreview.RedrawDecrementPreview();
            _bleedPreview.RedrawDecrementPreview();
            _poisonPreview.RedrawDecrementPreview();
            _frostPreview.RedrawDecrementPreview();
            _cursePreview.RedrawDecrementPreview();
        }
        #endregion

        #region On Pointer Enter.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _levelingHub._levelingMenuManager.previewHub.CheckIsShowingPreviewSelector();
            RedrawDefinitionDetail();
        }
        #endregion

        protected override void RedrawIncrementPreview()
        {
            _afterChangesText.text = _statsHandler._prev_playerLevel.ToString();
            _afterChangesText.color = LevelingMenuManager.singleton._afterChangesFontColor;
        }

        protected override void RedrawDecrementPreview(bool _hasBackToInitialValue)
        {
            _afterChangesText.text = _statsHandler._prev_playerLevel.ToString();

            if (_hasBackToInitialValue)
                _afterChangesText.color = LevelingMenuManager.singleton._beforeChangesFontColor;
        }

        public override void RedrawConfirmedAttributePreview()
        {
            _afterChangesText.text = _statsHandler.playerLevel.ToString();
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
        }
    }
}