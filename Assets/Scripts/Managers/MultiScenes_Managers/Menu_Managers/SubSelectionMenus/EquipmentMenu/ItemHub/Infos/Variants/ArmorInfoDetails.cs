using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class ArmorInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Top Info Texts.")]
        [SerializeField] TMP_Text dmg_reduct_Text;
        [SerializeField] TMP_Text dmg_reduct_val_Text;
        [SerializeField] TMP_Text vs_reduct_Text;
        [SerializeField] TMP_Text vs_reduct_val_Text;
        [SerializeField] TMP_Text status_resis_Text;
        [SerializeField] TMP_Text status_resis_val_Text;
        [SerializeField] TMP_Text poise_val_Text;

        #region Top Image Fields.
        [Header("Top Info Images.")]
        [SerializeField] Image dmg_reduct_Img;
        [SerializeField] Image vs_reduct_Img;
        [SerializeField] Image status_resis_Img;
        #endregion

        [Header("Buttom Desc Texts.")]
        [SerializeField] TMP_Text armor_desc_Text;

        [Header("Refs.")]
        [ReadOnlyInspector] public ItemHub _itemHub;

        RuntimeArmor runtimeArmor;
        ArmorItem _referedArmorItem;

        public void ShowArmorInfoDetails(RuntimeArmor _runtimeArmor)
        {
            runtimeArmor = _runtimeArmor;
            _referedArmorItem = runtimeArmor.GetReferedArmorItem();

            ShowInfoDetails();
            UpdateArmorInfoDetails();
            UpdateArmorInfoDetails_DmgReductTypeRelated();
            UpdateArmorInfoDetails_VsReductTypeRelated();
        }

        protected void UpdateArmorInfoDetails()
        {
            /// General Info
            itemTitle_Text.text = runtimeArmor.runtimeName;
            itemIcon_Image.sprite = _referedArmorItem.itemIcon;

            /// Top Info Text
            dmg_reduct_val_Text.text = _referedArmorItem.dmgReduct.ToString();
            vs_reduct_val_Text.text = _referedArmorItem.vsReduct.ToString();
            status_resis_val_Text.text = "-";
            poise_val_Text.text = "-";

            /// Bottom Desc Text
            armor_desc_Text.text = _referedArmorItem.itemDescription_1;
        }

        void UpdateArmorInfoDetails_DmgReductTypeRelated()
        {
            switch (_referedArmorItem.damageReductionType)
            {
                case P_Armor_DamageReductionTypeEnum.Physical:
                    dmg_reduct_Img.sprite = _itemHub.physical_dmg_reduct_sprite;
                    dmg_reduct_Text.text = "Physical";
                    break;
                case P_Armor_DamageReductionTypeEnum.Magic:
                    dmg_reduct_Img.sprite = _itemHub.magic_dmg_reduct_sprite;
                    dmg_reduct_Text.text = "Magic";
                    break;
                case P_Armor_DamageReductionTypeEnum.Fire:
                    dmg_reduct_Img.sprite = _itemHub.fire_dmg_reduct_sprite;
                    dmg_reduct_Text.text = "Fire";
                    break;
                case P_Armor_DamageReductionTypeEnum.Lightning:
                    dmg_reduct_Img.sprite = _itemHub.lightning_dmg_reduct_sprite;
                    dmg_reduct_Text.text = "Lightning";
                    break;
                case P_Armor_DamageReductionTypeEnum.Dark:
                    dmg_reduct_Img.sprite = _itemHub.dark_dmg_reduct_sprite;
                    dmg_reduct_Text.text = "Dark";
                    break;
            }
        }

        void UpdateArmorInfoDetails_VsReductTypeRelated()
        {
            switch (_referedArmorItem.vsReductionType)
            {
                case P_Armor_VsReductionTypeEnum.strike:
                    vs_reduct_Img.sprite = _itemHub.strike_vs_reduct_sprite;
                    vs_reduct_Text.text = "strike";
                    break;
                case P_Armor_VsReductionTypeEnum.slash:
                    vs_reduct_Img.sprite = _itemHub.slash_vs_reduct_sprite;
                    vs_reduct_Text.text = "slash";
                    break;
                case P_Armor_VsReductionTypeEnum.thrust:
                    vs_reduct_Img.sprite = _itemHub.thrust_vs_reduct_sprite;
                    vs_reduct_Text.text = "thrust";
                    break;
            }
        }

        void UpdateArmorInfoDetails_StatusResisTypeRelated()
        {
            
        }
    }
}
