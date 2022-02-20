using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public abstract class BaseDefinablePreview : MonoBehaviour
    {
        [Header("Config.")]
        public Canvas _referedDefineDetail;
        [SerializeField] protected Text _beforeChangesText;
        [SerializeField] protected Text _afterChangesText;
        
        [Header("Manager Refs.")]
        [NonSerialized] public StatsAttributeHandler _statsHandler;

        #region Init.
        public void BaseSetup(StatsAttributeHandler _statsHandler)
        {
            this._statsHandler = _statsHandler;
        }
        #endregion

        #region Redraws.
        public abstract void RedrawDefinitionDetail();
        #endregion
    }
}