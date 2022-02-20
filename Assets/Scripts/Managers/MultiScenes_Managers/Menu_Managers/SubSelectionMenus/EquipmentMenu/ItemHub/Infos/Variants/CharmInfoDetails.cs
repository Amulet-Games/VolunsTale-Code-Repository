﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class CharmInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Bottom Desc Texts.")]
        [SerializeField] TMP_Text charmEffect_Text;

        RuntimeCharm runtimeCharm;
        CharmItem referedCharm;
        
        public void ShowCharmInfoDetails(RuntimeCharm _runtimeCharm)
        {
            runtimeCharm = _runtimeCharm;
            referedCharm = _runtimeCharm._referedCharmItem;

            ShowInfoDetails();
            UpdateCharmDetails();
        }

        void UpdateCharmDetails()
        {
            /// General Info
            itemTitle_Text.text = runtimeCharm.runtimeName;
            itemIcon_Image.sprite = referedCharm.itemIcon;

            /// Bottom Desc Text
            charmEffect_Text.text = referedCharm.charmEffectText;
        }
    }
}
