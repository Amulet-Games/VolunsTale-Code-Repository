using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class PowerupInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Bottom Desc Texts.")]
        [SerializeField] TMP_Text powerupEffect_Text;

        RuntimePowerup runtimePowerup;
        PowerupItem referedPowerup;
        
        public void ShowPowerupInfoDetails(RuntimePowerup _runtimePowerup)
        {
            runtimePowerup = _runtimePowerup;
            referedPowerup = _runtimePowerup._referedPowerupItem;

            ShowInfoDetails();
            UpdatePowerupInfoDetails();
        }

        void UpdatePowerupInfoDetails()
        {
            /// General Info
            itemTitle_Text.text = runtimePowerup.runtimeName;
            itemIcon_Image.sprite = referedPowerup.itemIcon;

            /// Bottom Desc Text
            powerupEffect_Text.text = referedPowerup.powerupEffectText;
        }
    }
}