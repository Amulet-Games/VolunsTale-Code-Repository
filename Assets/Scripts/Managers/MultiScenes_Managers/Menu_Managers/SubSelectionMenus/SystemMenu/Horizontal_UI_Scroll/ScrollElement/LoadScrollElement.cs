using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LoadScrollElement : BaseScrollElement
    {
        [Header("Drag and Drop Refs.")]
        public LoadSystemDetail _loadSystemDetail;

        public override BaseSystemDetail GetReferedElementDetail()
        {
            return _loadSystemDetail;
        }

        #region Setup.
        public override void Setup(HorizontalScrollHandler _scrollHandler)
        {
            this._scrollHandler = _scrollHandler;

            BaseSetup();
            SetupLoadSystemDetail();
        }

        void SetupLoadSystemDetail()
        {
            _loadSystemDetail.Setup(_scrollHandler._systemMenuManager);
        }
        #endregion
    }
}