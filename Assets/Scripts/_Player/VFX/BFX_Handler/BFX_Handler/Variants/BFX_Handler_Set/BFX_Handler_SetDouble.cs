using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BFX_Handler_SetDouble : BFX_Handler_SetBase
    {
        [Header("1sts. Drag and Drops.")]
        public BFX_DecalEmitter decalEmitter;
        public BFX_DecalUpdater decalUpdater;

        [Header("2nds. Drag and Drops.")]
        public BFX_BloodFxUpdater bloodUpdater_2nd;
        public BFX_DecalEmitter decalEmitter_2nd;
        public BFX_DecalUpdater decalUpdater_2nd;

        #region Init.
        public override void Init(BFX_HandlerPool _bfxHandlerPool)
        {
            handlerPool = _bfxHandlerPool;

            InitRefs();
            InitPropertyIDs();

            Init_Double_BloodUpdater();
            Init_Double_DecalEmitter();
            Init_Double_DecalUpdater();
        }

        void InitPropertyIDs()
        {
            _UseCustomTimeId = gameManager._UseCustomTimeId;
            _TimeInFramesId = gameManager._TimeInFramesId;
            _LightIntencityId = gameManager._LightIntencityId;

            cutoutPropertyID = gameManager.cutoutPropertyID;
            forwardDirPropertyID = gameManager.forwardDirPropertyID;
        }

        void Init_Double_BloodUpdater()
        {
            GroupInitRefs();

            GroupExecuteInit();

            void GroupInitRefs()
            {
                bloodUpdater._bfxHandler = this;
                bloodUpdater_2nd._bfxHandler = this;
            }

            void GroupExecuteInit()
            {
                bloodUpdater.Init();
                bloodUpdater_2nd.Init();
            }
        }

        void Init_Double_DecalEmitter()
        {
            decalEmitter._bfxHandler = this;
            decalEmitter_2nd._bfxHandler = this;

            decalEmitter.Init();
            decalEmitter_2nd.Init();
        }

        void Init_Double_DecalUpdater()
        {
            GroupInitRefs();

            GroupExecuteInit();

            void GroupInitRefs()
            {
                decalUpdater._bfxHandler = this;
                decalUpdater_2nd._bfxHandler = this;

                decalUpdater_2nd._isLastDecalInSet = true;
            }

            void GroupExecuteInit()
            {
                decalUpdater.Init();
                decalUpdater_2nd.Init();
            }
        }
        #endregion

        #region On Start.
        public override void On_BFX_Start()
        {
            gameObject.SetActive(true);

            bloodUpdater.OnBloodStart();
            bloodUpdater_2nd.OnBloodStart();

            decalEmitter.OnDecalStart();
            decalEmitter_2nd.OnDecalStart();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            UpdateDeltas();

            BloodUpdater_Double_Tick();

            DecalUpdater_Double_Tick();
        }

        void BloodUpdater_Double_Tick()
        {
            bloodUpdater.Tick();
            bloodUpdater_2nd.Tick();
        }

        void DecalUpdater_Double_Tick()
        {
            decalUpdater.Tick();
            decalUpdater_2nd.Tick();
        }
        #endregion
    }
}