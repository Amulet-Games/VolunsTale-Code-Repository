using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public abstract class AIDisplayManager : MonoBehaviour
    {
        [Header("Health Bar Config.")]
        public CanvasGroup eHealthBarGroup;
        public LeanTweenType eHealthBarEaseType;
        public float eHealthBarFadeInSpeed;
        public float eHealthBarFadeOutSpeed;

        [Header("Health Slider Config.")]
        public Slider health;
        public Slider h_vis;
        public float sliderLerpSpeed = 4;

        [Header("Player Dmg Display Text Config.")]
        public TMP_Text dmgDisplayText;
        public LeanTweenType dmgDisplayTextEaseType;
        public float dmgDisplayTextFadeOutSpeed;
        public float dmgDisplayRate;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] protected bool _isShowingDamageDisplayText;
        [ReadOnlyInspector, SerializeField] protected bool _isOnHitUpdateHealthBarNeeded;
        [ReadOnlyInspector, SerializeField] protected float _dmgDisplayTimer;
        [ReadOnlyInspector, SerializeField] protected float _currentDisplayingDmgValue;

        [Header("Refs.")]
        [ReadOnlyInspector, SerializeField] protected Canvas eHealthBarCanvas;
        [ReadOnlyInspector, SerializeField] protected Canvas dmgDisplayTextCanvas;
        [ReadOnlyInspector, SerializeField] protected AIManager _ai;

        [Header("Non Serialized.")]
        protected int eHealthBarFadeInTweenId;
        protected int eHealthBarFadeOutTweenId;
        protected int dmgDisplayTextFadeOutTweenId;

        public abstract void Setup(AIManager _ai);

        public abstract void LateTick();
        
        public abstract void OnLockon();

        public abstract void OffLockon();

        public abstract void OnHit();

        public abstract void OnHitAgain();

        public abstract void OnDeath();

        public abstract void OnPlayerDeath();

        public abstract void OnCheckpointRefresh();

        #region Late Tick.
        protected void UpdateHealthBarValue()
        {
            if (_isOnHitUpdateHealthBarNeeded)
            {
                h_vis.value = Mathf.Lerp(h_vis.value, health.value, sliderLerpSpeed);
                if (h_vis.value - health.value < 2f)
                {
                    h_vis.value = (float)_ai.currentEnemyHealth;
                    SetIsOnHitUpdateHealthBarNeededToFalse();
                }
            }
        }

        protected void MonitorDamageDisplayTimer()
        {
            _dmgDisplayTimer += _ai._delta;
            if (_dmgDisplayTimer >= dmgDisplayRate)
            {
                UnRegisterDamageDisplayText();
            }
        }
        #endregion

        #region Register / UnRegister Damage Display Text.
        protected virtual void RegisterDamageDisplayText()
        {
            EditDamageDisplayText();

            if (!_isShowingDamageDisplayText)
            {
                _isShowingDamageDisplayText = true;
                ShowDamageDisplayText();
            }
        }

        protected void EditDamageDisplayText()
        {
            _dmgDisplayTimer = 0;
            _currentDisplayingDmgValue += (int)_ai._previousHitDamage;
            dmgDisplayText.text = _currentDisplayingDmgValue.ToString();
        }
        
        protected void UnRegisterDamageDisplayText()
        {
            _currentDisplayingDmgValue = 0;
            _isShowingDamageDisplayText = false;
            HideDamageDisplayText();
        }
        #endregion

        #region Show / Hide Health Bar.
        protected void EnableEnemyHealthBar()
        {
            eHealthBarCanvas.enabled = true;
        }

        protected void ShowEnemyHealthBar()
        {
            if (LeanTween.isTweening(eHealthBarFadeOutTweenId))
                LeanTween.cancel(eHealthBarFadeOutTweenId);

            EnableEnemyHealthBar();
            eHealthBarFadeInTweenId = LeanTween.alphaCanvas(eHealthBarGroup, 1, eHealthBarFadeInSpeed).setEase(eHealthBarEaseType).id;
        }

        protected void HideEnemyHealthBar()
        {
            if (LeanTween.isTweening(eHealthBarFadeInTweenId))
                LeanTween.cancel(eHealthBarFadeInTweenId);

            eHealthBarFadeOutTweenId = LeanTween.alphaCanvas(eHealthBarGroup, 0, eHealthBarFadeOutSpeed).setEase(eHealthBarEaseType).setOnComplete(DisableEnemyHealthBar).id;
        }

        protected virtual void DisableEnemyHealthBar()
        {
            eHealthBarCanvas.enabled = false;
        }
        #endregion

        #region Show / Hide Damage Text.
        protected void ShowDamageDisplayText()
        {
            dmgDisplayTextCanvas.enabled = true;
        }

        protected void HideDamageDisplayText()
        {
            dmgDisplayTextFadeOutTweenId = LeanTween.alpha(dmgDisplayText.rectTransform, 0, dmgDisplayTextFadeOutSpeed).setEase(dmgDisplayTextEaseType).setOnComplete(DisableDamageDisplayText).id;
        }

        protected virtual void DisableDamageDisplayText()
        {
            dmgDisplayTextCanvas.enabled = false;
        }
        #endregion

        #region Set Immediately.
        public void SetHealthBarValueToFullImmediately()
        {
            health.value = _ai.totalEnemyHealth;
            h_vis.value = _ai.totalEnemyHealth;
        }
        #endregion

        #region Set Status.
        protected void SetIsOnHitUpdateHealthBarNeededToTrue()
        {
            _isOnHitUpdateHealthBarNeeded = true;
        }

        protected void SetIsOnHitUpdateHealthBarNeededToFalse()
        {
            _isOnHitUpdateHealthBarNeeded = false;
        }
        #endregion

        #region Setup.
        protected void BaseSetupGetReferences()
        {
            eHealthBarCanvas = eHealthBarGroup.GetComponent<Canvas>();
            dmgDisplayTextCanvas = dmgDisplayText.GetComponent<Canvas>();
        }

        protected void SetupHealthBarValue()
        {
            float _enemyFullHealth = _ai.totalEnemyHealth;
            health.maxValue = _enemyFullHealth;
            h_vis.maxValue = _enemyFullHealth;

            health.value = _enemyFullHealth;
            h_vis.value = _enemyFullHealth;
        }
        #endregion
    }
}