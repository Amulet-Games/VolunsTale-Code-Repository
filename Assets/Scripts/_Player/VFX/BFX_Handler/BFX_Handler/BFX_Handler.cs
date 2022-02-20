using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BFX_Handler : MonoBehaviour
    {
        [Header("Main Settings.")]
        public float _lightIntensityMultiplier = 1;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public BFX_HandlerPool handlerPool;
        [ReadOnlyInspector] public StateManager playerStates;
        [ReadOnlyInspector] public GameManager gameManager;

        [Header("Drag and Drops.")]
        public BFX_BloodFxUpdater bloodUpdater;

        #region Private.
        [HideInInspector] public float _delta;

        /// BloodFxUpdater Propery IDs
        [HideInInspector] public int _UseCustomTimeId;
        [HideInInspector] public int _TimeInFramesId;
        [HideInInspector] public int _LightIntencityId;
        #endregion

        #region Init.
        public virtual void Init(BFX_HandlerPool _bfxHandlerPool)
        {
            handlerPool = _bfxHandlerPool;

            InitRefs();
            InitPropertyIDs();

            InitBloodUpdater();
        }
        
        protected void InitRefs()
        {
            playerStates = SessionManager.singleton._states;
            gameManager = GameManager.singleton;
        }

        void InitPropertyIDs()
        {
            _UseCustomTimeId = gameManager._UseCustomTimeId;
            _TimeInFramesId = gameManager._TimeInFramesId;
            _LightIntencityId = gameManager._LightIntencityId;
        }

        void InitBloodUpdater()
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
        #endregion

        #region On Start.
        public virtual void On_BFX_Start()
        {
            gameObject.SetActive(true);

            bloodUpdater.OnBloodStart();
        }
        #endregion

        #region Tick.
        public virtual void Tick()
        {
            UpdateDeltas();

            BloodUpdater_Tick();
        }

        protected void UpdateDeltas()
        {
            _delta = playerStates._delta;
        }
        
        void BloodUpdater_Tick()
        {
            bloodUpdater.Tick();
        }
        #endregion

        #region On Stop.
        public void On_BFX_Stop()
        {
            playerStates.RemoveBfxHandlerFromInProcessList(this);
            handlerPool.ReturnToPool(this);
        }
        
        public void ReturnToBackpack()
        {
            gameObject.SetActive(false);
            transform.parent = gameManager._BloodFx_Bp;
        }
        #endregion
    }
}