using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GraphicScrollElement : BaseScrollElement
    {
        [Header("Drag and Drop Refs.")]
        public GraphicSystemDetail _graphicSystemDetail;

        public override BaseSystemDetail GetReferedElementDetail()
        {
            return _graphicSystemDetail;
        }

        #region Setup.
        public override void Setup(HorizontalScrollHandler _scrollHandler)
        {
            this._scrollHandler = _scrollHandler;

            BaseSetup();
            SetupGraphicSystemDetail();
        }

        void SetupGraphicSystemDetail()
        {
            _graphicSystemDetail.Setup(_scrollHandler._systemMenuManager);
        }
        #endregion
    }
}