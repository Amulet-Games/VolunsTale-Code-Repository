using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class MainHudManager : MonoBehaviour
    {
        #region MainHud.
        [Header("MainHud Config.")]
        public CanvasGroup mainHudGroup;
        public LeanTweenType mainHudEaseType;
        public float mainHudFadeInSpeed;
        public float mainHudFadeOutSpeed;
        #endregion

        #region Upper Left.

        #region Sliders.
        [Header("Stats Sliders Configs.")]
        public float healthSliderLerpSpeed = 0.02f;
        public float staminaSliderLerpSpeed = 0.06f;
        public float sliderSizeMultiplier;
        public Slider health;
        public Slider h_vis;
        public Slider focus;
        public Slider f_vis;
        public Slider stamina;
        public Slider s_vis;
        public Image staminaSliderFillImage;
        public Color staminaDefaultColor;
        public Color staminaWaitForRecoverColor;
        public float staminaColorChangeSpeed;
        #endregion

        #region Grids.
        [Header("Effect Icons.")]
        public EffectIcon[] statsEffectIcons = new EffectIcon[3];
        public EffectIcon[] weaponBuffEffectIcons = new EffectIcon[2];

        [Header("Two Hand Fist Fighter Mod Icon.")]
        public EffectIcon fighterModeEffectIcon;
        public Sprite fighterModeSprite;

        [Header("Effect Tween.")]
        public LeanTweenType _iconEaseType = LeanTweenType.easeOutCirc;
        public float statsEffectIconFadeTime = 0.45f;
        public float weaponBuffIconFadeTime = 0.25f;
        #endregion

        #region Damage Previewer.
        public DamagePreviewer damagePreviewer;
        #endregion

        #endregion

        #region Upper Right.
        [Header("Volun Texts Configs.")]
        public TMP_Text _volunSlotText;
        public int CountFPS = 30;
        public float Duration = 1f;

        [ReadOnlyInspector] public int _volunSlotValue;
        Coroutine CountingCoroutine;
        #endregion
        
        #region QSlots.

        #region QSlots Icons.
        [Header("QSlots Icons.")]
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;
        public Image consumableIcon;
        #endregion

        #region QSlots Highlighters.
        [Header("QSlots Highlighters.")]
        public RectTransform leftWeaponHighlighterRect;
        public RectTransform rightWeaponHighlighterRect;
        public RectTransform consumableHighlighterRect;
        public LeanTweenType qSlotHighlighterEaseType;
        public float qSlotHighlighterFadeTime;
        public float qSlotHighlighterMaxFadeValue;
        public float qSlotHighlighterMinFadeValue;
        #endregion

        #region QSlots Group.
        [Header("Weapon Group. Canvas.(Drag and Drop)")]
        public CanvasGroup leftWeaponGroup;
        public CanvasGroup rightWeaponGroup;
        public Canvas leftWeaponGroupCanvas;
        public Canvas rightWeaponGroupCanvas;
        public LeanTweenType weaponGroupEaseType;
        public float weaponGroupFadeTime;
        #endregion

        #region QSlots Texts.
        public TMP_Text leftWeaponNameText;
        public TMP_Text rightWeaponNameText;
        #endregion

        #region QSlots Stickers.
        [Header("LH Weapon Stickers.")]
        public ActionSticker _lh_rb_actionSticker;
        public ActionSticker _lh_lb_actionSticker;
        public ActionSticker _lh_rt_actionSticker;
        public ActionSticker _lh_lt_actionSticker;

        [Header("RH Weapon Stickers.")]
        public ActionSticker _rh_rb_actionSticker;
        public ActionSticker _rh_lb_actionSticker;
        public ActionSticker _rh_rt_actionSticker;
        public ActionSticker _rh_lt_actionSticker;
        #endregion

        #region Two Handing Icon.
        [Header("Two Handing Icon. Canvas.(Drag and Drop)")]
        public Image left_twoHandingIcon;
        public Image right_twoHandingIcon;
        public Canvas left_twoHandingIconCanvas;
        public Canvas right_twoHandingIconCanvas;
        public LeanTweenType twoHandingFadeEaseType;
        public float twoHandingFadeTime;
        #endregion

        #region Mini QSlots Config.
        [Header("Mini QSlots Config.")]
        public Image nextConsumableQSlotIcon;
        public Image secondNextConsumableQSlotIcon;
        public LeanTweenType miniQSlotFadeEaseType;
        public float miniQSlotFadeSpeed = 0.2f;
        #endregion

        #endregion

        #region Cards.

        #region Interaction Card.
        [Header("Interaction Card Configs.")]
        public InteractionCard _interactionCard;
        public Canvas pickup_inter_content;
        public Canvas openDoor_inter_content;
        public Canvas openChest_inter_content;
        public Canvas takeChest_inter_content;
        public Canvas activate_inter_content;
        public Canvas rest_inter_content;
        public Canvas talk_inter_content;

        [Header("Interaction Card Refs.")]
        [ReadOnlyInspector] public Canvas _current_inter_content;
        [ReadOnlyInspector] public Canvas _next_inter_content;
        [ReadOnlyInspector] public bool _isInteractionCardShown;
        #endregion

        #region Info Card.
        [Header("Info Card Configs.")]
        public ItemInfoCard _itemInfoCard;
        public float _infoCardDisplayRate;
        [HideInInspector] public float _infoCardDisplayTimer;
        [ReadOnlyInspector] public bool _isInfoCardDisplaying;
        #endregion

        #region Execution Cards.
        [Header("Execution Cards Configs.")]
        public InteractionCard _executionCard;
        public Canvas parryExe_content;
        #endregion

        #endregion

        #region Full Screen.

        #region Damage Screen.
        [Header("Damage Screen Config.")]
        public RectTransform damagedScreenRect;
        public float damageScreenFadeSpeed = 0.075f;
        #endregion

        #region Death Screen.
        [Header("Death Screen Config.")]
        public Image deathScreenImage;
        public Sprite deathScreen_NoTrans_Sprite;
        public Sprite deathScreen_FullTrans_Sprite;
        public LeanTweenType deathScreenEaseType;
        public float deathScreenFadeInSpeed;
        public float switchDeathScreenWaitTime = 1f;
        #endregion

        #region Bonfire Lit Message.
        [Header("bonfire Lit Configs.")]
        public RectTransform bonfireLitScreenRect;
        public LeanTweenType bonfireLitEaseType = LeanTweenType.linear;
        public float bonfireLitFadeInSpeed = 0.6f;
        public float bonfireLitFadeOutSpeed = 0.6f;
        #endregion

        #region Boss Victory Message.
        [Header("bonfire Lit Configs.")]
        public RectTransform bossVictoryScreenRect;
        public LeanTweenType bossVictoryEaseType = LeanTweenType.linear;
        public float bossVictoryFadeInSpeed = 0.6f;
        public float bossVictoryFadeOutSpeed = 0.6f;
        #endregion

        #endregion

        #region Refs.

        #region Managers.
        [NonSerialized] public SavableInventory _inventory;
        [NonSerialized] public StatsAttributeHandler _statsHandler;
        [NonSerialized] public StateManager _states;
        #endregion

        #region Canvases.
        /// Highlighter.
        Canvas leftWeaponHighlighterCanvas;
        Canvas rightWeaponHighlighterCanvas;
        Canvas spellWeaponHighlighterCanvas;
        Canvas consumableHighlighterCanvas;

        /// Mini Slots.
        Canvas preConsumableQSlotCanvas;
        Canvas nextConsumableQSlotCanvas;

        Canvas _mainHudCanvas;
        Canvas _damagedScreenCanvas;
        Canvas _deathScreenCanvas;
        Canvas _bonfireLitMessageCanvas;
        Canvas _bossVictoryMessageCanvas;
        #endregion

        #region RectTransforms.
        RectTransform healthSliderRect;
        RectTransform healthVisSliderRect;
        RectTransform focusSliderRect;
        RectTransform focusVisSliderRect;
        RectTransform staminaSliderRect;
        RectTransform staminaVisSliderRect;
        RectTransform deathScreenRect;
        #endregion
        
        #endregion

        #region Privates.
        [HideInInspector] public float _delta;
        int _cur_mainHudTweenId;

        int leftWeaponQSlotTweenId;
        int rightWeaponQSlotTweenId;
        int spellWeaponQSlotTweenId;
        int consumableQSlotTweenId;

        int preConsumableShowTweenId;
        int preConsumableHideTweenId;
        int nextConsumableShowTweenId;
        int nextConsumableHideTweenId;

        RectTransform.Axis slider_h_axis;
        #endregion 
        
        public void LateTick()
        {
            UpdateHealthSlider();

            UpdateStaminaVisualSlider();

            UpdateFocusSlider();

            MonitorInfoCard();

            DamagePreviewerLateTick();
        }

        #region SHOW / HIDE HUD.
        public void ShowMainHud()
        {
            CancelUnFinishTweening();

            EnableMainHudCanvas();
            _cur_mainHudTweenId = LeanTween.alphaCanvas(mainHudGroup, 1, mainHudFadeInSpeed).setEase(mainHudEaseType).id;
        }

        public void HideMainHud()
        {
            CancelUnFinishTweening();

            _cur_mainHudTweenId = LeanTween.alphaCanvas(mainHudGroup, 0, mainHudFadeOutSpeed).setEase(mainHudEaseType).setOnComplete(DisableMainHudeCanvas).id;
        }

        void CancelUnFinishTweening()
        {
            if (LeanTween.isTweening(_cur_mainHudTweenId))
                LeanTween.cancel(_cur_mainHudTweenId);
        }

        void EnableMainHudCanvas()
        {
            _mainHudCanvas.enabled = true;
        }

        void DisableMainHudeCanvas()
        {
            _mainHudCanvas.enabled = false;
        }
        #endregion

        #region Reflect Modified Changes.
        public void ReflectCurrentHealthChanges()
        {
            health.value = _statsHandler._hp;
        }

        public void ReflectCurrentStaminaChanges()
        {
            stamina.value = _statsHandler._stamina;
        }

        public void ReflectCurrentFocusChanges()
        {
            focus.value = _statsHandler._fp;
        }

        public void OnCheckpointEven_RefillFullPlayerStats()
        {
            ReflectCurrentHealthChanges();
            ReflectCurrentStaminaChanges();
            ReflectCurrentFocusChanges();
        }
        #endregion

        #region STATS SLIDER.
        void UpdateHealthSlider()
        {
            if (h_vis.value != _statsHandler._hp)
            {
                h_vis.value = Mathf.Lerp(h_vis.value, _statsHandler._hp, healthSliderLerpSpeed);
                if ((h_vis.value - _statsHandler._hp) < 0.5f)
                {
                    h_vis.value = _statsHandler._hp;
                }
            }
        }
        
        void UpdateStaminaVisualSlider()
        {
            if (s_vis.value != _statsHandler._stamina)
            {
                s_vis.value = Mathf.Lerp(s_vis.value, stamina.value, staminaSliderLerpSpeed);
                if ((s_vis.value - stamina.value) < 0.5f)
                {
                    s_vis.value = stamina.value;
                }
            }
        }
        
        void UpdateFocusSlider()
        {
            if (f_vis.value != _statsHandler._fp)
            {
                f_vis.value = Mathf.Lerp(f_vis.value, _statsHandler._fp, healthSliderLerpSpeed);
                if ((f_vis.value - _statsHandler._fp) < 0.5f)
                {
                    f_vis.value = _statsHandler._fp;
                }
            }
        }
        
        public void UpdateSliderWidthByType(StatSliderEnum _statSliderType)
        {
            switch (_statSliderType)
            {
                case StatSliderEnum.health:
                    health.maxValue = _statsHandler.b_hp;
                    h_vis.maxValue = _statsHandler.b_hp;

                    healthSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, health.maxValue * sliderSizeMultiplier);
                    healthVisSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, h_vis.maxValue * sliderSizeMultiplier);
                    break;

                case StatSliderEnum.focus:
                    focus.maxValue = _statsHandler.b_fp;
                    f_vis.maxValue = _statsHandler.b_fp;

                    focusSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, focus.maxValue * sliderSizeMultiplier);
                    focusVisSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, f_vis.maxValue * sliderSizeMultiplier);
                    break;

                case StatSliderEnum.stamina:
                    //float _curMaxBaseStamina = (int);
                    stamina.maxValue = _statsHandler.b_stamina;
                    s_vis.maxValue = _statsHandler.b_stamina;

                    staminaSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, stamina.maxValue * sliderSizeMultiplier);
                    staminaVisSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, s_vis.maxValue * sliderSizeMultiplier);
                    break;
            }
        }
        
        public void OnStaminaSliderWaitForRecover()
        {
            LeanTween.value(staminaSliderFillImage.gameObject, 0.1f, 1, staminaColorChangeSpeed)
                .setEaseInOutCirc()
                .setOnUpdate((value) => 
                {
                    staminaSliderFillImage.color = Color.Lerp(staminaDefaultColor, staminaWaitForRecoverColor, value);
                }
                );
        }

        public void OffStaminaSliderWaitForRecover()
        {
            LeanTween.value(staminaSliderFillImage.gameObject, 0.1f, 1, staminaColorChangeSpeed)
                .setEaseInOutCirc()
                .setOnUpdate((value) =>
                {
                    staminaSliderFillImage.color = Color.Lerp(staminaWaitForRecoverColor, staminaDefaultColor, value);
                }
                );
        }
        #endregion

        #region Volun Text.
        public void RefreshVolunTextWithoutEffect()
        {
            _volunSlotValue = _statsHandler.voluns;
            _volunSlotText.text = _volunSlotValue.ToString();
        }

        public void RefreshVolunTextWithEffect()
        {
            if (CountingCoroutine != null)
            {
                StopCoroutine(CountingCoroutine);
            }

            CountingCoroutine = StartCoroutine(EditTextWithCorountine(_statsHandler.voluns));

            _volunSlotValue = _statsHandler.voluns;
        }

        IEnumerator EditTextWithCorountine(int newValue)
        {
            WaitForSeconds Wait = new WaitForSeconds(1f / CountFPS);
            int previousValue = _volunSlotValue;
            int stepAmount;

            if (newValue - previousValue < 0)
            {
                stepAmount = Mathf.FloorToInt((newValue - previousValue) / (CountFPS * Duration));  /// newValue = -20, previousValue = 0, CountFPS = 30, and Duration = 1; (-20 - 0) / (30 * 1) // -0.6667 (CeilToInt) -> 0
            }
            else
            {
                stepAmount = Mathf.CeilToInt((newValue - previousValue) / (CountFPS * Duration)); /// newValue = 20, previousValue = 0, CountFPS = 30, and Duration = 1; (20 - 0) / (30 * 1) // 0.6667 (FloorToInt) -> 0
            }

            if (previousValue < newValue)
            {
                while(previousValue < newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue > newValue)
                    {
                        previousValue = newValue;
                    }

                    _volunSlotText.text = previousValue.ToString("N0");
                    yield return Wait;
                }
            }
            else
            {
                while (previousValue > newValue)
                {
                    previousValue += stepAmount;
                    if (previousValue < newValue)
                    {
                        previousValue = newValue;
                    }

                    _volunSlotText.text = previousValue.ToString("N0");
                    yield return Wait;
                }
            }
        }
        #endregion

        #region Leveling Menu.
        public void OnLevelingConfirm_RefreshAllSliders()
        {
            RefreshVolunTextWithoutEffect();

            UpdateSliderWidthByType(StatSliderEnum.health);
            ReflectCurrentHealthChanges();

            UpdateSliderWidthByType(StatSliderEnum.focus);
            ReflectCurrentFocusChanges();

            UpdateSliderWidthByType(StatSliderEnum.stamina);
            ReflectCurrentStaminaChanges();
        }
        #endregion

        #region Effect Grid.
        public void RegisterStatsEffectIcon(int _id, Sprite _iconSprite)
        {
            EffectIcon _effectIcon = statsEffectIcons[_id];
            _effectIcon._iconImage.sprite = _iconSprite;
            _effectIcon.EnableIcon();

            LeanTween.alpha(_effectIcon._iconRect, 1, statsEffectIconFadeTime).setEase(_iconEaseType);
        }

        public void UnRegisterStatsEffectIcon(int _id)
        {
            EffectIcon _effectIcon = statsEffectIcons[_id];
            LeanTween.alpha(_effectIcon._iconRect, 0, statsEffectIconFadeTime).setEase(_iconEaseType).setOnComplete(_effectIcon.DisableIcon);
        }

        public void RegisterWeaponBuffEffectIcon(int _id, Sprite _iconSprite)
        {
            EffectIcon _effectIcon = weaponBuffEffectIcons[_id];
            _effectIcon._iconImage.sprite = _iconSprite;
            _effectIcon.EnableIcon();

            LeanTween.alpha(_effectIcon._iconRect, 1, weaponBuffIconFadeTime).setEase(_iconEaseType);
        }

        public void UnRegisterWeaponBuffEffectIcon(int _id)
        {
            EffectIcon _effectIcon = weaponBuffEffectIcons[_id];
            LeanTween.alpha(_effectIcon._iconRect, 0, weaponBuffIconFadeTime).setEase(_iconEaseType).setOnComplete(_effectIcon.DisableIcon);
        }

        public void RegisterFighterModeIcon()
        {
            fighterModeEffectIcon._iconImage.sprite = fighterModeSprite;
            fighterModeEffectIcon.EnableIcon();

            LeanTween.alpha(fighterModeEffectIcon._iconRect, 1, weaponBuffIconFadeTime).setEase(_iconEaseType);
        }

        public void UnRegisterFighterModeIcon()
        {
            LeanTween.alpha(fighterModeEffectIcon._iconRect, 0, weaponBuffIconFadeTime).setEase(_iconEaseType).setOnComplete(fighterModeEffectIcon.DisableIcon);
        }
        #endregion

        #region REGISTER WEAPON HUD.

        #region Right.
        public void RegisterRhWeaponQSlot()
        {
            WeaponItem _rightHandWeapon = _inventory._rightHandWeapon_referedItem;

            rightWeaponIcon.enabled = true;
            RegisterRhWeaponIcon();
            RegisterRhWeaponText();
            Register_1H_RhActionStickers();

            void RegisterRhWeaponIcon()
            {
                rightWeaponIcon.sprite = _rightHandWeapon.itemIcon;
            }

            void RegisterRhWeaponText()
            {
                rightWeaponNameText.text = _inventory._rightHandWeapon.runtimeName;
            }

            void Register_1H_RhActionStickers()
            {
                _rh_rb_actionSticker._stickerIcon.sprite = _rightHandWeapon._1HandedRbActionSprite;
                _rh_rt_actionSticker._stickerIcon.sprite = _rightHandWeapon._1HandedRtActionSprite;
            }
        }
        #endregion

        #region Left.
        public void RegisterLhWeaponQSlot()
        {
            WeaponItem _leftHandWeapon = _inventory._leftHandWeapon_referedItem;

            leftWeaponIcon.enabled = true;
            RegisterLhWeaponIcon();
            RegisterLhWeaponText();
            Register_1H_LhActionStickers();

            void RegisterLhWeaponIcon()
            {
                leftWeaponIcon.sprite = _leftHandWeapon.itemIcon;
            }

            void RegisterLhWeaponText()
            {
                leftWeaponNameText.text = _inventory._leftHandWeapon.runtimeName;
            }

            void Register_1H_LhActionStickers()
            {
                _lh_lb_actionSticker._stickerIcon.sprite = _leftHandWeapon._1HandedLbActionSprite;
                _lh_lt_actionSticker._stickerIcon.sprite = _leftHandWeapon._1HandedLtActionSprite;
            }
        }
        #endregion

        #endregion

        #region REGISTER / EMPTY CONSUMABLE ICONS
        /// REGISTER ICONS
        public void RegisterConsumQSlotIcon()
        {
            consumableIcon.enabled = true;

            if (_inventory._consumable.isCarryingEmpty)
            {
                consumableIcon.sprite = _inventory._consumable_referedItem.GetEmptyNoBaseConsumableSprite();
            }
            else
            {
                consumableIcon.sprite = _inventory._consumable_referedItem._noBaseItemIcon;
            }
        }

        public void ChangeConsumQSlotIconToEmpty()
        {
            consumableIcon.sprite = _inventory._consumable_referedItem.GetEmptyNoBaseConsumableSprite();
        }

        public void ChangeConsumQSlotIconToNormal()
        {
            consumableIcon.sprite = _inventory._consumable_referedItem._noBaseItemIcon;
        }
        
        /// EMPTY ICONS
        public void EmptyConsumQSlotIcon()
        {
            consumableIcon.enabled = false;
            consumableIcon.sprite = null;
        }
        #endregion

        #region ON / OFF QSLOT HIGHLIGHTER.

        #region Right Weapon.
        void EnableRightWeaponQSlotHighlighter()
        {
            rightWeaponHighlighterCanvas.enabled = true;
        }

        void DisableRightWeaponQSlotHighlighter()
        {
            rightWeaponHighlighterCanvas.enabled = false;
            LeanTween.alpha(rightWeaponHighlighterRect, qSlotHighlighterMaxFadeValue, 0);
        }

        public void ShowRightWeaponHighLighter()
        {
            if (LeanTween.isTweening(rightWeaponQSlotTweenId))
                LeanTween.cancel(rightWeaponQSlotTweenId);

            EnableRightWeaponQSlotHighlighter();
            rightWeaponQSlotTweenId = LeanTween.alpha(rightWeaponHighlighterRect, qSlotHighlighterMinFadeValue, qSlotHighlighterFadeTime).setEase(qSlotHighlighterEaseType).setOnComplete(DisableRightWeaponQSlotHighlighter).id;
        }
        #endregion

        #region Left Weapon.
        void EnableLeftWeaponQSlotHighlighter()
        {
            leftWeaponHighlighterCanvas.enabled = true;
        }

        void DisableLeftWeaponQSlotHighlighter()
        {
            leftWeaponHighlighterCanvas.enabled = false;
            LeanTween.alpha(leftWeaponHighlighterRect, qSlotHighlighterMaxFadeValue, 0);
        }

        public void ShowLeftWeaponHighLighter()
        {
            if (LeanTween.isTweening(leftWeaponQSlotTweenId))
                LeanTween.cancel(leftWeaponQSlotTweenId);

            EnableLeftWeaponQSlotHighlighter();
            leftWeaponQSlotTweenId = LeanTween.alpha(leftWeaponHighlighterRect, qSlotHighlighterMinFadeValue, qSlotHighlighterFadeTime).setEase(qSlotHighlighterEaseType).setOnComplete(DisableLeftWeaponQSlotHighlighter).id;
        }
        #endregion

        #region Consumable.
        void EnableConsumableQSlotHighlighter()
        {
            consumableHighlighterCanvas.enabled = true;
        }

        void DisableConsumableQSlotHighlighter()
        {
            consumableHighlighterCanvas.enabled = false;
            LeanTween.alpha(consumableHighlighterRect, qSlotHighlighterMaxFadeValue, 0);
        }

        public void ShowConsumableHighLighter()
        {
            if (LeanTween.isTweening(consumableQSlotTweenId))
                LeanTween.cancel(consumableQSlotTweenId);

            EnableConsumableQSlotHighlighter();
            consumableQSlotTweenId = LeanTween.alpha(consumableHighlighterRect, qSlotHighlighterMinFadeValue, qSlotHighlighterFadeTime).setEase(qSlotHighlighterEaseType).setOnComplete(DisableConsumableQSlotHighlighter).id;
        }
        #endregion

        #endregion

        #region On / Off Two Handing Left Hand Weapon.
        /// ON.
        public void OnTwoHandingLeftHandWeapon()
        {
            ShowLeftTwoHandingStatus();
            Move_LhWeaponIcon_ToTwoHandingPos();
            HideRightWeaponGroup();
            Register_2H_LhActionStickers();

            void Register_2H_LhActionStickers()
            {
                WeaponItem _leftHandWeapon = _inventory._leftHandWeapon_referedItem;
                
                _lh_rb_actionSticker._stickerCanvas.enabled = true;
                _lh_rb_actionSticker._stickerIcon.sprite = _leftHandWeapon._2HandedRbActionSprite;

                _lh_lb_actionSticker.MoveTo_2H_Position();
                _lh_lb_actionSticker._stickerIcon.sprite = _leftHandWeapon._2HandedLbActionSprite;

                _lh_rt_actionSticker._stickerCanvas.enabled = true;
                _lh_rt_actionSticker._stickerIcon.sprite = _leftHandWeapon._2HandedRtActionSprite;

                _lh_lt_actionSticker.MoveTo_2H_Position();
                _lh_lt_actionSticker._stickerIcon.sprite = _leftHandWeapon._2HandedLtActionSprite;
            }   
        }

        /// OFF.
        public void OffTwoHandingLeftHandWeapon()
        {
            HideLeftTwoHandingStatus();
            Move_LhWeaponIcon_ToOriginalPos();
            ShowRightWeaponGroup();
            UnRegister_2H_LhActionStickers();

            void UnRegister_2H_LhActionStickers()
            {
                _lh_rb_actionSticker._stickerCanvas.enabled = false;
                _lh_rt_actionSticker._stickerCanvas.enabled = false;

                _lh_lb_actionSticker.MoveTo_1H_Position();
                _lh_lb_actionSticker._stickerIcon.sprite = _inventory._leftHandWeapon_referedItem._1HandedLbActionSprite;

                _lh_lt_actionSticker.MoveTo_1H_Position();
                _lh_lt_actionSticker._stickerIcon.sprite = _inventory._leftHandWeapon_referedItem._1HandedLtActionSprite;
            }
        }
        #endregion

        #region On / Off Two Handing Right Hand Weapon.
        /// ON.
        public void OnTwoHandingRightHandWeapon()
        {
            ShowRightTwoHandingStatus();
            Move_RhWeaponIcon_ToTwoHandingPos();
            HideLeftWeaponGroup();
            Register_2H_RhActionStickers();

            void Register_2H_RhActionStickers()
            {
                WeaponItem _rightHandWeapon = _inventory._rightHandWeapon_referedItem;
                
                _rh_rb_actionSticker._stickerIcon.sprite = _rightHandWeapon._2HandedRbActionSprite;

                _rh_lb_actionSticker._stickerCanvas.enabled = true;
                _rh_lb_actionSticker._stickerIcon.sprite = _rightHandWeapon._2HandedLbActionSprite;

                _rh_rt_actionSticker.MoveTo_2H_Position(); 
                _rh_rt_actionSticker._stickerIcon.sprite = _rightHandWeapon._2HandedRtActionSprite;

                _rh_lt_actionSticker._stickerCanvas.enabled = true;
                _rh_lt_actionSticker._stickerIcon.sprite = _rightHandWeapon._2HandedLtActionSprite;
            }
        }

        /// OFF.
        public void OffTwoHandingRightHandWeapon()
        {
            HideRightTwoHandingStatus();
            Move_RhWeaponIcon_ToOriginalPos();
            ShowLeftWeaponGroup();
            UnRegister_2H_RhActionStickers();

            void UnRegister_2H_RhActionStickers()
            {
                _rh_lb_actionSticker._stickerCanvas.enabled = false;
                _rh_lt_actionSticker._stickerCanvas.enabled = false;
                
                _rh_rb_actionSticker._stickerIcon.sprite = _inventory._rightHandWeapon_referedItem._1HandedRbActionSprite;

                _rh_rt_actionSticker.MoveTo_1H_Position();
                _rh_rt_actionSticker._stickerIcon.sprite = _inventory._rightHandWeapon_referedItem._1HandedRtActionSprite;
            }
        }
        #endregion

        #region Show / Hide Two Handing Status.
        public void ShowLeftTwoHandingStatus()
        {
            EnableLeftTwoHandingCanvas();
            LeanTween.alpha(left_twoHandingIcon.rectTransform, 1, twoHandingFadeTime).setEase(twoHandingFadeEaseType);

            void EnableLeftTwoHandingCanvas()
            {
                left_twoHandingIconCanvas.enabled = true;
            }
        }

        public void HideLeftTwoHandingStatus()
        {
            LeanTween.alpha(left_twoHandingIcon.rectTransform, 0, twoHandingFadeTime).setEase(twoHandingFadeEaseType).setOnComplete(OnCompleteHideTwoHandingStatus);

            void OnCompleteHideTwoHandingStatus()
            {
                left_twoHandingIconCanvas.enabled = false;
            }
        }

        public void ShowRightTwoHandingStatus()
        {
            EnableRightTwoHandingCanvas();
            LeanTween.alpha(right_twoHandingIcon.rectTransform, 1, twoHandingFadeTime).setEase(twoHandingFadeEaseType);

            void EnableRightTwoHandingCanvas()
            {
                right_twoHandingIconCanvas.enabled = true;
            }
        }

        public void HideRightTwoHandingStatus()
        {
            LeanTween.alpha(right_twoHandingIcon.rectTransform, 0, twoHandingFadeTime).setEase(twoHandingFadeEaseType).setOnComplete(OnCompleteHideRightTwoHandingStatus);

            void OnCompleteHideRightTwoHandingStatus()
            {
                right_twoHandingIconCanvas.enabled = false;
            }
        }
        #endregion

        #region Move Weapon Icon Two Handing Status.
        public void Move_LhWeaponIcon_ToTwoHandingPos()
        {
            Vector3 _tempPos = leftWeaponIcon.rectTransform.localPosition;
            _tempPos.x = 6.5f;
            leftWeaponIcon.rectTransform.localPosition = _tempPos;
            //LeanTween.moveX(leftWeaponIcon.rectTransform, 6.5f, 0.1f).setEaseOutCubic();
        }

        public void Move_LhWeaponIcon_ToOriginalPos()
        {
            Vector3 _tempPos = leftWeaponIcon.rectTransform.localPosition;
            _tempPos.x = 0f;
            leftWeaponIcon.rectTransform.localPosition = _tempPos;
            //LeanTween.moveX(leftWeaponIcon.rectTransform, 0, 0.1f).setEaseOutCubic();
        }

        public void Move_RhWeaponIcon_ToTwoHandingPos()
        {
            Vector3 _tempPos = rightWeaponIcon.rectTransform.localPosition;
            _tempPos.x = -6.5f;
            rightWeaponIcon.rectTransform.localPosition = _tempPos;
            //LeanTween.moveX(rightWeaponIcon.rectTransform, -6.5f, 0.1f).setEaseOutCubic();
        }

        public void Move_RhWeaponIcon_ToOriginalPos()
        {
            Vector3 _tempPos = rightWeaponIcon.rectTransform.localPosition;
            _tempPos.x = 0f;
            rightWeaponIcon.rectTransform.localPosition = _tempPos;
            //LeanTween.moveX(rightWeaponIcon.rectTransform, 0, 0.1f).setEaseOutCubic();
        }
        #endregion

        #region Show / Hide Weapon Group.
        public void ShowLeftWeaponGroup()
        {
            EnableLeftWeaponGroupCanvas();
            LeanTween.alphaCanvas(leftWeaponGroup, 1, weaponGroupFadeTime).setEase(weaponGroupEaseType);

            void EnableLeftWeaponGroupCanvas()
            {
                leftWeaponGroupCanvas.enabled = true;
            }
        }

        public void HideLeftWeaponGroup()
        {
            LeanTween.alphaCanvas(leftWeaponGroup, 0, weaponGroupFadeTime).setEase(weaponGroupEaseType).setOnComplete(OnCompleteHideLeftWeaponGroup);

            void OnCompleteHideLeftWeaponGroup()
            {
                leftWeaponGroupCanvas.enabled = false;
            }
        }

        public void ShowRightWeaponGroup()
        {
            EnableRightWeaponGroupCanvas();
            LeanTween.alphaCanvas(rightWeaponGroup, 1, weaponGroupFadeTime).setEase(weaponGroupEaseType);

            void EnableRightWeaponGroupCanvas()
            {
                rightWeaponGroupCanvas.enabled = true;
            }
        }

        public void HideRightWeaponGroup()
        {
            LeanTween.alphaCanvas(rightWeaponGroup, 0, weaponGroupFadeTime).setEase(weaponGroupEaseType).setOnComplete(OnCompleteHideRightWeaponGroup);

            void OnCompleteHideRightWeaponGroup()
            {
                rightWeaponGroupCanvas.enabled = false;
            }
        }
        #endregion

        #region ON / OFF MINI QSLOT.
        public void ShowNextConsumableQSlot(RuntimeConsumable _runtimeConsumable)
        {
            if (LeanTween.isTweening(nextConsumableHideTweenId))
                LeanTween.cancel(nextConsumableHideTweenId);

            EnableNextConsumableQSlot();
            RegisterNextConsumableQSlotIcon(_runtimeConsumable);
            nextConsumableShowTweenId = LeanTween.alpha(nextConsumableQSlotIcon.rectTransform, 1, miniQSlotFadeSpeed).setEase(miniQSlotFadeEaseType).id;

            void EnableNextConsumableQSlot()
            {
                nextConsumableQSlotCanvas.enabled = true;
            }
        }

        public void HideNextConsumableQSlot()
        {
            if (LeanTween.isTweening(nextConsumableShowTweenId))
                LeanTween.cancel(nextConsumableShowTweenId);

            nextConsumableHideTweenId = LeanTween.alpha(nextConsumableQSlotIcon.rectTransform, 0, miniQSlotFadeSpeed).setEase(miniQSlotFadeEaseType).setOnComplete(OnCompleteHideNextConsumableQSlot).id;

            void OnCompleteHideNextConsumableQSlot()
            {
                nextConsumableQSlotCanvas.enabled = false;
            }
        }

        public void Show2ndNextConsumableQSlot(RuntimeConsumable _runtimeConsumable)
        {
            if (LeanTween.isTweening(preConsumableHideTweenId))
                LeanTween.cancel(preConsumableHideTweenId);

            EnablePreConsumableQSlot();
            RegisterPreConsumableQSlotIcon(_runtimeConsumable);
            preConsumableShowTweenId = LeanTween.alpha(secondNextConsumableQSlotIcon.rectTransform, 1, miniQSlotFadeSpeed).setEase(miniQSlotFadeEaseType).id;

            void EnablePreConsumableQSlot()
            {
                preConsumableQSlotCanvas.enabled = true;
            }
        }

        public void Hide2ndNextConsumableQSlot()
        {
            if (LeanTween.isTweening(preConsumableShowTweenId))
                LeanTween.cancel(preConsumableShowTweenId);

            preConsumableHideTweenId = LeanTween.alpha(secondNextConsumableQSlotIcon.rectTransform, 0, miniQSlotFadeSpeed).setEase(miniQSlotFadeEaseType).setOnComplete(OnCompleteHidePreConsumableQSlot).id;

            void OnCompleteHidePreConsumableQSlot()
            {
                preConsumableQSlotCanvas.enabled = false;
            }
        }
        #endregion

        #region REGISTER / DISABLE MINI QSLOT ICON.
        public void RegisterPreConsumableQSlotIcon(RuntimeConsumable _runtimeConsumable)
        {
            if (_runtimeConsumable.isCarryingEmpty)
            {
                secondNextConsumableQSlotIcon.sprite = _runtimeConsumable._baseConsumableItem.GetEmptyNoBaseConsumableSprite();
            }
            else
            {
                secondNextConsumableQSlotIcon.sprite = _runtimeConsumable._baseConsumableItem._noBaseItemIcon;
            }
        }
        
        public void RegisterNextConsumableQSlotIcon(RuntimeConsumable _runtimeConsumable)
        {
            if (_runtimeConsumable.isCarryingEmpty)
            {
                nextConsumableQSlotIcon.sprite = _runtimeConsumable._baseConsumableItem.GetEmptyNoBaseConsumableSprite();
            }
            else
            {
                nextConsumableQSlotIcon.sprite = _runtimeConsumable._baseConsumableItem._noBaseItemIcon;
            }
        }

        #endregion

        #region SHOW / HIDE INTERACTION CARDS.
        public void ShowInteractionCard()
        {
            if (_current_inter_content != _next_inter_content)
            {
                if (_current_inter_content)
                {
                    DeactivateInteractionCardContent(_current_inter_content);
                }

                _current_inter_content = _next_inter_content;
                _next_inter_content = null;

                ActivateInteractionCardContent();
            }

            if (!_isInteractionCardShown)
            {
                _interactionCard.ShowInteractionCard();
                _isInteractionCardShown = true;
            }
        }

        public void HideInteractionCard_FadeOut()
        {
            _isInteractionCardShown = false;
            _interactionCard.HideInterCard_FadeOut_WithOnCompleteAction(OnCompleteDeactivateInterContent);
        }

        public void HideInteractionCard_MoveOut()
        {
            _isInteractionCardShown = false;
            _interactionCard.HideInterCard_MoveOut_WithOnCompleteAction(OnCompleteDeactivateInterContent);
        }
        
        void OnCompleteDeactivateInterContent()
        {
            // Card Canvas.
            _interactionCard.DisableCardCanvas();

            // Card Content.
            if (_current_inter_content != null)
            {
                DeactivateInteractionCardContent(_current_inter_content);
                _current_inter_content = null;
            }
        }

        void DeactivateInteractionCardContent(Canvas _content)
        {
            _content.enabled = false;
            _content.gameObject.SetActive(false);
        }

        void ActivateInteractionCardContent()
        {
            _current_inter_content.enabled = true;
            _current_inter_content.gameObject.SetActive(true);
        }
        
        #region Set Next Inter Content.
        public void SetNextInterContent_Pickup()
        {
            _next_inter_content = pickup_inter_content;
            ShowInteractionCard();
        }

        public void SetNextInterContent_OpenDoor()
        {
            _next_inter_content = openDoor_inter_content;
            ShowInteractionCard();
        }

        public void SetNextInterContent_OpenChest()
        {
            _next_inter_content = openChest_inter_content;
            ShowInteractionCard();
        }

        public void SetNextInterContent_TakeChest()
        {
            _next_inter_content = takeChest_inter_content;
            ShowInteractionCard();
        }

        public void SetNextInterContent_Ignite()
        {
            _next_inter_content = activate_inter_content;
            ShowInteractionCard();
        }

        public void SetNextInterContent_Rest()
        {
            _next_inter_content = rest_inter_content;
            ShowInteractionCard();
        }

        public void SetNextInterContentByType(PlayerInteractable.InteractionTypeEnum _interactableType)
        {
            switch (_interactableType)
            {
                case PlayerInteractable.InteractionTypeEnum.Pickup:
                    _next_inter_content = pickup_inter_content;
                    break;
                case PlayerInteractable.InteractionTypeEnum.Talk:
                    _next_inter_content = talk_inter_content;
                    break;
                case PlayerInteractable.InteractionTypeEnum.OpenDoor:
                    _next_inter_content = openDoor_inter_content;
                    break;
                case PlayerInteractable.InteractionTypeEnum.OpenChest:
                    _next_inter_content = openChest_inter_content;
                    break;
                case PlayerInteractable.InteractionTypeEnum.TakeChest:
                    _next_inter_content = takeChest_inter_content;
                    break;
                case PlayerInteractable.InteractionTypeEnum.Ignite:
                    _next_inter_content = activate_inter_content;
                    break;
                case PlayerInteractable.InteractionTypeEnum.Rest:
                    _next_inter_content = rest_inter_content;
                    break;
            }

            ShowInteractionCard();
        }
        #endregion

        #endregion

        #region SHOW / HIDE EXECUTION CARDS.
        public void ShowExecutionCard()
        {
            EnableParryExecuteContent();
            _executionCard.ShowInteractionCard();
        }

        public void HideExecutionCard_FadeOut()
        {
            DisableParryExecuteContent();
            _executionCard.HideInterCard_FadeOut_WithOnCompleteAction(DisableParryExecuteContent);
        }

        public void HideExecutionCard_MoveOut()
        {
            DisableParryExecuteContent();
            _executionCard.HideInterCard_MoveOut_WithOnCompleteAction(DisableParryExecuteContent);
        }

        #region Execution Card Content.
        void EnableParryExecuteContent()
        {
            parryExe_content.enabled = true;
            parryExe_content.gameObject.SetActive(true);
        }

        void DisableParryExecuteContent()
        {
            parryExe_content.enabled = false;
            parryExe_content.gameObject.SetActive(false);
        }
        #endregion

        #endregion

        #region REGISTER / UNREGISTER INFO CARDS.
        void MonitorInfoCard()
        {
            if (_isInfoCardDisplaying)
            {
                _infoCardDisplayTimer += _delta;
                if (_infoCardDisplayTimer > _infoCardDisplayRate)
                {
                    _infoCardDisplayTimer = 0;
                    _isInfoCardDisplaying = false;

                    HideItemInfoCard();
                }
            }
        }

        void HideItemInfoCard()
        {
            _itemInfoCard.OffInfoCard();
        }
        #endregion
        
        #region SHOW DAMAGED SCREEN.
        public void ShowDamagedScreen()
        {
            EnableDamagedScreen();
            LeanTween.alpha(damagedScreenRect, 1, damageScreenFadeSpeed).setOnComplete(HideDamagedScreen);
        }

        public void ShowDamagedScreen_RegisterDamagePreviwer()
        {
            ShowDamagedScreen();
            RegisterDamagePreviewer();
        }

        void HideDamagedScreen()
        {
            LeanTween.alpha(damagedScreenRect, 0, damageScreenFadeSpeed).setOnComplete(DisableDamagedScreen);
        }

        void EnableDamagedScreen()
        {
            _damagedScreenCanvas.enabled = true;
        }

        void DisableDamagedScreen()
        {
            _damagedScreenCanvas.enabled = false;
        }
        #endregion

        #region Damage Previewer.
        public void RegisterDamagePreviewer()
        {
            damagePreviewer.RegisterDamagePreviewer();
        }

        void DamagePreviewerLateTick()
        {
            damagePreviewer.LateTick();
        }
        #endregion

        #region SHOW DEATH SCREENS.
        public void ShowDeathScreen()
        {
            _deathScreenCanvas.enabled = true;
            LeanTween.alpha(deathScreenRect, 1, deathScreenFadeInSpeed).setEase(deathScreenEaseType).setOnComplete(StartDelaySwitchDeathSpriteCount);
        }

        void StartDelaySwitchDeathSpriteCount()
        {
            LeanTween.delayedCall(switchDeathScreenWaitTime, SwitchDeathSpriteWhenComplete);
        }

        void SwitchDeathSpriteWhenComplete()
        {
            deathScreenImage.sprite = deathScreen_FullTrans_Sprite;
        }

        public void HideDeathScreen()
        {
            LeanTween.alpha(deathScreenRect, 0, 0).setOnComplete(DisableDeathScreen);
        }

        void DisableDeathScreen()
        {
            deathScreenImage.sprite = deathScreen_NoTrans_Sprite;
            _deathScreenCanvas.enabled = false;
        }
        #endregion

        #region SHOW / HIDE BONFIRE MESSAGE.
        public void ShowBonfireLitScreen()
        {
            EnableBonfireLitScreen();
            LeanTween.alpha(bonfireLitScreenRect, 1, bonfireLitFadeInSpeed).setEase(bonfireLitEaseType).setOnComplete(HideBonfireLitScreen);
        }

        public void HideBonfireLitScreen()
        {
            LeanTween.alpha(bonfireLitScreenRect, 0, bonfireLitFadeOutSpeed).setEase(bonfireLitEaseType).setDelay(1f).setOnComplete(DisableBonfireLitScreen);
        }
        
        void EnableBonfireLitScreen()
        {
            _bonfireLitMessageCanvas.enabled = true;
        }

        void DisableBonfireLitScreen()
        {
            _bonfireLitMessageCanvas.enabled = false;
        }
        #endregion

        #region SHOW / HIDE BOSS VICTORY MESSAGE.
        public void ShowBossVictoryScreen()
        {
            EnableBossVictoryScreen();
            LeanTween.alpha(bossVictoryScreenRect, 1, bossVictoryFadeInSpeed).setEase(bossVictoryEaseType).setOnComplete(HideBossVictoryScreen);
        }

        public void HideBossVictoryScreen()
        {
            LeanTween.alpha(bossVictoryScreenRect, 0, bossVictoryFadeOutSpeed).setEase(bossVictoryEaseType).setDelay(1.5f).setOnComplete(DisableBossVictoryScreen);
        }

        void EnableBossVictoryScreen()
        {
            _bossVictoryMessageCanvas.enabled = true;
        }

        void DisableBossVictoryScreen()
        {
            _bossVictoryMessageCanvas.enabled = false;
        }
        #endregion

        #region Setup.
        public void SetupManagerReferences(StateManager _states)
        {
            /// This runs before Setup().
            this._states = _states;
            _statsHandler = _states.statsHandler;
            _inventory = _states._savableInventory;
        }

        public void Setup()
        {
            SetupCanvases();

            SetupPlayerStatsSliders();
            SetupVolunText();
            SetupEffectIcons();

            SetupQSlotIcons();
            SetupQSlotHighlighters();

            SetupMiniQSlots();

            SetupInteractionCards();
            SetupInfoCard();

            SetupExecutionCard();

            SetupDamagedScreen();
            SetupDeathScreen();
            SetupBonfireLitScreenScreen();

            SetupDamagePreviewer();
        }

        void SetupCanvases()
        {
            leftWeaponHighlighterCanvas = leftWeaponHighlighterRect.GetComponent<Canvas>();
            rightWeaponHighlighterCanvas = rightWeaponHighlighterRect.GetComponent<Canvas>();
            consumableHighlighterCanvas = consumableHighlighterRect.GetComponent<Canvas>();
            
            preConsumableQSlotCanvas = secondNextConsumableQSlotIcon.GetComponent<Canvas>();
            nextConsumableQSlotCanvas = nextConsumableQSlotIcon.GetComponent<Canvas>();

            _mainHudCanvas = mainHudGroup.GetComponent<Canvas>();
            _damagedScreenCanvas = damagedScreenRect.GetComponent<Canvas>();
            _deathScreenCanvas = deathScreenImage.GetComponent<Canvas>();
            _bonfireLitMessageCanvas = bonfireLitScreenRect.GetComponent<Canvas>();
            _bossVictoryMessageCanvas = bossVictoryScreenRect.GetComponent<Canvas>();
        }

        void SetupPlayerStatsSliders()
        {
            GetSliderRectReferences();

            SetupSliderWidth();
            SetupSliderWidth();
            SetupSliderWidth();

            void SetupSliderWidth()
            {
                #region Health
                float _curMaxBaseHealth = (int)_statsHandler.b_hp;
                health.maxValue = _curMaxBaseHealth;
                h_vis.maxValue = _curMaxBaseHealth;
                h_vis.value = _curMaxBaseHealth;

                healthSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, health.maxValue * sliderSizeMultiplier);
                healthVisSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, h_vis.maxValue * sliderSizeMultiplier);
                #endregion

                #region Focus.
                float _curMaxBaseFocus = (int)_statsHandler.b_fp;
                focus.maxValue = _curMaxBaseFocus;
                f_vis.maxValue = _curMaxBaseFocus;
                f_vis.value = _curMaxBaseFocus;

                focusSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, focus.maxValue * sliderSizeMultiplier);
                focusVisSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, f_vis.maxValue * sliderSizeMultiplier);
                #endregion

                #region Stamina.
                float _curMaxBaseStamina = (int)_statsHandler.b_stamina;
                stamina.maxValue = _curMaxBaseStamina;
                s_vis.maxValue = _curMaxBaseStamina;
                s_vis.value = _curMaxBaseStamina;

                staminaSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, stamina.maxValue * sliderSizeMultiplier);
                staminaVisSliderRect.SetSizeWithCurrentAnchors(slider_h_axis, s_vis.maxValue * sliderSizeMultiplier);
                #endregion
            }
        }

        void SetupVolunText()
        {
            RefreshVolunTextWithoutEffect();
        }

        void GetSliderRectReferences()
        {
            healthSliderRect = health.GetComponent<RectTransform>();
            healthVisSliderRect = h_vis.GetComponent<RectTransform>();

            focusSliderRect = focus.GetComponent<RectTransform>();
            focusVisSliderRect = f_vis.GetComponent<RectTransform>();

            staminaSliderRect = stamina.GetComponent<RectTransform>();
            staminaVisSliderRect = s_vis.GetComponent<RectTransform>();

            slider_h_axis = RectTransform.Axis.Horizontal;
        }

        void SetupEffectIcons()
        {
            for (int i = 0; i < statsEffectIcons.Length; i++)
            {
                statsEffectIcons[i]._mainHudManager = this;
                statsEffectIcons[i].Init();
            }

            for (int i = 0; i < weaponBuffEffectIcons.Length; i++)
            {
                weaponBuffEffectIcons[i]._mainHudManager = this;
                weaponBuffEffectIcons[i].Init();
            }

            fighterModeEffectIcon._mainHudManager = this;
            fighterModeEffectIcon.Init();
        }

        void SetupQSlotIcons()
        {
            EmptyConsumQSlotIcon();
        }

        void SetupQSlotHighlighters()
        {
            DisableLeftWeaponQSlotHighlighter();
            DisableRightWeaponQSlotHighlighter();
            DisableConsumableQSlotHighlighter();
        }

        void SetupMiniQSlots()
        {
            preConsumableQSlotCanvas.enabled = false;
            nextConsumableQSlotCanvas.enabled = false;
        }
        
        void SetupInteractionCards()
        {
            _interactionCard.Setup();
        }
        
        void SetupInfoCard()
        {
            _itemInfoCard.Init(this);
        }

        void SetupExecutionCard()
        {
            _executionCard.Setup();
        }

        void SetupDamagedScreen()
        {
            DisableDamagedScreen();
        }

        void SetupDeathScreen()
        {
            deathScreenRect = deathScreenImage.rectTransform;
            DisableDeathScreen();
        }

        void SetupBonfireLitScreenScreen()
        {
            DisableBonfireLitScreen();
        }

        void SetupDamagePreviewer()
        {
            damagePreviewer.Setup(_states);
        }
        #endregion
        
        public enum StatSliderEnum
        {
            health,
            focus,
            stamina
        }
    }
}