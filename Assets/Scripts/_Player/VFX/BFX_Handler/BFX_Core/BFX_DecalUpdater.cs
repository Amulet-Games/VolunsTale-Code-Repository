using UnityEngine;
using System.Collections;
using System;

namespace SA
{
    public class BFX_DecalUpdater : MonoBehaviour
    {
        [Header("Config.")]
        public AnimationCurve FloatCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float GraphTimeMultiplier = 1, GraphIntensityMultiplier = 1;
        
        [Header("Status.")]
        [ReadOnlyInspector] public float timeLapsed;
        [ReadOnlyInspector] public bool _canUpdate;
        [ReadOnlyInspector] public bool _isLastDecalInSet;

        [Header("Drag and Drop.")]
        public Renderer _decalRenderer;

        [Header("Refs")]
        [ReadOnlyInspector] public BFX_DecalEmitter decalEmitter;
        [ReadOnlyInspector] public BFX_Handler_SetBase _bfxHandler;

        #region HideInInspector.
        float _graphEvalZeroPoint;
        float _clampedLightIntersity;

        MaterialPropertyBlock props;
        #endregion

        public void Init()
        {
            InitGetMatPropertyBlock();

            void InitGetMatPropertyBlock()
            {
                props = new MaterialPropertyBlock();

                _graphEvalZeroPoint = FloatCurve.Evaluate(0) * GraphIntensityMultiplier;
                _clampedLightIntersity = Mathf.Clamp(_bfxHandler._lightIntensityMultiplier, 0.01f, 1f);
            }
        }
        
        public void Tick()
        {
            if (!_canUpdate)
                return;

            IncrementTimeLapse();

            SetPropertyBlock();

            CheckIsDecalEnded();

            void IncrementTimeLapse()
            {
                timeLapsed += _bfxHandler._delta;
            }

            void SetPropertyBlock()
            {
                _decalRenderer.GetPropertyBlock(props);

                float eval = FloatCurve.Evaluate(timeLapsed / GraphTimeMultiplier) * GraphIntensityMultiplier;
                props.SetFloat(_bfxHandler.cutoutPropertyID, eval);

                _decalRenderer.SetPropertyBlock(props);
            }

            void CheckIsDecalEnded()
            {
                if (timeLapsed >= GraphTimeMultiplier)
                {
                    DeactivateUpdater();
                }
            }
        }
        
        #region Activate / Deactivate Updater.
        public void ActivateUpdater()
        {
            AllowDecalStartUpdate();

            EnableRenderer();

            OnActivateSetPropertyBlock();
            
            void AllowDecalStartUpdate()
            {
                _canUpdate = true;
            }
            
            void EnableRenderer()
            {
                _decalRenderer.enabled = true;
            }

            void OnActivateSetPropertyBlock()
            {
                _decalRenderer.GetPropertyBlock(props);

                props.SetFloat(_bfxHandler.cutoutPropertyID, _graphEvalZeroPoint);
                props.SetVector(_bfxHandler.forwardDirPropertyID, transform.up);
                props.SetFloat(_bfxHandler._LightIntencityId, _clampedLightIntersity);

                _decalRenderer.SetPropertyBlock(props);
            }
        }

        void DeactivateUpdater()
        {
            StopDecalUpdate();

            DisableRenderer();

            ResetTimeLapsed();

            StopBFXHandler();

            void StopDecalUpdate()
            {
                _canUpdate = false;
            }

            void DisableRenderer()
            {
                _decalRenderer.enabled = false;
            }

            void ResetTimeLapsed()
            {
                timeLapsed = 0;
            }

            void StopBFXHandler()
            {
                if (_isLastDecalInSet)
                    _bfxHandler.On_BFX_Stop();
            }
        }
        #endregion
    }
}
