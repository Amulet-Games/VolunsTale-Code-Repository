using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class StatusHub : MonoBehaviour
    {
        #region Status Hub Texts.

        #region First
        [Header("First Status Hub, Top Field.")]
        [SerializeField] Text rWeapon1_Text;
        [SerializeField] Text rWeapon2_Text;
        [SerializeField] Text rWeapon3_Text;
        [SerializeField] Text lWeapon1_Text;
        [SerializeField] Text lWeapon2_Text;
        [SerializeField] Text lWeapon3_Text;

        [Header("Mid Field.")]
        [SerializeField] Text physical_reduction_Text;
        [SerializeField] Text strike_reduction_Text;
        [SerializeField] Text slash_reduction_Text;
        [SerializeField] Text thrust_reduction_Text;
        [SerializeField] Text magic_reduction_Text;
        [SerializeField] Text fire_reduction_Text;
        [SerializeField] Text lightning_reduction_Text;
        [SerializeField] Text dark_reduction_Text;

        [Header("Low Field.")]
        [SerializeField] Text bleed_resistance_Text;
        [SerializeField] Text poison_resistance_Text;
        [SerializeField] Text frost_resistance_Text;
        [SerializeField] Text curse_resistance_Text;
        #endregion

        #region Second
        [Header("Second Status Hub, Top Field.")]
        [SerializeField] Text level_Text;

        [Header("Mid Field.")]
        [SerializeField] Text vigor_Text;
        [SerializeField] Text adaptation_Text;
        [SerializeField] Text endurance_Text;
        [SerializeField] Text vitality_Text;
        [SerializeField] Text strength_Text;
        [SerializeField] Text hexes_Text;
        [SerializeField] Text intelligence_Text;
        [SerializeField] Text divinity_Text;
        [SerializeField] Text fortune_Text;

        [Header("First Low Field.")]
        [SerializeField] Text cur_health_Text;
        [SerializeField] Text full_health_Text;
        [SerializeField] Text cur_focus_Text;
        [SerializeField] Text full_focus_Text;
        [SerializeField] Text full_stamina_Text;

        [Header("Second Low Field.")]
        [SerializeField] Text cur_equipLoad_Text;
        [SerializeField] Text full_equipLoad_Text;
        [SerializeField] Text poise_Text;
        [SerializeField] Text itemDiscovery_Text;
        [SerializeField] Text attunementSlots_Text;
        #endregion

        #endregion

        #region Status Hub Tween.
        [Header("Status Hub Tween.")]
        public LeanTweenType _statusHubEaseType = LeanTweenType.easeOutExpo;
        public float _statusHubFadeInSpeed = 0.25f;
        public float _statusHubFadeOutSpeed = 0.25f;
        #endregion

        #region Update Intervals.
        [Header("Update Intervals.")]
        public int weaponTextUpdateInterval = 8;
        public int firstStatusHubTextUpdateInterval = 4;
        public int mainHudStatsTextUpdateInterval = 2;
        #endregion

        #region Fonts.
        [Header("Symbol Font.")]
        public Font _symbolFont;
        public Font _normalFont;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public bool isStatusHubOpened;
        [ReadOnlyInspector, SerializeField] bool isInSecondStatusDetail;
        [ReadOnlyInspector, SerializeField] int _frameCount;
        #endregion

        #region Groups Drops.
        [Header("First Group (Drops).")]
        [SerializeField] CanvasGroup firstStatusHubGroup;
        [SerializeField] Canvas firstStatusHubCanvas;

        [Header("Second Group (Drops).")]
        [SerializeField] CanvasGroup secondStatusHubGroup;
        [SerializeField] Canvas secondStatusHubCanvas;
        #endregion

        #region Refs.
        [Header("Refs.")]
        EquipmentMenuManager _equipmentMenuManager;
        #endregion

        #region Privates.
        [NonSerialized] StatsAttributeHandler _statsHandler;
        RuntimeWeapon[] _rightHandSlots;
        RuntimeWeapon[] _leftHandSlots;
        int _cur_statusHubTweenId;
        #endregion

        #region Tick.
        public void Tick()
        {
            UpdateFrameCount();
            UpdateStatusText();
        }

        void UpdateFrameCount()
        {
            _frameCount = Time.frameCount;
        }

        #region Update Text.
        void UpdateStatusText()
        {
            if (!isInSecondStatusDetail)
            {
                UpdateFirstStatusText();
            }
            else
            {
                UpdateSecondStatusText();
            }
        }
        
        void UpdateFirstStatusText()
        {
            if (_frameCount % weaponTextUpdateInterval == 0)
            {
                UpdateWeaponsAttackPowerText();
            }
            else if (_frameCount % firstStatusHubTextUpdateInterval == 0)
            {
                physical_reduction_Text.text = _statsHandler.b_physical_reduction.ToString();
                strike_reduction_Text.text = _statsHandler.b_strike_reduction.ToString();
                slash_reduction_Text.text = _statsHandler.b_slash_reduction.ToString();
                thrust_reduction_Text.text = _statsHandler.b_thrust_reduction.ToString();
                magic_reduction_Text.text = _statsHandler.b_magic_reduction.ToString();
                fire_reduction_Text.text = _statsHandler.b_fire_reduction.ToString();
                lightning_reduction_Text.text = _statsHandler.b_lightning_reduction.ToString();
                dark_reduction_Text.text = _statsHandler.b_dark_reduction.ToString();

                bleed_resistance_Text.text = _statsHandler.b_bleed_resistance.ToString();
                poison_resistance_Text.text = _statsHandler.b_poison_resistance.ToString();
                frost_resistance_Text.text = _statsHandler.b_frost_resistance.ToString();
                curse_resistance_Text.text = _statsHandler.b_curse_resistance.ToString();
            }
        }

        void UpdateSecondStatusText()
        {
            if (_frameCount % firstStatusHubTextUpdateInterval == 0)
            {
                level_Text.text = _statsHandler.playerLevel.ToString();
                vigor_Text.text = _statsHandler.vigor.ToString();
                adaptation_Text.text = _statsHandler.adaptation.ToString();
                endurance_Text.text = _statsHandler.endurance.ToString();
                vitality_Text.text = _statsHandler.vitality.ToString();
                strength_Text.text = _statsHandler.strength.ToString();
                hexes_Text.text = _statsHandler.hexes.ToString();
                intelligence_Text.text = _statsHandler.intelligence.ToString();
                divinity_Text.text = _statsHandler.divinity.ToString();
                fortune_Text.text = _statsHandler.fortune.ToString();

                full_equipLoad_Text.text = _statsHandler.b_total_equip_load.ToString();
                cur_equipLoad_Text.text = _statsHandler.b_cur_equip_load.ToString();
                poise_Text.text = _statsHandler.b_poise.ToString();
                itemDiscovery_Text.text = _statsHandler._item_discover.ToString();
                attunementSlots_Text.text = _statsHandler.b_attunement_slots.ToString();
            }
            else if (_frameCount % mainHudStatsTextUpdateInterval == 0)
            {
                full_health_Text.text = _statsHandler.b_hp.ToString();
                cur_health_Text.text = _statsHandler._hp.ToString();
                full_focus_Text.text = _statsHandler.b_fp.ToString();
                cur_focus_Text.text = _statsHandler._fp.ToString();
                full_stamina_Text.text = _statsHandler.b_stamina.ToString();
            }
        }

        void UpdateWeaponsAttackPowerText()
        {
            /// Rh
            if (_rightHandSlots[0] != null)
            {
                rWeapon1_Text.text = Math.Round(_rightHandSlots[0].ReturnWeaponPreviewAttackPower(), 1, MidpointRounding.AwayFromZero).ToString();
            }

            if (_rightHandSlots[1] != null)
            {
                rWeapon2_Text.text = Math.Round(_rightHandSlots[1].ReturnWeaponPreviewAttackPower(), 1, MidpointRounding.AwayFromZero).ToString();
            }

            if (_rightHandSlots[2] != null)
            {
                rWeapon3_Text.text = Math.Round(_rightHandSlots[2].ReturnWeaponPreviewAttackPower(), 1, MidpointRounding.AwayFromZero).ToString();
            }

            /// Lh
            if (_leftHandSlots[0] != null)
            {
                lWeapon1_Text.text = Math.Round(_leftHandSlots[0].ReturnWeaponPreviewAttackPower(), 1, MidpointRounding.AwayFromZero).ToString();
            }

            if (_leftHandSlots[1] != null)
            {
                lWeapon2_Text.text = Math.Round(_leftHandSlots[1].ReturnWeaponPreviewAttackPower(), 1, MidpointRounding.AwayFromZero).ToString();
            }

            if (_leftHandSlots[2] != null)
            {
                lWeapon3_Text.text = Math.Round(_leftHandSlots[2].ReturnWeaponPreviewAttackPower(), 1, MidpointRounding.AwayFromZero).ToString();
            }
        }
        #endregion

        #endregion

        #region Reset Hub OnMenuOpen / OnMenuClose.
        public void ResetHubOnMenuOpen()
        {
            ResetFrameConut();
            SetIsStatusHubOpenedToTrue();
            Refresh_All_WeaponAttacksTextStyle();

            void ResetFrameConut()
            {
                _frameCount = 0;
            }

            void SetIsStatusHubOpenedToTrue()
            {
                isStatusHubOpened = true;
            }
        }

        public void ResetHubOnMenuClose()
        {
            if (isInSecondStatusDetail)
            {
                _equipmentMenuManager.SetIsSwitchPlayerStatusHubStatus(false);
            }

            SetIsStatusHubOpenedToFalse();

            void SetIsStatusHubOpenedToFalse()
            {
                isStatusHubOpened = false;
            }
        }
        #endregion

        #region On First / Second Status Hub.
        public void OnFirstStatusHub()
        {
            isInSecondStatusDetail = false;
            ShowFirstStatusHub();
        }

        public void OnSecondStatusHub()
        {
            isInSecondStatusDetail = true;
            ShowSecondStatusHub();
        }
        #endregion

        #region Show / Hide StatusHub.

        #region First.
        void ShowFirstStatusHub()
        {
            if (secondStatusHubCanvas.enabled)
            {
                HideSecondStatusHub(true);
            }
            else
            {
                TweenOnFirstStatusHub();
            }
        }

        void OnCompleteTweenOnFirstHub()
        {
            secondStatusHubCanvas.enabled = false;
            TweenOnFirstStatusHub();
        }

        void HideFirstStatusHub(bool _tweenOnSecondHubOnComplete)
        {
            CancelUnFinishTweenJob();

            if (!_tweenOnSecondHubOnComplete)
            {
                _cur_statusHubTweenId = LeanTween.alphaCanvas(firstStatusHubGroup, 0, _statusHubFadeOutSpeed).setEase(_statusHubEaseType).setOnComplete(OnCompleteHideFirstHub).id;
            }
            else
            {
                _cur_statusHubTweenId = LeanTween.alphaCanvas(firstStatusHubGroup, 0, _statusHubFadeOutSpeed).setEase(_statusHubEaseType).setOnComplete(OnCompleteTweenOnSecondHub).id;
            }
        }

        void OnCompleteHideFirstHub()
        {
            firstStatusHubCanvas.enabled = false;
        }
        #endregion

        #region Second.
        void ShowSecondStatusHub()
        {
            if (firstStatusHubCanvas.enabled)
            {
                HideFirstStatusHub(true);
            }
            else
            {
                TweenOnSecondStatusHub();
            }
        }

        void OnCompleteTweenOnSecondHub()
        {
            firstStatusHubCanvas.enabled = false;
            TweenOnSecondStatusHub();
        }

        void HideSecondStatusHub(bool _tweenOnFirstHubOnComplete)
        {
            CancelUnFinishTweenJob();

            if (!_tweenOnFirstHubOnComplete)
            {
                _cur_statusHubTweenId = LeanTween.alphaCanvas(secondStatusHubGroup, 0, _statusHubFadeOutSpeed).setEase(_statusHubEaseType).setOnComplete(OnCompleteHideSecondHub).id;
            }
            else
            {
                _cur_statusHubTweenId = LeanTween.alphaCanvas(secondStatusHubGroup, 0, _statusHubFadeOutSpeed).setEase(_statusHubEaseType).setOnComplete(OnCompleteTweenOnFirstHub).id;
            }
        }

        void OnCompleteHideSecondHub()
        {
            secondStatusHubCanvas.enabled = false;
        }
        #endregion

        void TweenOnFirstStatusHub()
        {
            firstStatusHubCanvas.enabled = true;
            _cur_statusHubTweenId = LeanTween.alphaCanvas(firstStatusHubGroup, 1, _statusHubFadeInSpeed).setEase(_statusHubEaseType).id;
        }

        void TweenOnSecondStatusHub()
        {
            secondStatusHubCanvas.enabled = true;
            _cur_statusHubTweenId = LeanTween.alphaCanvas(secondStatusHubGroup, 1, _statusHubFadeInSpeed).setEase(_statusHubEaseType).id;
        }

        void CancelUnFinishTweenJob()
        {
            if (LeanTween.isTweening(_cur_statusHubTweenId))
                LeanTween.cancel(_cur_statusHubTweenId);
        }
        #endregion

        #region Refresh.

        #region Weapon Attack Power Text Style.
        void Refresh_All_WeaponAttacksTextStyle()
        {
            Refresh_Rh_WeaponAttacksTextStyle();
            Refresh_Lh_WeaponAttacksTextStyle();
        }

        public void Refresh_Rh_WeaponAttacksTextStyle()
        {
            if (_rightHandSlots[0] != null)
            {
                rWeapon1_Text.font = _normalFont;
            }
            else
            {
                rWeapon1_Text.font = _symbolFont;
                rWeapon1_Text.text = "-";
            }

            if (_rightHandSlots[1] != null)
            {
                rWeapon2_Text.font = _normalFont;
            }
            else
            {
                rWeapon2_Text.font = _symbolFont;
                rWeapon2_Text.text = "-";
            }

            if (_rightHandSlots[2] != null)
            {
                rWeapon3_Text.font = _normalFont;
            }
            else
            {
                rWeapon3_Text.font = _symbolFont;
                rWeapon3_Text.text = "-";
            }
        }

        public void Refresh_Lh_WeaponAttacksTextStyle()
        {
            if (_leftHandSlots[0] != null)
            {
                lWeapon1_Text.font = _normalFont;
            }
            else
            {
                lWeapon1_Text.font = _symbolFont;
                lWeapon1_Text.text = "-";
            }

            if (_leftHandSlots[1] != null)
            {
                lWeapon2_Text.font = _normalFont;
            }
            else
            {
                lWeapon2_Text.font = _symbolFont;
                lWeapon2_Text.text = "-";
            }

            if (_leftHandSlots[2] != null)
            {
                lWeapon3_Text.font = _normalFont;
            }
            else
            {
                lWeapon3_Text.font = _symbolFont;
                lWeapon3_Text.text = "-";
            }
        }
        #endregion

        #endregion

        #region Setup.
        public void Setup(EquipmentMenuManager _equipmentMenuManager)
        {
            this._equipmentMenuManager = _equipmentMenuManager;

            SetupReferences();
        }

        void SetupReferences()
        {
            StateManager states = _equipmentMenuManager._inp._states;
            SavableInventory _inventory = states._savableInventory;

            _statsHandler = states.statsHandler;

            _inventory._statusHub = this;
            _rightHandSlots = _inventory.rightHandSlots;
            _leftHandSlots = _inventory.leftHandSlots;
        }
        #endregion
    }
}