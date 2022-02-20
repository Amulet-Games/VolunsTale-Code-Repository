using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SA
{
    public class PostProcessManager : MonoBehaviour
    {
        [Header("Depth Of Field Mask Init Value.")]
        public float _DOF_start_initValue = 0f;
        public float _DOF_radius_initValue = 1.5f;

        [Header("Depth Of Field Mask Opened Tween")]
        public float _DOF_TweenSpeed_whenMaskOpened;
        public float _DOF_start_targetValue_whenMaskOpened = 13f;
        public float _DOF_radius_targetValue_whenMaskOpened = 1f;

        [Header("Depth Of Field After Start Pressed Tween.")]
        public float _DOF_TweenSpeed_afterStartPressed;
        public float _DOF_start_targetValue_afterStartPressed = 35.29f;
        public float _DOF_radius_targetValue_afterStartPressed = 0.5f;

        [Header("Post Process Volume. (Drag and Drop.)")]
        public Volume _TL_PPV;
        public Volume _TS_PPV;
        public Volume _WF_PPV;

        [ReadOnlyInspector] public DepthOfField _TS_DepthOfField;

        public static PostProcessManager singleton;
        private void Awake()
        {
            singleton = this;

            /// Get Depth Of Field.
            _TS_PPV.profile.TryGet(out _TS_DepthOfField);
        }

        /// ChainOperationFinish_StartGame_LoadLevel_EndOps
        public void SwitchVolumeToWinterField()
        {
            _TL_PPV.enabled = false;
            _TS_PPV.enabled = false;
            _WF_PPV.enabled = true;
        }

        /// ChainOperationFinish_QuitGame_ReloadTitleScreen_EndOps
        public void SwitchVolumeToTitleScreen()
        {
            _TL_PPV.enabled = true;
            _TS_PPV.enabled = true;
            _WF_PPV.enabled = false;

            Reset_DOF_Status();
        }

        #region Main Menu Switch Effects.
        public void Expand_TitleScreen_BlurEffect_AfterStartButton()
        {
            LeanTween.value(_TS_PPV.gameObject, _TS_DepthOfField.gaussianStart.value, _DOF_start_targetValue_afterStartPressed, _DOF_TweenSpeed_afterStartPressed).setEaseOutQuint().setOnUpdate((value) => _TS_DepthOfField.gaussianStart.value = value);
            LeanTween.value(_TS_PPV.gameObject, _TS_DepthOfField.gaussianMaxRadius.value, _DOF_radius_targetValue_afterStartPressed, _DOF_TweenSpeed_afterStartPressed).setEaseOutQuint().setOnUpdate((value) => _TS_DepthOfField.gaussianMaxRadius.value = value);
        }

        public void Expand_TitleScreen_BlurEffect_MaskOpen()
        {
            LeanTween.value(_TS_PPV.gameObject, _TS_DepthOfField.gaussianStart.value, _DOF_start_targetValue_whenMaskOpened, _DOF_TweenSpeed_whenMaskOpened).setEaseInCirc().setOnUpdate((value) => _TS_DepthOfField.gaussianStart.value = value);
            LeanTween.value(_TS_PPV.gameObject, _TS_DepthOfField.gaussianMaxRadius.value, _DOF_radius_targetValue_whenMaskOpened, _DOF_TweenSpeed_whenMaskOpened).setEaseInCirc().setOnUpdate((value) => _TS_DepthOfField.gaussianMaxRadius.value = value);
        }

        void Reset_DOF_Status()
        {
            _TS_DepthOfField.gaussianStart.value = _DOF_start_initValue;
            _TS_DepthOfField.gaussianMaxRadius.value = _DOF_radius_initValue;
        }
        #endregion
    }
}