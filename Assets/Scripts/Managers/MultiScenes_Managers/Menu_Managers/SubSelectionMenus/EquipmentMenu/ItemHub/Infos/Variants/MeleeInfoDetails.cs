using System.Text;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class MeleeInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        #region Top Text Fields.
        [Header("Top Info Texts.")]
        [SerializeField] TMP_Text elementType_Text;
        [SerializeField] TMP_Text main_damage_Text;
        [SerializeField] TMP_Text main_damage_bonus_Text;
        [SerializeField] TMP_Text defense_Text;
        [SerializeField] TMP_Text specialAbility_Text;
        [SerializeField] TMP_Text main_scaling_Text;
        [SerializeField] TMP_Text execDamage_Text;
        #endregion

        #region Top Image Fields.
        [Header("Top Info Images.")]
        [SerializeField] Image damage_Img;
        [SerializeField] Image defense_Img;
        [SerializeField] Image scaling_attri_Img;
        #endregion

        #region Top Canvas Fields.
        [Header("Adv Element Canvases.")]
        [SerializeField] Canvas physical_adv_canvas;
        [SerializeField] Canvas magical_adv_canvas;
        [SerializeField] Canvas fire_adv_canvas;
        [SerializeField] Canvas lightning_adv_canvas;
        [SerializeField] Canvas dark_adv_canvas;

        [Header("Dis Adv Element Canvases.")]
        [SerializeField] Canvas physical_disa_canvas;
        [SerializeField] Canvas magical_disa_canvas;
        [SerializeField] Canvas fire_disa_canvas;
        [SerializeField] Canvas lightning_disa_canvas;
        [SerializeField] Canvas dark_disa_canvas;
        #endregion

        [Header("Bottom Desc Texts.")]
        [SerializeField] TMP_Text weapon_desc_Text;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] protected Canvas _prevAdvCanvas;
        [ReadOnlyInspector, SerializeField] protected Canvas _prevDisaCanvas;

        [Header("Refs.")]
        [ReadOnlyInspector] public ItemHub _itemHub;

        protected WeaponModifiableStats weaponModifiableStats;
        protected WeaponItem referedWeaponItem;

        #region Setup.
        public void Setup()
        {
            SetupSetDefault_Adv_Disa_Canvases();
        }

        protected void SetupSetDefault_Adv_Disa_Canvases()
        {
            _prevAdvCanvas = physical_adv_canvas;
            _prevDisaCanvas = physical_disa_canvas;
        }
        #endregion

        public void ShowWeaponInfoDetails(RuntimeWeapon _runtimeWeapon)
        {
            itemTitle_Text.text = _runtimeWeapon.runtimeName;
            weaponModifiableStats = _runtimeWeapon.weaponModifiableStats;
            referedWeaponItem = _runtimeWeapon._referedWeaponItem;
            
            ShowInfoDetails();
            UpdateWeaponInfoDetails();
            UpdateWeaponInfoDetails_ElementRelated();
        }
        
        protected virtual void UpdateWeaponInfoDetails()
        {
            /// General Info
            itemIcon_Image.sprite = referedWeaponItem.itemIcon;

            /// Top Info Text
            main_damage_Text.text = weaponModifiableStats._mainAtkPower.ToString();
            main_damage_bonus_Text.text = GetMainDamageBonusText();
            defense_Text.text = referedWeaponItem._guardAbsorpValue.ToString();
            specialAbility_Text.text = referedWeaponItem.GetSpecialAbilityText();
            main_scaling_Text.text = referedWeaponItem.GetMainAtkAttriScalingText();
            execDamage_Text.text = weaponModifiableStats._criticalAtkPower.ToString();

            /// Bottom Desc Text
            weapon_desc_Text.text = referedWeaponItem.itemDescription_1;
            
            /// StrBuilder Text.
            string GetMainDamageBonusText()
            {
                weaponModifiableStats.RefreshTrueBonusDamageValue();
                StringBuilder strBuilder = _itemHub._strBuilder;

                strBuilder.Clear();
                strBuilder.Append("(+").Append(Math.Round(weaponModifiableStats._mainAtkBonus, MidpointRounding.AwayFromZero)).Append(")");
                return strBuilder.ToString();
            }
        }

        /// Element Related
        protected void UpdateWeaponInfoDetails_ElementRelated()
        {
            HidePrevious_Adv_Disa_Canvases();

            switch (referedWeaponItem.weaponMainElementType)
            {
                case P_Weapon_ElementTypeEnum.Physical:

                    elementType_Text.text = "Physical";
                    damage_Img.sprite = _itemHub.physical_damage_sprite;
                    defense_Img.sprite = _itemHub.physical_defense_sprite;
                    scaling_attri_Img.sprite = _itemHub.str_sprite;

                    _prevAdvCanvas = magical_adv_canvas;
                    _prevAdvCanvas.enabled = true;

                    _prevDisaCanvas = physical_disa_canvas;
                    _prevDisaCanvas.enabled = true;
                    break;

                case P_Weapon_ElementTypeEnum.Magic:

                    elementType_Text.text = "Magic";
                    damage_Img.sprite = _itemHub.magic_damage_sprite;
                    defense_Img.sprite = _itemHub.magic_defense_sprite;
                    scaling_attri_Img.sprite = _itemHub.int_sprite;

                    _prevAdvCanvas = physical_adv_canvas;
                    _prevAdvCanvas.enabled = true;

                    _prevDisaCanvas = magical_disa_canvas;
                    _prevDisaCanvas.enabled = true;
                    break;

                case P_Weapon_ElementTypeEnum.Fire:

                    elementType_Text.text = "Fire";
                    damage_Img.sprite = _itemHub.fire_damage_sprite;
                    defense_Img.sprite = _itemHub.fire_defense_sprite;
                    scaling_attri_Img.sprite = _itemHub.hex_sprite;

                    _prevAdvCanvas = dark_adv_canvas;
                    _prevAdvCanvas.enabled = true;

                    _prevDisaCanvas = lightning_disa_canvas;
                    _prevDisaCanvas.enabled = true;
                    break;

                case P_Weapon_ElementTypeEnum.Lightning:

                    elementType_Text.text = "Lightning";
                    damage_Img.sprite = _itemHub.lightning_damage_sprite;
                    defense_Img.sprite = _itemHub.lightning_defense_sprite;
                    scaling_attri_Img.sprite = _itemHub.div_sprite;

                    _prevAdvCanvas = fire_adv_canvas;
                    _prevAdvCanvas.enabled = true;

                    _prevDisaCanvas = dark_disa_canvas;
                    _prevDisaCanvas.enabled = true;
                    break;

                case P_Weapon_ElementTypeEnum.Dark:

                    elementType_Text.text = "Dark";
                    damage_Img.sprite = _itemHub.dark_damage_sprite;
                    defense_Img.sprite = _itemHub.dark_defense_sprite;
                    scaling_attri_Img.sprite = _itemHub.hex_sprite;

                    _prevAdvCanvas = lightning_adv_canvas;
                    _prevAdvCanvas.enabled = true;

                    _prevDisaCanvas = fire_disa_canvas;
                    _prevDisaCanvas.enabled = true;
                    break;
            }
        }

        /// Preivous Adv Disa Canvases.
        void HidePrevious_Adv_Disa_Canvases()
        {
            _prevAdvCanvas.enabled = false;
            _prevDisaCanvas.enabled = false;
        }
    }
}
