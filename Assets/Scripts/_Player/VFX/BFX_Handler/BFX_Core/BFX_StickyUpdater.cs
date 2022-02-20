using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BFX_StickyUpdater : MonoBehaviour
    {
        [Header("Config.")]
        public AnimationCurve FloatCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        public float GraphTimeMultiplier = 1, GraphIntensityMultiplier = 1;

        [Header("Status.")]
        [ReadOnlyInspector] public float timeLapsed;

        [Header("Drag and Drop.")]
        public Renderer _decalRenderer;

        [Header("Refs")]
        [ReadOnlyInspector] public StateManager playerStates;
        [ReadOnlyInspector] public GameManager gameManager;

        #region Private.
        float _graphEvalZeroPoint;

        MaterialPropertyBlock props;
        #endregion

        public void Init()
        {
            InitRefs();
            InitGetMatPropertyBlock();

            void InitRefs()
            {
                playerStates = SessionManager.singleton._states;
                gameManager = GameManager.singleton;
            }

            void InitGetMatPropertyBlock()
            {
                props = new MaterialPropertyBlock();
                _graphEvalZeroPoint = FloatCurve.Evaluate(0) * GraphIntensityMultiplier;
            }
        }

        public void OnStickyStart()
        {
            ActivateSticky();

            SetPropertyBlock();

            void ActivateSticky()
            {
                gameObject.SetActive(true);
            }

            void SetPropertyBlock()
            {
                _decalRenderer.GetPropertyBlock(props);

                props.SetFloat(gameManager.cutoutPropertyID, _graphEvalZeroPoint);
                props.SetVector(gameManager.forwardDirPropertyID, transform.up);

                _decalRenderer.SetPropertyBlock(props);
            }
        }

        public void Tick()
        {
            UpdateDeltas();

            SetPropertyBlock();

            CheckIsDecalEnded();
            
            void UpdateDeltas()
            {
                timeLapsed += playerStates._delta;
            }

            void SetPropertyBlock()
            {
                _decalRenderer.GetPropertyBlock(props);

                float eval = FloatCurve.Evaluate(timeLapsed / GraphTimeMultiplier) * GraphIntensityMultiplier;
                props.SetFloat(gameManager.cutoutPropertyID, eval);

                _decalRenderer.SetPropertyBlock(props);
            }

            void CheckIsDecalEnded()
            {
                if (timeLapsed >= GraphTimeMultiplier)
                {
                    OnStickyEnd();
                }
            }
        }

        void OnStickyEnd()
        {
            ResetTimeLapsed();

            ReturnToPool();

            void ResetTimeLapsed()
            {
                timeLapsed = 0;
            }

            void ReturnToPool()
            {
                playerStates.RemoveStickyUpdaterFromInProcessList(this);
                gameManager._stickyPool.ReturnToPool(this);
            }
        }

        public void ReturnToBackpack()
        {
            gameObject.SetActive(false);
            transform.parent = gameManager._BloodFx_Bp;
        }
    }
}