using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{ 
    public class BFX_Handler_SetSingle : BFX_Handler_SetBase
    {
        [Header("1sts. Drag and Drops.")]
        public BFX_DecalEmitter decalEmitter;
        public BFX_DecalUpdater decalUpdater;

        #region Init.
        public override void Init(BFX_HandlerPool _bfxHandlerPool)
        {
            handlerPool = _bfxHandlerPool;

            InitRefs();
            InitPropertyIDs();

            Init_Single_BloodUpdater();
            Init_Single_DecalEmitter();
            Init_Single_DecalUpdater();
        }

        void InitPropertyIDs()
        {
            _UseCustomTimeId = gameManager._UseCustomTimeId;
            _TimeInFramesId = gameManager._TimeInFramesId;
            _LightIntencityId = gameManager._LightIntencityId;

            cutoutPropertyID = gameManager.cutoutPropertyID;
            forwardDirPropertyID = gameManager.forwardDirPropertyID;
        }

        void Init_Single_BloodUpdater()
        {
            InitRefs();
            
            ExecuteInit();

            void InitRefs()
            {
                bloodUpdater._bfxHandler = this;
            }
            
            void ExecuteInit()
            {
                bloodUpdater.Init();
            }
        }

        void Init_Single_DecalEmitter()
        {
            decalEmitter._bfxHandler = this;
            decalEmitter.Init();
        }

        void Init_Single_DecalUpdater()
        {
            InitRef();
            
            ExecuteInit();

            void InitRef()
            {
                decalUpdater._bfxHandler = this;
                decalUpdater._isLastDecalInSet = true;
            }
            
            void ExecuteInit()
            {
                decalUpdater.Init();
            }
        }
        #endregion

        #region On Start.
        public override void On_BFX_Start()
        {
            gameObject.SetActive(true);

            bloodUpdater.OnBloodStart();
            decalEmitter.OnDecalStart();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            UpdateDeltas();

            BloodUpdater_Single_Tick();

            DecalUpdater_Single_Tick();
        }

        void BloodUpdater_Single_Tick()
        {
            bloodUpdater.Tick();
        }

        void DecalUpdater_Single_Tick()
        {
            decalUpdater.Tick();
        }
        #endregion
    }
}