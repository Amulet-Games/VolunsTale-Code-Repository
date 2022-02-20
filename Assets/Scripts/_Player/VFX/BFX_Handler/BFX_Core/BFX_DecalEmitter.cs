using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SA
{
    public class BFX_DecalEmitter : MonoBehaviour
    {
        [Header("Config.")]
        public float TimeHeightMax = 3.1f;
        public float TimeHeightMin = -0.1f;
        public float delayOffset;
        [Space]
        public Vector3 TimeScaleMax = Vector3.one;
        public Vector3 TimeScaleMin = Vector3.one;
        [Space]
        public Vector3 TimeOffsetMax = Vector3.zero;
        public Vector3 TimeOffsetMin = Vector3.zero;
        [Space]
        public AnimationCurve TimeByHeight = AnimationCurve.Linear(0, 0, 1, 1);
        
        private Vector3 startLocalScale;
        private Vector3 startLocalPos;
        private float timeDelay;
        
        [Header("Drag and Drop.")]
        public BFX_DecalUpdater decalUpdater;

        [Header("Refs.")]
        [ReadOnlyInspector] public BFX_Handler_SetBase _bfxHandler;

        [Header("Private.")]
        Transform mTransform;
        Transform rootTransform;

        public void Init()
        {
            SetupRefs();
            SetupTransforms();
            SetupStartStatus();

            void SetupRefs()
            {
                decalUpdater.decalEmitter = this;
            }

            void SetupTransforms()
            {
                mTransform = transform;
                rootTransform = mTransform.root;
            }

            void SetupStartStatus()
            {
                startLocalScale = mTransform.localScale;
                startLocalPos = mTransform.localPosition;
            }
        }
        
        #region Prepare Decal.
        public void OnDecalStart()
        {
            InitEmitterPosition();
            
            void InitEmitterPosition()
            {
                float ground = _bfxHandler.playerStates.mTransform.position.y;
                float decalHeightFromGround = rootTransform.position.y - ground;

                float currentScale = rootTransform.localScale.y;
                float scaledTimeHeightMax = TimeHeightMax * currentScale;

                ///* Dont spawn Decal if the blood is too High from the ground or too Low.
                if (decalHeightFromGround <= scaledTimeHeightMax && decalHeightFromGround >= (TimeHeightMin * currentScale))
                {
                    float diff = Mathf.Abs(decalHeightFromGround / scaledTimeHeightMax);

                    Vector3 scaleMul = Vector3.Lerp(TimeScaleMin, TimeScaleMax, diff);
                    mTransform.localScale = new Vector3(scaleMul.x * startLocalScale.x, startLocalScale.y, scaleMul.z * startLocalScale.z);

                    mTransform.position = new Vector3(0, ground + 0.05f, 0);
                    mTransform.localPosition = new Vector3(startLocalPos.x, mTransform.localPosition.y, startLocalPos.z) + Vector3.Lerp(TimeOffsetMin, TimeOffsetMax, diff);
                    
                    timeDelay = TimeByHeight.Evaluate(diff) - delayOffset;

                    ActivateUpdaterAfterWait();
                }

                void ActivateUpdaterAfterWait()
                {
                    Invoke("ActivateDecalUpdater", timeDelay);
                }
            }
        }

        void ActivateDecalUpdater()
        {
            decalUpdater.ActivateUpdater();
        }
        #endregion
    }
}