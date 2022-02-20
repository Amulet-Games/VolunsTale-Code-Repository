using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SA
{
    public class ItemHub : MonoBehaviour
    {
        #region SideHub Tween.
        [Header("Status.")]
        public LeanTweenType _sideHubEaseType = LeanTweenType.easeOutCirc;
        public float _sideHubFadeInSpeed;
        public float _sideHubFadeOutSpeed;
        #endregion

        #region Sprites.
        [Header("Damages Sprites (Drops).")]
        public Sprite physical_damage_sprite;
        public Sprite magic_damage_sprite;
        public Sprite fire_damage_sprite;
        public Sprite lightning_damage_sprite;
        public Sprite dark_damage_sprite;

        [Header("Defense Sprites (Drops).")]
        public Sprite physical_defense_sprite;
        public Sprite magic_defense_sprite;
        public Sprite fire_defense_sprite;
        public Sprite lightning_defense_sprite;
        public Sprite dark_defense_sprite;

        [Header("Attribute Sprites (Drops).")]
        public Sprite str_sprite;
        public Sprite hex_sprite;
        public Sprite int_sprite;
        public Sprite div_sprite;

        [Header("Damage Reduct Sprites (Drops).")]
        public Sprite physical_dmg_reduct_sprite;
        public Sprite magic_dmg_reduct_sprite;
        public Sprite fire_dmg_reduct_sprite;
        public Sprite lightning_dmg_reduct_sprite;
        public Sprite dark_dmg_reduct_sprite;

        [Header("VS Reduct Sprites (Drops).")]
        public Sprite strike_vs_reduct_sprite;
        public Sprite slash_vs_reduct_sprite;
        public Sprite thrust_vs_reduct_sprite;

        [Header("Status Resis Sprites (Drops).")]
        public Sprite bleed_resis_sprite;
        public Sprite poison_resis_sprite;
        public Sprite frost_resis_sprite;
        public Sprite curse_resis_sprite;
        #endregion

        #region TMP Assets.
        [Header("Font Asset.")]
        public TMP_FontAsset arbutusSlab_normal_asset;
        public TMP_FontAsset century_normal_asset;
        #endregion

        #region Info Hub Drops.
        [Header("Info Hub (Drops).")]
        [SerializeField] CanvasGroup infoHubGroup;
        [SerializeField] Canvas infoHubCanvas;
        #endregion

        #region Alter Hub Drops.
        [Header("Alter Hub (Drops).")]
        [SerializeField] CanvasGroup alterHubGroup;
        [SerializeField] Canvas alterHubCanvas;
        #endregion
        
        #region Loaded Info.
        [Header("Loaded Info (Drops).")]
        [SerializeField] MeleeInfoDetails meleeInfoDetails;
        [SerializeField] CatalystsInfoDetails catalystsInfoDetails;
        [SerializeField] SpellInfoDetails spellInfoDetails;
        [SerializeField] RangedInfoDetails rangedInfoDetails;
        [SerializeField] ArrowInfoDetails arrowInfoDetails;
        [SerializeField] ArmorInfoDetails armorInfoDetails;
        [SerializeField] CharmInfoDetails charmInfoDetails;
        [SerializeField] PowerupInfoDetails powerupInfoDetails;
        [SerializeField] StatsEffectInfoDetails statsEffectInfoDetails;
        [SerializeField] ThrowableInfoDetails throwableInfoDetails;
        [SerializeField] RingInfoDetails ringInfoDetails;
        #endregion

        #region Empty Info.
        [Header("Empty Info (Drops).")]
        /// Weapons.
        [SerializeField] EmptyWeaponInfoDetails emptyWeaponInfoDetails;
        [SerializeField] EmptyArrowInfoDetails emptyArrowInfoDetails;
        /// Armors.
        [SerializeField] EmptyHeadInfoDetails emptyHeadInfoDetails;
        [SerializeField] EmptyChestInfoDetails emptyChestInfoDetails;
        [SerializeField] EmptyHandInfoDetails emptyHandInfoDetails;
        [SerializeField] EmptyLegInfoDetails emptyLegInfoDetails;
        /// Accessiors.
        [SerializeField] EmptyCharmInfoDetails emptyCharmInfoDetails;
        [SerializeField] EmptyPowerupInfoDetails emptyPowerupInfoDetails;
        [SerializeField] EmptyConsumableInfoDetails emptyConsumableInfoDetails;
        [SerializeField] EmptyRingInfoDetails emptyRingInfoDetails;
        #endregion

        #region Loaded Alter.
        [Header("Loaded Pre Alters (Drops).")]
        [SerializeField] MeleeAlterDetails pre_meleeAlterDetails;
        [SerializeField] CatalystsAlterDetails pre_catalystsAlterDetails;
        [SerializeField] SpellAlterDetails pre_spellAlterDetails;
        [SerializeField] RangedAlterDetails pre_rangedAlterDetails;
        [SerializeField] ArrowAlterDetails pre_arrowAlterDetails;
        [SerializeField] ArmorAlterDetails pre_armorAlterDetails;
        [SerializeField] CharmAlterDetails pre_charmAlterDetails;
        [SerializeField] PowerupAlterDetails pre_powerupAlterDetails;
        [SerializeField] StatsEffectAlterDetails pre_statsEffectAlterDetails;
        [SerializeField] ThrowableAlterDetails pre_throwableAlterDetails;
        [SerializeField] RingAlterDetails pre_ringAlterDetails;

        [Header("Loaded Post Alters (Drops).")]
        [SerializeField] MeleeAlterDetails post_meleeAlterDetails;
        [SerializeField] CatalystsAlterDetails post_catalystsAlterDetails;
        [SerializeField] SpellAlterDetails post_spellAlterDetails;
        [SerializeField] RangedAlterDetails post_rangedAlterDetails;
        [SerializeField] ArrowAlterDetails post_arrowAlterDetails;
        [SerializeField] ArmorAlterDetails post_armorAlterDetails;
        [SerializeField] CharmAlterDetails post_charmAlterDetails;
        [SerializeField] PowerupAlterDetails post_powerupAlterDetails;
        [SerializeField] StatsEffectAlterDetails post_statsEffectAlterDetails;
        [SerializeField] ThrowableAlterDetails post_throwableAlterDetails;
        [SerializeField] RingAlterDetails post_ringAlterDetails;
        #endregion

        #region Empty Alter.
        [Header("Empty Alter (Drops).")]
        /// Arrows.
        [SerializeField] EmptyArrowAlterDetails emptyArrowAlterDetails;
        /// Accessiors.
        [SerializeField] EmptyCharmAlterDetails emptyCharmAlterDetails;
        [SerializeField] EmptyPowerupAlterDetails emptyPowerupAlterDetails;
        [SerializeField] EmptyConsumableAlterDetails emptyConsumableAlterDetails;
        [SerializeField] EmptyRingAlterDetails emptyRingAlterDetails;
        #endregion

        #region Hints.
        [Header("Hints.")]
        [SerializeField] Canvas loadedEquipDetailHint;
        [SerializeField] Canvas emptyEquipDetailHint;
        #endregion

        #region Refs.
        [Header("Refs.")]
        [ReadOnlyInspector, SerializeField] ItemInfoDetails _cur_InfoDetails;
        [ReadOnlyInspector, SerializeField] ItemAlterDetails _cur_Pre_AlterDetails;
        [ReadOnlyInspector, SerializeField] ItemAlterDetails _cur_Post_AlterDetails;
        [ReadOnlyInspector] public ItemEquipSlotsDetail _cur_EquipSlotsDetail;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public bool isShowAlterHub;
        [ReadOnlyInspector] public bool isShowReviewSlotEmptyInfo;
        #endregion

        #region Private.
        [NonSerialized] SavableInventory _inventory;
        public StringBuilder _strBuilder;
        int _cur_itemHubTweenId;
        #endregion

        public void SwitchIsShowAlterHubStatus()
        {
            isShowAlterHub = !isShowAlterHub;
            if (isShowAlterHub)
            {
                OnAlterHub();
                HideCurrentInfoDetails();
                Shown_Cur_AlterDetails();

                _cur_EquipSlotsDetail.RedrawAlterDetail();
            }
            else
            {
                OnInfoHub();
                HideAlterDetails();

                _cur_EquipSlotsDetail.RedrawInfoDetail();
            }
        }

        public void OnInfoHub()
        {
            ShowInfoHub();
        }

        public void OnAlterHub()
        {
            ShowAlterHub();
        }

        #region On Equip Detail Quit.
        public void OnLoadedEquipDetailQuit()
        {
            if (isShowAlterHub)
            {
                ResetIsShowItemAlterStatus();
                OnInfoHub();
                HideAlterDetails();
            }
            else
            {
                HideCurrentInfoDetails();
            }

            void ResetIsShowItemAlterStatus()
            {
                isShowAlterHub = false;
            }
        }

        public void OnEmptyEquipDetailQuit()
        {
            HideCurrentInfoDetails();
        }
        #endregion

        #region Hide Info / Alter Details.
        public void HideCurrentInfoDetails()
        {
            _cur_InfoDetails.HideInfoDetails();
        }

        void HideAlterDetails()
        {
            _cur_Pre_AlterDetails.HideAlterDetails();
            _cur_Post_AlterDetails.HideAlterDetails();
        }
        #endregion
        
        #region Show / Hide ItemHubs.
        void ShowInfoHub()
        {
            HideAlterHub(true);
        }

        void ShowAlterHub()
        {
            HideInfoHub(true);
        }

        void HideInfoHub(bool _tweenOnAlterHubOnComplete)
        {
            CancelUnFinishTweeningJob();

            _cur_itemHubTweenId = LeanTween.alpha(infoHubGroup.gameObject, 0, _sideHubFadeOutSpeed).setEase(_sideHubEaseType).setOnComplete(OnCompleteTweenOnAlterHub).id;
            
            void OnCompleteTweenOnAlterHub()
            {
                infoHubCanvas.enabled = false;
                TweenOnAlterHub();
            }
        }

        void HideAlterHub(bool _tweenOnInfoHubOnComplete)
        {
            CancelUnFinishTweeningJob();

            _cur_itemHubTweenId = LeanTween.alphaCanvas(alterHubGroup, 0, _sideHubFadeOutSpeed).setEase(_sideHubEaseType).setOnComplete(OnCompleteTweenOnInfoHub).id;
            
            void OnCompleteTweenOnInfoHub()
            {
                alterHubCanvas.enabled = false;
                TweenOnInfoHub();
            }
        }

        void TweenOnInfoHub()
        {
            infoHubCanvas.enabled = true;
            _cur_itemHubTweenId = LeanTween.alphaCanvas(infoHubGroup, 1, _sideHubFadeInSpeed).setEase(_sideHubEaseType).id;
        }

        void TweenOnAlterHub()
        {
            alterHubCanvas.enabled = true;
            _cur_itemHubTweenId = LeanTween.alphaCanvas(alterHubGroup, 1, _sideHubFadeInSpeed).setEase(_sideHubEaseType).id;      
        }

        void CancelUnFinishTweeningJob()
        {
            if (LeanTween.isTweening(_cur_itemHubTweenId))
                LeanTween.cancel(_cur_itemHubTweenId);
        }
        #endregion

        #region Show Info Details
        public void ShowWeaponInfoDetails(RuntimeWeapon _runtimeWeapon)
        {
            meleeInfoDetails.ShowWeaponInfoDetails(_runtimeWeapon);
            _cur_InfoDetails = meleeInfoDetails;
        }

        public void ShowArrowInfoDetails(RuntimeArrow _runtimeArrow)
        {
            arrowInfoDetails.ShowArrowInfoDetails(_runtimeArrow);
            _cur_InfoDetails = arrowInfoDetails;
        }

        public void ShowArmorInfoDetails(RuntimeArmor _runtimeArmor)
        {
            armorInfoDetails.ShowArmorInfoDetails(_runtimeArmor);
            _cur_InfoDetails = armorInfoDetails;
        }

        public void ShowCharmInfoDetails(RuntimeCharm _runtimeCharm)
        {
            charmInfoDetails.ShowCharmInfoDetails(_runtimeCharm);
            _cur_InfoDetails = charmInfoDetails;
        }

        public void ShowPowerupInfoDetails(RuntimePowerup _runtimePowerup)
        {
            powerupInfoDetails.ShowPowerupInfoDetails(_runtimePowerup);
            _cur_InfoDetails = powerupInfoDetails;
        }

        public void ShowConsumableInfoDetails(RuntimeConsumable _runtimeConsumable)
        {
            if (_runtimeConsumable.isStatsEffectConsumable)
            {
                statsEffectInfoDetails.ShowInfoDetails();
                _runtimeConsumable.UpdateStatsEffectInfoDetails(statsEffectInfoDetails);
                _cur_InfoDetails = statsEffectInfoDetails;
            }
            else
            {
                throwableInfoDetails.ShowThrowableInfoDetails(_runtimeConsumable.GetThrowableConsumable());
                _cur_InfoDetails = throwableInfoDetails;
            }
        }

        public void ShowRingInfoDetails(RuntimeRing _runtimeRing)
        {
            ringInfoDetails.ShowRingInfoDetails(_runtimeRing);
            _cur_InfoDetails = ringInfoDetails;
        }

        #region DEFAULT INFO.
        public void ShowFistInfoDetails()
        {
            meleeInfoDetails.ShowWeaponInfoDetails(_inventory.runtimeUnarmed);
            _cur_InfoDetails = meleeInfoDetails;
        }

        public void ShowDeprivedHeadInfoDetails()
        {
            armorInfoDetails.ShowArmorInfoDetails(_inventory.runtimeDeprivedHead);
            _cur_InfoDetails = armorInfoDetails;
        }

        public void ShowDeprivedChestInfoDetails()
        {
            armorInfoDetails.ShowArmorInfoDetails(_inventory.runtimeDeprivedChest);
            _cur_InfoDetails = armorInfoDetails;
        }

        public void ShowDeprivedHandInfoDetails()
        {
            armorInfoDetails.ShowArmorInfoDetails(_inventory.runtimeDeprivedHand);
            _cur_InfoDetails = armorInfoDetails;
        }

        public void ShowDeprivedLegInfoDetails()
        {
            armorInfoDetails.ShowArmorInfoDetails(_inventory.runtimeDeprivedLeg);
            _cur_InfoDetails = armorInfoDetails;
        }
        #endregion

        #region EMPTY INFO.
        public void ShowEmptyArrowInfoDetails(bool _isReviewSlotEmpty)
        {
            isShowReviewSlotEmptyInfo = _isReviewSlotEmpty;
            _cur_InfoDetails = emptyArrowInfoDetails;

            emptyArrowInfoDetails.ShowEmptyArrowInfo();
        }

        public void ShowEmptyWeaponInfoDetails()
        {
            _cur_InfoDetails = emptyWeaponInfoDetails;
            emptyWeaponInfoDetails.ShowEmptyWeaponInfo();
        }

        public void ShowEmptyHeadInfoDetails()
        {
            _cur_InfoDetails = emptyHeadInfoDetails;
            emptyHeadInfoDetails.ShowEmptyHeadInfo();
        }

        public void ShowEmptyChestInfoDetails()
        {
            _cur_InfoDetails = emptyChestInfoDetails;
            emptyChestInfoDetails.ShowEmptyChestInfo();
        }

        public void ShowEmptyHandInfoDetails()
        {
            _cur_InfoDetails = emptyHandInfoDetails;
            emptyHandInfoDetails.ShowEmptyHandInfo();
        }

        public void ShowEmptyLegInfoDetails()
        {
            _cur_InfoDetails = emptyLegInfoDetails;
            emptyLegInfoDetails.ShowEmptyLegInfo();
        }

        public void ShowEmptyCharmInfoDetails(bool _isReviewSlotEmpty)
        {
            isShowReviewSlotEmptyInfo = _isReviewSlotEmpty;
            _cur_InfoDetails = emptyCharmInfoDetails;

            emptyCharmInfoDetails.ShowEmptyCharmInfo();
        }

        public void ShowEmptyPowerupInfoDetails(bool _isReviewSlotEmpty)
        {
            isShowReviewSlotEmptyInfo = _isReviewSlotEmpty;
            _cur_InfoDetails = emptyPowerupInfoDetails;

            emptyPowerupInfoDetails.ShowEmptyPowerupInfo();
        }

        public void ShowEmptyConsumableInfoDetails(bool _isReviewSlotEmpty)
        {
            _cur_InfoDetails = emptyConsumableInfoDetails;
            isShowReviewSlotEmptyInfo = _isReviewSlotEmpty;

            emptyConsumableInfoDetails.ShowEmptyConsumableInfo();
        }

        public void ShowEmptyRingInfoDetails(bool _isReviewSlotEmpty)
        {
            _cur_InfoDetails = emptyRingInfoDetails;
            isShowReviewSlotEmptyInfo = _isReviewSlotEmpty;

            emptyRingInfoDetails.ShowEmptyRingInfo();
        }
        #endregion

        #endregion

        #region Redraw / Set Alter Details. 

        #region Pre Alters.
        public void Redraw_Pre_Weapon_AlterDetails(RuntimeWeapon _runtimeWeapon)
        {
            pre_meleeAlterDetails.RedrawWeaponAlterDetails(_runtimeWeapon);
            _cur_Pre_AlterDetails = pre_meleeAlterDetails;
        }

        public void Redraw_Pre_Arrow_AlterDetails(RuntimeArrow _runtimeArrow)
        {
            pre_arrowAlterDetails.RedrawArrowAlterDetails(_runtimeArrow);
            _cur_Pre_AlterDetails = pre_arrowAlterDetails;
        }

        public void Redraw_Pre_Armor_AlterDetails(RuntimeArmor _runtimeArmor)
        {
            pre_armorAlterDetails.RedrawArmorAlterDetails(_runtimeArmor);
            _cur_Pre_AlterDetails = pre_armorAlterDetails;
        }

        public void Redraw_Pre_Charm_AlterDetails(RuntimeCharm _runtimeCharm)
        {
            pre_charmAlterDetails.RedrawCharmAlterDetails(_runtimeCharm);
            _cur_Pre_AlterDetails = pre_charmAlterDetails;
        }

        public void Redraw_Pre_Powerup_AlterDetails(RuntimePowerup _runtimePowerup)
        {
            pre_powerupAlterDetails.RedrawPowerupAlterDetails(_runtimePowerup);
            _cur_Pre_AlterDetails = pre_powerupAlterDetails;
        }

        public void Redraw_Pre_Consumable_AlterDetails(RuntimeConsumable _runtimeConsumable)
        {
            if (_runtimeConsumable.isStatsEffectConsumable)
            {
                _runtimeConsumable.UpdateStatsEffectAlterDetails(pre_statsEffectAlterDetails);
                _cur_Pre_AlterDetails = pre_statsEffectAlterDetails;
            }
            else
            {
                pre_throwableAlterDetails.RedrawThrowableAlterDetails(_runtimeConsumable.GetThrowableConsumable());
                _cur_Pre_AlterDetails = pre_throwableAlterDetails;
            }
        }

        public void Redraw_Pre_Ring_AlterDetails(RuntimeRing _runtimeRing)
        {
            pre_ringAlterDetails.RedrawRingAlterDetails(_runtimeRing);
            _cur_Pre_AlterDetails = pre_ringAlterDetails;
        }

        #region Defaults.
        public void Redraw_Pre_Fist_AlterDetails()
        {
            pre_meleeAlterDetails.RedrawWeaponAlterDetails(_inventory.runtimeUnarmed);
            _cur_Pre_AlterDetails = pre_meleeAlterDetails;
        }

        public void Redraw_Pre_DeprivedHead_AlterDetails()
        {
            pre_armorAlterDetails.RedrawArmorAlterDetails(_inventory.runtimeDeprivedHead);
            _cur_Pre_AlterDetails = pre_armorAlterDetails;
        }

        public void Redraw_Pre_DeprivedChest_AlterDetails()
        {
            pre_armorAlterDetails.RedrawArmorAlterDetails(_inventory.runtimeDeprivedChest);
            _cur_Pre_AlterDetails = pre_armorAlterDetails;
        }

        public void Redraw_Pre_DeprivedHand_AlterDetails()
        {
            pre_armorAlterDetails.RedrawArmorAlterDetails(_inventory.runtimeDeprivedHand);
            _cur_Pre_AlterDetails = pre_armorAlterDetails;
        }

        public void Redraw_Pre_DeprivedLeg_AlterDetails()
        {
            pre_armorAlterDetails.RedrawArmorAlterDetails(_inventory.runtimeDeprivedLeg);
            _cur_Pre_AlterDetails = pre_armorAlterDetails;
        }
        #endregion

        #region Emptys.
        public void Set_Empty_Arrow_CurPre_AlterDetails()
        {
            _cur_Pre_AlterDetails = emptyArrowAlterDetails;
        }

        public void Set_Empty_Charm_CurPre_AlterDetails()
        {
            _cur_Pre_AlterDetails = emptyCharmAlterDetails;
        }

        public void Set_Empty_Powerup_CurPre_AlterDetails()
        {
            _cur_Pre_AlterDetails = emptyPowerupAlterDetails;
        }

        public void Set_Empty_Consumable_CurPre_AlterDetails()
        {
            _cur_Pre_AlterDetails = emptyConsumableAlterDetails;
        }

        public void Set_Empty_Ring_CurPre_AlterDetails()
        {
            _cur_Pre_AlterDetails = emptyRingAlterDetails;
        }
        #endregion

        #endregion

        #region Post Alters.
        
        #region Set Post Alters.
        public void Set_Weapon_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_meleeAlterDetails;
        }

        public void Set_Arrow_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_arrowAlterDetails;
        }

        public void Set_Armor_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_armorAlterDetails;
        }

        public void Set_Charm_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_charmAlterDetails;
        }

        public void Set_Powerup_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_powerupAlterDetails;
        }

        public void Set_StatsEffect_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_statsEffectAlterDetails;
        }

        public void Set_Throwable_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_throwableAlterDetails;
        }

        public void Set_Ring_CurPost_AlterDetails()
        {
            _cur_Post_AlterDetails = post_ringAlterDetails;
        }
        #endregion

        #region Redraws Post Alters.
        public void Redraw_Post_Weapon_AlterDetails(RuntimeWeapon _runtimeWeapon)
        {
            post_meleeAlterDetails.RedrawWeaponAlterDetails(_runtimeWeapon);
        }

        public void Redraw_Post_Arrow_AlterDetails(RuntimeArrow _runtimeArrow)
        {
            post_arrowAlterDetails.RedrawArrowAlterDetails(_runtimeArrow);
        }

        public void Redraw_Post_Armor_AlterDetails(RuntimeArmor _runtimeArmor)
        {
            post_armorAlterDetails.RedrawArmorAlterDetails(_runtimeArmor);
        }

        public void Redraw_Post_Charm_AlterDetails(RuntimeCharm _runtimeCharm)
        {
            post_charmAlterDetails.RedrawCharmAlterDetails(_runtimeCharm);
        }

        public void Redraw_Post_Powerup_AlterDetails(RuntimePowerup _runtimePowerup)
        {
            post_powerupAlterDetails.RedrawPowerupAlterDetails(_runtimePowerup);
        }

        public void Redraw_Post_Consumable_AlterDetails(RuntimeConsumable _runtimeConsumable)
        {
            if (_runtimeConsumable.isStatsEffectConsumable)
            {
                _runtimeConsumable.UpdateStatsEffectAlterDetails(post_statsEffectAlterDetails);
            }
            else
            {
                post_throwableAlterDetails.RedrawThrowableAlterDetails(_runtimeConsumable.GetThrowableConsumable());
            }
        }

        public void Redraw_Post_Ring_AlterDetails(RuntimeRing _runtimeRing)
        {
            post_ringAlterDetails.RedrawRingAlterDetails(_runtimeRing);
        }
        #endregion

        #endregion

        #endregion

        #region Show Alter Details.
        void Shown_Cur_AlterDetails()
        {
            _cur_Pre_AlterDetails.ShowAlterDetails();
            _cur_Post_AlterDetails.ShowAlterDetails();
        }
        #endregion

        #region Show Empty / Loaded Hint.
        public void ShowEmptyHint()
        {
            loadedEquipDetailHint.enabled = false;
            emptyEquipDetailHint.enabled = true;
        }

        public void ShowLoadedHint()
        {
            loadedEquipDetailHint.enabled = true;
            emptyEquipDetailHint.enabled = false;
        }
        #endregion

        #region Reset Hub OnMenuOpen / OnMenuClose.
        public void ResetHubOnMenuOpen()
        {

        }

        public void ResetHubOnMenuClose()
        {
            HideCurrentInfoDetails();
        }
        #endregion

        #region Setup.
        public void Setup(EquipmentMenuManager _equipmentMenuManager)
        {
            _inventory = _equipmentMenuManager._inp._states._savableInventory;
            
            SetupInfoDetails();

            SetupAlterDetails();

            SetupGetStrBuilder();

            void SetupGetStrBuilder()
            {
                _strBuilder = _equipmentMenuManager._inp.itemHub_strBuilder;
            }
        }

        void SetupInfoDetails()
        {
            SetupAssignInfoDetailsItemHubRef();

            meleeInfoDetails.Setup();

            /* Uncomment these when catalysts, spell, bow is used as weapon 
            catalystsInfoDetails.Setup();
            spellInfoDetails.Setup();
            rangedInfoDetails.Setup();
            */

            void SetupAssignInfoDetailsItemHubRef()
            {
                meleeInfoDetails._itemHub = this;
                armorInfoDetails._itemHub = this;

                emptyArrowInfoDetails._itemHub = this;
                emptyCharmInfoDetails._itemHub = this;
                emptyPowerupInfoDetails._itemHub = this;
                emptyConsumableInfoDetails._itemHub = this;
                emptyRingInfoDetails._itemHub = this;
                
                /* Uncomment these when catalysts, throwable consumable, bow is used as weapon 
                catalystsInfoDetails._itemHub = this;
                rangedInfoDetails._itemHub = this;
                throwableInfoDetails._itemHub = this;
                arrowInfoDetails._itemHub = this;
                */
            }
        }

        void SetupAlterDetails()
        {
            SetupAssignAlterDetailsItemHubRef();

            pre_meleeAlterDetails.Setup();
            post_meleeAlterDetails.Setup();

            void SetupAssignAlterDetailsItemHubRef()
            {
                pre_meleeAlterDetails._itemHub = this;
                pre_armorAlterDetails._itemHub = this;

                post_meleeAlterDetails._itemHub = this;
                post_armorAlterDetails._itemHub = this;
            }
        }
        #endregion
    }
}