using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class ThrowableInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Top Info Texts.")]
        [SerializeField] TMP_Text main_scaling_Text;
        [SerializeField] TMP_Text curCarryingAmount_Text;
        [SerializeField] TMP_Text maxCarryingAmount_Text;
        [SerializeField] TMP_Text curStoredAmount_Text;
        [SerializeField] TMP_Text maxStoredAmount_Text;

        [Header("Top Info Images.")]
        [SerializeField] Image scaling_attri_Img;

        [Header("Bottom Desc Texts.")]
        [SerializeField] TMP_Text consumableEffect_Text;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public ItemHub _itemHub;

        ThrowableConsumable _runtimeThrowableConsumable;
        ThrowableConsumableItem _referedThrowableConsumable;
        
        public void ShowThrowableInfoDetails(ThrowableConsumable _throwableConsumable)
        {
            _runtimeThrowableConsumable = _throwableConsumable;
            _referedThrowableConsumable = _throwableConsumable._referedThrowableItem;

            ShowInfoDetails();
            UpdateThrowableInfoDetails();
            UpdateThrowableInfoDetails_ElementRelated();
        }

        void UpdateThrowableInfoDetails()
        {
            /// General Info
            itemTitle_Text.text = _runtimeThrowableConsumable.runtimeName;
            itemIcon_Image.sprite = _referedThrowableConsumable.itemIcon;

            /// Top Info Text
            main_scaling_Text.text = _referedThrowableConsumable.GetThrowableAtkAttriScalingText();

            maxCarryingAmount_Text.text = _referedThrowableConsumable.maxCarryingAmount.ToString();
            curCarryingAmount_Text.text = _runtimeThrowableConsumable.curCarryingAmount.ToString();

            maxStoredAmount_Text.text = _referedThrowableConsumable.maxStoredAmount.ToString();
            curStoredAmount_Text.text = _runtimeThrowableConsumable.curStoredAmount.ToString();

            /// Bottom Desc Text
            consumableEffect_Text.text = _referedThrowableConsumable.consumableEffectText.ToString();
        }

        /// Element Related
        protected void UpdateThrowableInfoDetails_ElementRelated()
        {
            switch (_referedThrowableConsumable.throwableAttackPowerType)
            {
                case P_Weapon_ElementTypeEnum.Physical:
                    scaling_attri_Img.sprite = _itemHub.str_sprite;
                    break;

                case P_Weapon_ElementTypeEnum.Magic:
                    scaling_attri_Img.sprite = _itemHub.int_sprite;
                    break;
               
                case P_Weapon_ElementTypeEnum.Fire:
                    scaling_attri_Img.sprite = _itemHub.hex_sprite;
                    break;

                case P_Weapon_ElementTypeEnum.Lightning:
                    scaling_attri_Img.sprite = _itemHub.div_sprite;
                    break;

                case P_Weapon_ElementTypeEnum.Dark:
                    scaling_attri_Img.sprite = _itemHub.hex_sprite;
                    break;
            }
        }
    }
}