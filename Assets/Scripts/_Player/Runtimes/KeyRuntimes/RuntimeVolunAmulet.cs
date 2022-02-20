using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RuntimeVolunAmulet : MonoBehaviour
    {
        [Header("Intensity Config.")]
        public float _originalEmitIntensity = 1.4f;
        public float _maxEmitIntensity = 2.6f;
        public float _blinkEmitIntensity = 3.2f;
        float _curEmitIntensity;
        Color _originalColor;
        Color _blinkEmitColor;
        Color _currentColor;

        [Header("Change Speed Config.")]
        public float _maxSpeed = 0.2f;
        public float _minSpeed = 0.5f;
        public float _maxSpeedIntenDifference = 1.4f;
        [ReadOnlyInspector] public float _calculatedSpeed;

        [Header("Volun Absorb Fx.")]
        public ParticleSystem _volunAbsorbFx;
        public ParticleSystem _weaponAbsorbFx;
        public ParticleSystem _armorAbsorbFx;
        public ParticleSystem _powerupAbsorbFx;
        public ParticleSystem _ringAbsorbFx;
        public ParticleSystem _consumableAbsorbFx;

        [Header("Refs.")]
        [ReadOnlyInspector, SerializeField] VolunAmuletItem _referedAmuletItem;
        [ReadOnlyInspector, SerializeField] Transform _hipTransform;
        [ReadOnlyInspector, SerializeField] Transform _rightHandTransform;
        [ReadOnlyInspector, SerializeField] Transform _mTransform;
        [ReadOnlyInspector] StateManager _states;

        #region Non Serialized.
        Material amuletMaterial;
        int _emissionColorPropertyId;
        int _intensityTweenId;
        #endregion

        public void ShowWhenIgnite()
        {
            _mTransform.parent = _rightHandTransform;

            _mTransform.localPosition = _referedAmuletItem._igniteHeldTransform.pos;
            _mTransform.localEulerAngles = _referedAmuletItem._igniteHeldTransform.eulers;
        }

        public void ShowWhenLevelup()
        {
            _mTransform.parent = _rightHandTransform;

            _mTransform.localPosition = _referedAmuletItem._levelupHeldTransform.pos;
            _mTransform.localEulerAngles = _referedAmuletItem._levelupHeldTransform.eulers;
        }

        public void SheathAmulet()
        {
            _mTransform.parent = _hipTransform;

            _mTransform.localPosition = _referedAmuletItem._amuletSheathTransform.pos;
            _mTransform.localEulerAngles = _referedAmuletItem._amuletSheathTransform.eulers;
        }
        
        #region Blink Amulet / Reset Emission.

        #region Variants.
        /// On Enemy Killed.
        public void OnEnemyAIKilled_BlinkVolunAmulet()
        {
            GetEmissionChangeSpeed();

            if (LeanTween.isTweening(_intensityTweenId))
                LeanTween.cancel(_intensityTweenId);

            _intensityTweenId = LeanTween.value(0, 1, _calculatedSpeed).setEaseOutCirc().
                setOnUpdate
                (
                    (value) => amuletMaterial.SetColor(_emissionColorPropertyId, Color.Lerp(_currentColor, _blinkEmitColor, value))
                )
                .setOnComplete(OnCompleteBlinkEmission).id;

            void OnCompleteBlinkEmission()
            {
                _curEmitIntensity = GetTargetEmissionValue();

                float _new_SqrIntensity = Mathf.Pow(2, _curEmitIntensity);
                Color _targetColor = new Color(_originalColor.r * _new_SqrIntensity, _originalColor.g * _new_SqrIntensity, _originalColor.b * _new_SqrIntensity);

                if (LeanTween.isTweening(_intensityTweenId))
                    LeanTween.cancel(_intensityTweenId);

                _intensityTweenId = LeanTween.value(0, 1, _calculatedSpeed).setEaseOutCirc().
                    setOnUpdate
                    (
                        (value) => amuletMaterial.SetColor(_emissionColorPropertyId, Color.Lerp(_blinkEmitColor, _targetColor, value))
                    )
                    .setOnComplete(OnCompleteOverwriteCurrentColor).id;

                void OnCompleteOverwriteCurrentColor()
                {
                    _currentColor = _targetColor;
                }
            }
        }

        /// On Armor Preview Ended.
        public void OnArmorPreviewEnded_BlinkVolunAmulet()
        {
            BlinkVolunAmulet_WithOnCompleteAction(_states.ReverseArmorPreviewDissolve);
        }

        public void OnPowerupPreviewEnded_BlinkVolunAmulet()
        {
            if (_states._savableInventory.powerupSlot != null)
            {
                BlinkVolunAmulet_WithOnCompleteAction(_states.ReversePowerupPreviewDissolve);
            }
            else
            {
                BlinkVolunAmulet();
            }
        }

        public void OnWeaponAbsorb_BlinkVolunAmulet()
        {
            BlinkVolunAmulet();
        }

        public void OnRingAbsorbEnded_BlinkVolunAmulet()
        {
            BlinkVolunAmulet_WithOnCompleteAction(_states._savableInventory.ReversePreviewRing);
        }

        public void OnConsumableAbsorb_BlinkVolunAmulet()
        {
            BlinkVolunAmulet();
        }
        #endregion

        /// Blink Amulet.
        public void BlinkVolunAmulet()
        {
            GetEmissionChangeSpeed();

            if (LeanTween.isTweening(_intensityTweenId))
                LeanTween.cancel(_intensityTweenId);

            _intensityTweenId = LeanTween.value(0, 1, _calculatedSpeed).setEaseOutCirc().
                setOnUpdate
                (
                    (value) => amuletMaterial.SetColor(_emissionColorPropertyId, Color.Lerp(_currentColor, _blinkEmitColor, value))
                )
                .setOnComplete(OnCompleteBlinkEmission).id;

            void OnCompleteBlinkEmission()
            {
                _intensityTweenId = LeanTween.value(0, 1, _calculatedSpeed).setEaseOutCirc().
                    setOnUpdate
                    (
                        (value) => amuletMaterial.SetColor(_emissionColorPropertyId, Color.Lerp(_blinkEmitColor, _currentColor, value))
                    ).id;
            }
        }

        /// Blink Amulet With Complete Action.
        public void BlinkVolunAmulet_WithOnCompleteAction(Action _onCompleteAction)
        {
            GetEmissionChangeSpeed();

            if (LeanTween.isTweening(_intensityTweenId))
                LeanTween.cancel(_intensityTweenId);

            _intensityTweenId = LeanTween.value(0, 1, _calculatedSpeed).setEaseOutCirc().
                setOnUpdate
                (
                    (value) => amuletMaterial.SetColor(_emissionColorPropertyId, Color.Lerp(_currentColor, _blinkEmitColor, value))
                )
                .setOnComplete(OnCompleteBlinkEmission).id;

            void OnCompleteBlinkEmission()
            {
                _intensityTweenId = LeanTween.value(0, 1, _calculatedSpeed).setEaseOutCirc().
                    setOnUpdate
                    (
                        (value) => amuletMaterial.SetColor(_emissionColorPropertyId, Color.Lerp(_blinkEmitColor, _currentColor, value))
                    )
                    .setOnComplete(OnCompleteOverwriteCurrentColor).id;

                void OnCompleteOverwriteCurrentColor()
                {
                    _onCompleteAction.Invoke();
                }
            }
        }

        /// On Level up.
        public void OnLevelupChangeEmission()
        {
            _curEmitIntensity = GetTargetEmissionValue();

            float _new_SqrIntensity = Mathf.Pow(2, _curEmitIntensity);
            Color _targetColor = new Color(_originalColor.r * _new_SqrIntensity, _originalColor.g * _new_SqrIntensity, _originalColor.b * _new_SqrIntensity);

            if (LeanTween.isTweening(_intensityTweenId))
                LeanTween.cancel(_intensityTweenId);

            _intensityTweenId = LeanTween.value(0, 1, 1).setEaseOutCirc().
                setOnUpdate
                (
                    (value) =>
                    amuletMaterial.SetColor(_emissionColorPropertyId, Color.Lerp(_currentColor, _targetColor, value))

                ).setOnComplete(OnCompleteOverwriteCurrentColor).id;

            void OnCompleteOverwriteCurrentColor()
            {
                _currentColor = _targetColor;
            }
        }

        /// Set By Current Volun Ratio. (No Lerp)
        public void SetEmissionColorByCurrentVolun()
        {
            _curEmitIntensity = GetTargetEmissionValue();

            float _targetEmitIntensity = Mathf.Pow(2, _curEmitIntensity);
            _currentColor = new Color(_originalColor.r * _targetEmitIntensity, _originalColor.g * _targetEmitIntensity, _originalColor.b * _targetEmitIntensity);
            amuletMaterial.SetColor(_emissionColorPropertyId, _currentColor);
        }
        #endregion

        #region Play Fx.
        public void PlayVolunAbsorbFx()
        {
            _volunAbsorbFx.gameObject.SetActive(true);
            _volunAbsorbFx.Play();
        }

        public void PlayWeaponAbsorbFx()
        {
            _weaponAbsorbFx.gameObject.SetActive(true);
            _weaponAbsorbFx.Play();
        }

        public void PlayArmorAbsorbFx()
        {
            _armorAbsorbFx.gameObject.SetActive(true);
            _armorAbsorbFx.Play();
        }

        public void PlayPowerupAbsorbFx()
        {
            _powerupAbsorbFx.gameObject.SetActive(true);
            _powerupAbsorbFx.Play();
        }

        public void PlayRingAbsorbFx()
        {
            _ringAbsorbFx.gameObject.SetActive(true);
            _ringAbsorbFx.Play();
        }

        public void PlayConsumableAbsorbFx()
        {
            _consumableAbsorbFx.gameObject.SetActive(true);
            _consumableAbsorbFx.Play();
        }
        #endregion

        #region Init.
        public void Init(SavableInventory _inventory, VolunAmuletItem _referedAmuletItem)
        {
            this._referedAmuletItem = _referedAmuletItem;
            _states = _inventory._states;

            _hipTransform = _states.hipTransform;
            _rightHandTransform = _states.rightHandTransform;
            _mTransform = gameObject.transform;

            _inventory.runtimeAmulet = this;

            SheathAmulet();

            InitGetMaterialRef();
            InitGetBlinkEmitColor();
            SetEmissionColorByCurrentVolun();
        }

        void InitGetMaterialRef()
        {
            amuletMaterial = GetComponent<MeshRenderer>().material;
            _emissionColorPropertyId = Shader.PropertyToID("_EmissionColor");
            _originalColor = amuletMaterial.GetColor(_emissionColorPropertyId);
        }

        void InitGetBlinkEmitColor()
        {
            float _new_SqrIntensity = Mathf.Pow(2, _blinkEmitIntensity);
            _blinkEmitColor = new Color(_originalColor.r * _new_SqrIntensity, _originalColor.g * _new_SqrIntensity, _originalColor.b * _new_SqrIntensity);
        }
        #endregion

        #region Emission Change Speed.
        void GetEmissionChangeSpeed()
        {
            _calculatedSpeed = Mathf.Clamp01((_blinkEmitIntensity - _curEmitIntensity) / _maxSpeedIntenDifference) * (_maxSpeed - _minSpeed) + _minSpeed;
        }
        #endregion

        #region Emission Value.
        public float GetTargetEmissionValue()
        {
            return _states.statsHandler.GetLevelupRequireRatioProgress() * (_maxEmitIntensity - _originalEmitIntensity) + _originalEmitIntensity;
        }
        #endregion
    }
}