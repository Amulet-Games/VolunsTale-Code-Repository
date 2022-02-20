using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class QuitScrollElement : BaseScrollElement
    {
        [Header("Drag and Drop Refs.")]
        public QuitSystemDetail _quitSystemDetail;

        public override BaseSystemDetail GetReferedElementDetail()
        {
            return _quitSystemDetail;
        }

        #region Setup.
        public override void Setup(HorizontalScrollHandler _scrollHandler)
        {
            this._scrollHandler = _scrollHandler;

            BaseSetup();
            SetupQuitSystemDetail();
        }

        void SetupQuitSystemDetail()
        {
            _quitSystemDetail.Setup(_scrollHandler._systemMenuManager);
        }
        #endregion
    }
}