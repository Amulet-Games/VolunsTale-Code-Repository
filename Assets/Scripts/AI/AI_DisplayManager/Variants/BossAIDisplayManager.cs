using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BossAIDisplayManager : AIDisplayManager
    {
        public override void Setup(AIManager _ai)
        {
            this._ai = _ai;

            BaseSetupGetReferences();
            SetupHealthBarValue();

            DisableEnemyHealthBar();
            DisableDamageDisplayText();
        }

        public override void LateTick()
        {
            UpdateHealthBarValue();

            if (_isShowingDamageDisplayText)
                MonitorDamageDisplayTimer();
        }

        public override void OnLockon()
        {
            ShowEnemyHealthBar();
        }

        public override void OnHit()
        {
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
            EditDamageDisplayText();
        }

        public override void OnDeath()
        {
            HideEnemyHealthBar();

            UnRegisterDamageDisplayText();
        }

        public override void OnPlayerDeath()
        {
            HideEnemyHealthBar_OnPlayerDeath();

            UnRegisterDamageDisplayText_OnPlayerDeath();
        }

        public override void OnCheckpointRefresh()
        {
            health.value = (float)_ai.currentEnemyHealth;
        }

        void HideEnemyHealthBar_OnPlayerDeath()
        {
            if (LeanTween.isTweening(eHealthBarFadeInTweenId))
                LeanTween.cancel(eHealthBarFadeInTweenId);

            eHealthBarFadeOutTweenId = LeanTween.alphaCanvas(eHealthBarGroup, 0, 0.6f).setEase(eHealthBarEaseType).setOnComplete(DisableEnemyHealthBar).id;
        }

        void UnRegisterDamageDisplayText_OnPlayerDeath()
        {
            _currentDisplayingDmgValue = 0;
            _isShowingDamageDisplayText = false;
            dmgDisplayTextFadeOutTweenId = LeanTween.alpha(dmgDisplayText.rectTransform, 0, 0.6f).setEase(dmgDisplayTextEaseType).setOnComplete(DisableDamageDisplayText).id;
        }

        /// ------------- Not Being Used-----------------------------
        public override void OffLockon()
        {
        }
    }
}