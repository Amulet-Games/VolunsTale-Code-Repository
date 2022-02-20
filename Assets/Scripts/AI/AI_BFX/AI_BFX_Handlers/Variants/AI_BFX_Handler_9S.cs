﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_BFX_Handler_9S : AI_BFX_Handler
    {
        [Header("Scrollers (Drops).")]
        public AI_BFX_Scroller _1st_Scroller;
        public AI_BFX_Scroller _3rd_Scroller;
        public AI_BFX_Scroller _4th_Scroller;

        [Header("Delayables (Drops).")]
        public AI_BFX_Scroller_Delayable _2nd_Scroller_delayable;
        public AI_BFX_Scroller_Delayable _5th_Scroller_delayable;
        public AI_BFX_Scroller_Delayable _6th_Scroller_delayable;
        public AI_BFX_Scroller_Delayable _7th_Scroller_delayable;
        public AI_BFX_Scroller_Delayable _8th_Scroller_delayable;

        [Space(10)]
        public AI_BFX_Scroller_Delayable _9th_Scroller_delayable;

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
                _2nd_Scroller_delayable._mainTextId = _mainTextId;
                _3rd_Scroller._mainTextId = _mainTextId;
                _4th_Scroller._mainTextId = _mainTextId;
                _5th_Scroller_delayable._mainTextId = _mainTextId;
                _6th_Scroller_delayable._mainTextId = _mainTextId;
                _7th_Scroller_delayable._mainTextId = _mainTextId;
                _8th_Scroller_delayable._mainTextId = _mainTextId;
                _9th_Scroller_delayable._mainTextId = _mainTextId;
            }

            void SetupScrollers()
            {
                _1st_Scroller.Setup();
                _2nd_Scroller_delayable.Setup();
                _3rd_Scroller.Setup();
                _4th_Scroller.Setup();
                _5th_Scroller_delayable.Setup();
                _6th_Scroller_delayable.Setup();
                _7th_Scroller_delayable.Setup();
                _8th_Scroller_delayable.Setup();
                _9th_Scroller_delayable.Setup();

                _9th_Scroller_delayable.isLastUpdatable = true;
            }
        }
        #endregion

        #region On Start.
        public override void Start_AI_Bfx()
        {
            PrepareScrollers();
            PrepareDelayedScrollers();
            ActivateHandler();
            AddToActiveList();
            SetScrollersAnimStartTime();
        }

        void PrepareScrollers()
        {
            _1st_Scroller.OnScrollerStart();
            _3rd_Scroller.OnScrollerStart();
            _4th_Scroller.OnScrollerStart();
        }

        void PrepareDelayedScrollers()
        {
            _2nd_Scroller_delayable.StartScrollerDelayCount();
            _5th_Scroller_delayable.StartScrollerDelayCount();
            _6th_Scroller_delayable.StartScrollerDelayCount();
            _7th_Scroller_delayable.StartScrollerDelayCount();
            _8th_Scroller_delayable.StartScrollerDelayCount();
            _9th_Scroller_delayable.StartScrollerDelayCount();
        }

        void ActivateHandler()
        {
            gameObject.SetActive(true);
        }

        void SetScrollersAnimStartTime()
        {
            float _time = Time.time;
            _1st_Scroller.animationStartTime = _time;
            _3rd_Scroller.animationStartTime = _time;
            _4th_Scroller.animationStartTime = _time;
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
            _2nd_Scroller_delayable.Tick();
            _3rd_Scroller.Tick();
            _4th_Scroller.Tick();
            _5th_Scroller_delayable.Tick();
            _6th_Scroller_delayable.Tick();
            _7th_Scroller_delayable.Tick();
            _8th_Scroller_delayable.Tick();
            _9th_Scroller_delayable.Tick();
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