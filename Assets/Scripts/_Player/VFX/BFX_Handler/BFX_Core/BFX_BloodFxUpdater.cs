using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BFX_BloodFxUpdater : MonoBehaviour
    {
        public AnimationCurve AnimationSpeed = AnimationCurve.Linear(0, 0, 1, 1);
        public float FramesCount = 99;
        public float TimeLimit = 3;

        [Header("Status.")]
        [ReadOnlyInspector] public float currentTime;
        [ReadOnlyInspector] public bool _canUpdate;

        [Header("Drag and Drop.")]
        public Renderer _bloodFxRenderer;

        [Header("Refs.")]
        [ReadOnlyInspector] public BFX_Handler _bfxHandler;

        #region HideInInSpector.
        MaterialPropertyBlock propertyBlock;
        float totalFrames;
        #endregion
        
        public void Init()
        {
            InitGetMatPropertyBlock();

            GetTotalFrames();

            void InitGetMatPropertyBlock()
            {
                propertyBlock = new MaterialPropertyBlock();
            }

            void GetTotalFrames()
            {
                totalFrames = FramesCount + 1;
            }
        }

        public void OnBloodStart()
        {
            StartBloodUpdate();
            EnableRenderer();
            SetPropertyBlock();
            ResetCurrentTime();

            void StartBloodUpdate()
            {
                _canUpdate = true;
            }

            void EnableRenderer()
            {
                _bloodFxRenderer.enabled = true;
            }

            void SetPropertyBlock()
            {
                _bloodFxRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat(_bfxHandler._UseCustomTimeId, 1.0f);
                propertyBlock.SetFloat(_bfxHandler._TimeInFramesId, 0.0f);
                propertyBlock.SetFloat(_bfxHandler._LightIntencityId, Mathf.Clamp(_bfxHandler._lightIntensityMultiplier, 0.01f, 1f));
                _bloodFxRenderer.SetPropertyBlock(propertyBlock);
            }

            void ResetCurrentTime()
            {
                currentTime = 0;
            }
        }

        public void Tick()
        {
            if (!_canUpdate)
                return;

            if (currentTime > TimeLimit)
            {
                OnBloodEnd();
            }
            else
            {
                float currentFrameTime = AnimationSpeed.Evaluate(currentTime / TimeLimit);
                currentFrameTime = currentFrameTime * FramesCount + 1.1f;
                float timeInFrames = (Mathf.Ceil(-currentFrameTime) / totalFrames) + (1 / totalFrames);

                _bloodFxRenderer.GetPropertyBlock(propertyBlock);
                propertyBlock.SetFloat(_bfxHandler._TimeInFramesId, timeInFrames);
                _bloodFxRenderer.SetPropertyBlock(propertyBlock);
            }

            currentTime += _bfxHandler._delta;
        }

        void OnBloodEnd()
        {
            StopBloodUpdate();
            DisableRenderer();

            void StopBloodUpdate()
            {
                _canUpdate = false;
            }

            void DisableRenderer()
            {
                _bloodFxRenderer.enabled = false;
            }
        }
    }
}