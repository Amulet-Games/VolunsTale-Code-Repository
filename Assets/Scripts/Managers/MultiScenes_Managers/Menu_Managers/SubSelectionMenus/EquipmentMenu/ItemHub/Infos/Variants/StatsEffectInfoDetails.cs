using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class StatsEffectInfoDetails : ItemInfoDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Top Info Texts.")]
        public TMP_Text curCarryingAmount_Text;
        public TMP_Text maxCarryingAmount_Text;
        public TMP_Text curStoredAmount_Text;
        public TMP_Text maxStoredAmount_Text;

        [Header("Bottom Desc Texts.")]
        public TMP_Text consumableEffect_Text;
        
        [Header("Font Asset (Drops).")]
        public TMP_FontAsset arbutusSlab_normal_asset;
        public TMP_FontAsset century_normal_asset;
    }
}