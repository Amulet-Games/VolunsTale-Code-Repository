using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_P_BFX_Handler_4S : AI_Poolable_BFX_Handler
    {
        [Header("Scrollers (Drops).")]
        public AI_BFX_Scroller _1st_Scroller;
        public AI_BFX_Scroller _2nd_Scroller;
        public AI_BFX_Scroller _3rd_Scroller;

        [Space(10)]
        public AI_BFX_Scroller _4th_Scroller;

        #region Setup.
        public override void Setup(AI_BFX_HandlerPool _pool)
        {
            _referedPool = _pool;

            SetupMaterialPropertyID();

            SetupScrollers();

            SetupRefs();

            void SetupMaterialPropertyID()
            {
                int _mainTextId = Shader.PropertyToID("_MainTex");

                _1st_Scroller._mainTextId = _mainTextId;
                _2nd_Scroller._mainTextId = _mainTextId;
                _3rd_Scroller._mainTextId = _mainTextId;
                _4th_Scroller._mainTextId = _mainTextId;
            }

            void SetupScrollers()
            {
                _1st_Scroller.Setup();
                _2nd_Scroller.Setup();
                _3rd_Scroller.Setup();
                _4th_Scroller.Setup();

                _4th_Scroller.isLastUpdatable = true;
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
        }

        void PrepareScrollers()
        {
            _1st_Scroller.OnScrollerStart();
            _2nd_Scroller.OnScrollerStart();
            _3rd_Scroller.OnScrollerStart();
            _4th_Scroller.OnScrollerStart();
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
        }
        #endregion

        #region On End.
        public override void End_AI_Bfx()
        {
            RemoveFromActiveList();
            ReturnToPool();
        }
        #endregion
    }
}