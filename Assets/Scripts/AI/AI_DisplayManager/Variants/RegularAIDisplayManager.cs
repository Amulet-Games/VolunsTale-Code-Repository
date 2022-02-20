using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RegularAIDisplayManager : AIDisplayManager
    {
        [Header("AI Regular Config.")]
        public float eHealthBarUpdateInterval;
        public float eHealthBarDisplayRate;

        [Header("AI Regular Status.")]
        [ReadOnlyInspector, SerializeField] bool _isShowingHealthBar;
        [ReadOnlyInspector, SerializeField] bool _isAIBeingLockedOn;

        [Space(7)]
        [ReadOnlyInspector, SerializeField] float _eHealthBarDisplayTimer;
        
        [Header("Regular AI Refs.")]
        [ReadOnlyInspector, SerializeField] CameraHandler _playerCamHandler;
        [ReadOnlyInspector, SerializeField] Transform _mainCameraTransform;
        
        public override void Setup(AIManager _ai)
        {
            this._ai = _ai;

            SetupGetReferences();
            SetupHealthBarValue();

            base.DisableEnemyHealthBar();
            base.DisableDamageDisplayText();
        }

        public override void LateTick()
        {
            if (_isShowingHealthBar)
            {
                UpdateEnemyHealthBar();
                MonitorEnemyHealthBarTimer();
            }

            if (_isShowingDamageDisplayText)
            {
                MonitorDamageDisplayTimer();
            }
        }

        public override void OnLockon()
        {
            SetIsAIBeingLockedOnToTrue();
            RegisterEnemyHealthBar();
        }

        public override void OffLockon()
        {
            SetIsAIBeingLockedOnToFalse();
        }

        public override void OnHit()
        {
            ResetHealthBarDisplayTimer();
            RegisterEnemyHealthBar();
            RegisterDamageDisplayText();

            SetIsOnHitUpdateHealthBarNeededToTrue();
            OnHitRefreshHealthBarValue();

            void OnHitRefreshHealthBarValue()
            {
                health.value = (float)_ai.currentEnemyHealth;
            }
        }

        public override void OnHitAgain()
        {
            ResetHealthBarDisplayTimer();
            EditDamageDisplayText();

            SetIsOnHitUpdateHealthBarNeededToTrue();
            OnHitRefreshHealthBarValue();

            void OnHitRefreshHealthBarValue()
            {
                health.value = (float)_ai.currentEnemyHealth;
            }
        }

        public override void OnDeath()
        {
            UnRegisterEnemyHealthBar();
            ResetHealthBarDisplayTimer();

            UnRegisterDamageDisplayText();
        }

        public override void OnPlayerDeath()
        {
            UnRegisterEnemyHealthBar();
            ResetHealthBarDisplayTimer();

            UnRegisterDamageDisplayText();
        }

        public override void OnCheckpointRefresh()
        {
            health.value = (float)_ai.currentEnemyHealth;
        }

        /// RegisterDamageDisplayText
        protected override void RegisterDamageDisplayText()
        {
            EditDamageDisplayText();

            if (!_isShowingDamageDisplayText)
            {
                _isShowingDamageDisplayText = true;
                ShowDamageDisplayText();

                /// World UI Camera.
                _playerCamHandler.IncreaseWorldUICameraUsageCount();
            }
        }

        /// UnRegisterDamageDisplayText
        protected override void DisableDamageDisplayText()
        {
            base.DisableDamageDisplayText();

            /// World UI Camera.
            _playerCamHandler.DecreaseWorldUICameraUsageCount();
        }

        #region Late Tick.
        void UpdateEnemyHealthBar()
        {
            UpdateEnemyHealthBarTransform();

            if (_isOnHitUpdateHealthBarNeeded)
            {
                UpdateHealthBarValue();
            }
        }
        
        void UpdateEnemyHealthBarTransform()
        {
            if (_ai._frameCount % eHealthBarUpdateInterval == 0)
            {
                transform.LookAt(_mainCameraTransform.position);
            }
        }

        void MonitorEnemyHealthBarTimer()
        {
            if (_isAIBeingLockedOn)
                return;

            _eHealthBarDisplayTimer += _ai._delta;
            if (_eHealthBarDisplayTimer >= eHealthBarDisplayRate)
            {
                ResetHealthBarDisplayTimer();
                UnRegisterEnemyHealthBar();
            }
        }
        #endregion

        #region Register / UnRegister Health Bar.
        void RegisterEnemyHealthBar()
        {
            if (!_isShowingHealthBar)
            {
                _isShowingHealthBar = true;
                ShowEnemyHealthBar();

                /// World UI Camera.
                _playerCamHandler.IncreaseWorldUICameraUsageCount();
            }
        }
        
        void UnRegisterEnemyHealthBar()
        {
            _isShowingHealthBar = false;
            HideEnemyHealthBar();
        }

        void ResetHealthBarDisplayTimer()
        {
            _eHealthBarDisplayTimer = 0;
        }

        protected override void DisableEnemyHealthBar()
        {
            base.DisableEnemyHealthBar();

            /// World UI Camera.
            _playerCamHandler.DecreaseWorldUICameraUsageCount();
        }
        #endregion

        #region Set Status.
        void SetIsAIBeingLockedOnToTrue()
        {
            _isAIBeingLockedOn = true;
            ResetHealthBarDisplayTimer();
        }

        void SetIsAIBeingLockedOnToFalse()
        {
            _isAIBeingLockedOn = false;
        }
        #endregion

        #region Setup.
        void SetupGetReferences()
        {
            BaseSetupGetReferences();

            _playerCamHandler = CameraHandler.singleton;
            _mainCameraTransform = _playerCamHandler._mainCameraTransform;
            
            GetComponent<Canvas>().worldCamera = _playerCamHandler.worldUICamera;
        }
        #endregion
    }
}