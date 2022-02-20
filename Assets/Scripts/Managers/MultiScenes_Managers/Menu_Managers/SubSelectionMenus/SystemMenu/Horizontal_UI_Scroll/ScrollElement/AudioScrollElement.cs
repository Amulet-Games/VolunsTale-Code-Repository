using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AudioScrollElement : BaseScrollElement
    {
        [Header("Drag and Drop Refs.")]
        public AudioSystemDetail _audioSystemDetail;

        public override BaseSystemDetail GetReferedElementDetail()
        {
            return _audioSystemDetail;
        }

        #region Setup.
        public override void Setup(HorizontalScrollHandler _scrollHandler)
        {
            this._scrollHandler = _scrollHandler;

            BaseSetup();
            SetupAudioSystemDetail();
        }

        void SetupAudioSystemDetail()
        {
            _audioSystemDetail.Setup(_scrollHandler._systemMenuManager);
        }
        #endregion
    }
}