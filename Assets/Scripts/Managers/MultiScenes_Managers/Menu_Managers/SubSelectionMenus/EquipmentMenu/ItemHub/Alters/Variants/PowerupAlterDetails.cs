using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class PowerupAlterDetails : ItemAlterDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Bottom Desc Texts.")]
        [SerializeField] TMP_Text powerupEffect_Text;

        RuntimePowerup runtimePowerup;
        PowerupItem referedPowerup;

        public void RedrawPowerupAlterDetails(RuntimePowerup _runtimePowerup)
        {
            runtimePowerup = _runtimePowerup;
            referedPowerup = _runtimePowerup._referedPowerupItem;
            
            UpdatePowerupAlterDetails();
        }

        void UpdatePowerupAlterDetails()
        {
            /// General Info
            itemTitle_Text.text = runtimePowerup.runtimeName;
            itemIcon_Image.sprite = referedPowerup.itemIcon;

            /// Bottom Desc Text
            powerupEffect_Text.text = referedPowerup.powerupEffectText;
        }
    }
}