using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_BFX_Handler_11S : AI_BFX_Handler
    {
        [Header("Scrollers (Drops).")]
        public AI_BFX_Scroller _1st_Scroller;
        public AI_BFX_Scroller _2nd_Scroller;
        public AI_BFX_Scroller _3rd_Scroller;
        public AI_BFX_Scroller _4th_Scroller;
        public AI_BFX_Scroller _5th_Scroller;
        public AI_BFX_Scroller _6th_Scroller;
        public AI_BFX_Scroller _7th_Scroller;
        public AI_BFX_Scroller _8th_Scroller;
        public AI_BFX_Scroller _9th_Scroller;
        public AI_BFX_Scroller _10th_Scroller;

        [Space(10)]
        public AI_BFX_Scroller _11th_Scroller;

        #region Setup.
        public override void Setup(AISessionManager _aiSessionManager)
        {
            aiSessionManager = _aiSessionManager;

            SetupMaterialPropertyID();

            SetupScrollers();
            
            void SetupMaterialPropertyID()
            {
                int _mainTextId = Shader.PropertyToID("_MainTex");

                _1st_Scroller._mainTextId = _mainTextId;
                _2nd_Scroller._mainTextId = _mainTextId;
                _3rd_Scroller._mainTextId = _mainTextId;
                _4th_Scroller._mainTextId = _mainTextId;
                _5th_Scroller._mainTextId = _mainTextId;
                _6th_Scroller._mainTextId = _mainTextId;
                _7th_Scroller._mainTextId = _mainTextId;
                _8th_Scroller._mainTextId = _mainTextId;
                _9th_Scroller._mainTextId = _mainTextId;
                _10th_Scroller._mainTextId = _mainTextId;
                _11th_Scroller._mainTextId = _mainTextId;
            }

            void SetupScrollers()
            {
                _1st_Scroller.Setup();
                _2nd_Scroller.Setup();
                _3rd_Scroller.Setup();
                _4th_Scroller.Setup();
                _5th_Scroller.Setup();
                _6th_Scroller.Setup();
                _7th_Scroller.Setup();
                _8th_Scroller.Setup();
                _9th_Scroller.Setup();
                _10th_Scroller.Setup();
                _11th_Scroller.Setup();

                _11th_Scroller.isLastUpdatable = true;
            }
        }
        #endregion

        #region On Start.
        public override void Start_AI_Bfx()
        {
            PrepareScrollers();
            ActivateHandler();
            AddToActiveList();
            SetScrollersAnimStartTime();
        }

        void SetScrollersAnimStartTime()
        {
            float _time = Time.time;
            _1st_Scroller.animationStartTime = _time;
            _2nd_Scroller.animationStartTime = _time;
            _3rd_Scroller.animationStartTime = _time;
            _4th_Scroller.animationStartTime = _time;
            _5th_Scroller.animationStartTime = _time;
            _6th_Scroller.animationStartTime = _time;
            _7th_Scroller.animationStartTime = _time;
            _8th_Scroller.animationStartTime = _time;
            _9th_Scroller.animationStartTime = _time;
            _10th_Scroller.animationStartTime = _time;
            _11th_Scroller.animationStartTime = _time;
        }

        void PrepareScrollers()
        {
            _1st_Scroller.OnScrollerStart();
            _2nd_Scroller.OnScrollerStart();
            _3rd_Scroller.OnScrollerStart();
            _4th_Scroller.OnScrollerStart();
            _5th_Scroller.OnScrollerStart();
            _6th_Scroller.OnScrollerStart();
            _7th_Scroller.OnScrollerStart();
            _8th_Scroller.OnScrollerStart();
            _9th_Scroller.OnScrollerStart();
            _10th_Scroller.OnScrollerStart();
            _11th_Scroller.OnScrollerStart();
        }

        void ActivateHandler()
        {
            gameObject.SetActive(true);
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            UpdateScrollers();
        }

        void UpdateScrollers()
        {
            _1st_Scroller.Tick();
            _2nd_Scroller.Tick();
            _3rd_Scroller.Tick();
            _4th_Scroller.Tick();
            _5th_Scroller.Tick();
            _6th_Scroller.Tick();
            _7th_Scroller.Tick();
            _8th_Scroller.Tick();
            _9th_Scroller.Tick();
            _10th_Scroller.Tick();
            _11th_Scroller.Tick();
        }
        #endregion

        #region On End.
        public override void End_AI_Bfx()
        {
            RemoveFromActiveList();
            ReturnToBackpack();
        }
        #endregion
    }
}