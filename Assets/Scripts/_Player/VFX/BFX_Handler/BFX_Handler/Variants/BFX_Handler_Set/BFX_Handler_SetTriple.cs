using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BFX_Handler_SetTriple : BFX_Handler_SetBase
    {
        [Header("1sts. Drag and Drops.")]
        public BFX_DecalEmitter decalEmitter;
        public BFX_DecalUpdater decalUpdater;

        [Header("2nds. Drag and Drops.")]
        public BFX_BloodFxUpdater bloodUpdater_2nd;
        public BFX_DecalEmitter decalEmitter_2nd;
        public BFX_DecalUpdater decalUpdater_2nd;

        [Header("3rds. Drag and Drops.")]
        public BFX_BloodFxUpdater bloodUpdater_3rd;
        public BFX_DecalEmitter decalEmitter_3rd;
        public BFX_DecalUpdater decalUpdater_3rd;

        #region Init.
        public override void Init(BFX_HandlerPool _bfxHandlerPool)
        {
            handlerPool = _bfxHandlerPool;

            InitRefs();
            InitPropertyIDs();

            Init_Triple_BloodUpdater();
            Init_Triple_DecalEmitter();
            Init_Triple_DecalUpdater();
        }

        void InitPropertyIDs()
        {
            _UseCustomTimeId = gameManager._UseCustomTimeId;
            _TimeInFramesId = gameManager._TimeInFramesId;
            _LightIntencityId = gameManager._LightIntencityId;

            cutoutPropertyID = gameManager.cutoutPropertyID;
            forwardDirPropertyID = gameManager.forwardDirPropertyID;
        }

        void Init_Triple_BloodUpdater()
        {
            GroupInitRefs();
            
            GroupExecuteInit();
            
            void GroupInitRefs()
            {
                bloodUpdater._bfxHandler = this;
                bloodUpdater_2nd._bfxHandler = this;
                bloodUpdater_3rd._bfxHandler = this;
            }
            
            void GroupExecuteInit()
            {
                bloodUpdater.Init();
                bloodUpdater_2nd.Init();
                bloodUpdater_3rd.Init();
            }
        }

        void Init_Triple_DecalEmitter()
        {
            decalEmitter._bfxHandler = this;
            decalEmitter_2nd._bfxHandler = this;
            decalEmitter_3rd._bfxHandler = this;

            decalEmitter.Init();
            decalEmitter_2nd.Init();
            decalEmitter_3rd.Init();
        }

        void Init_Triple_DecalUpdater()
        {
            GroupInitRefs();
            
            GroupExecuteInit();

            void GroupInitRefs()
            {
                decalUpdater._bfxHandler = this;
                decalUpdater_2nd._bfxHandler = this;
                decalUpdater_3rd._bfxHandler = this;

                decalUpdater_3rd._isLastDecalInSet = true;
            }
            
            void GroupExecuteInit()
            {
                decalUpdater.Init();
                decalUpdater_2nd.Init();
                decalUpdater_3rd.Init();
            }
        }
        #endregion

        #region On Start.
        public override void On_BFX_Start()
        {
            gameObject.SetActive(true);

            bloodUpdater.OnBloodStart();
            bloodUpdater_2nd.OnBloodStart();
            bloodUpdater_3rd.OnBloodStart();

            decalEmitter.OnDecalStart();
            decalEmitter_2nd.OnDecalStart();
            decalEmitter_3rd.OnDecalStart();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            UpdateDeltas();

            BloodUpdater_Triple_Tick();

            DecalUpdater_Triple_Tick();
        }

        void BloodUpdater_Triple_Tick()
        {
            bloodUpdater.Tick();
            bloodUpdater_2nd.Tick();
            bloodUpdater_3rd.Tick();
        }

        void DecalUpdater_Triple_Tick()
        {
            decalUpdater.Tick();
            decalUpdater_2nd.Tick();
            decalUpdater_3rd.Tick();
        }
        #endregion
    }
}