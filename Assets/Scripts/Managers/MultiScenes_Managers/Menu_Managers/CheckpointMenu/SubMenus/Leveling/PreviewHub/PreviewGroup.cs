using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA
{
    public class PreviewGroup : MonoBehaviour
    {
        [Header("Alters Stats.")]
        public DefinableStatsPreview[] _statsPreviews;
        
        [Header("Manager Refs.")]
        [ReadOnlyInspector] public PreviewHub _previewHub;

        [Header("Private.")]
        int _statsPreviewLength;

        #region Redraw Confirmed Stats Preview.
        public void RedrawConfirmedStatsPreview()
        {
            for (int i = 0; i < _statsPreviewLength; i++)
            {
                _statsPreviews[i].RedrawConfirmedStatsPreview();
            }
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            LevelingMenuManager _levelingMenuManager = LevelingMenuManager.singleton;
            StatsAttributeHandler _statsHandler = _levelingMenuManager._inp._states.statsHandler;
            _previewHub = _levelingMenuManager.previewHub;

            _statsPreviewLength = _statsPreviews.Length;
            for (int i = 0; i < _statsPreviewLength; i++)
            {
                _statsPreviews[i].StatsPreviewSetup(_statsHandler, _previewHub);
            }
        }
        #endregion
    }
}