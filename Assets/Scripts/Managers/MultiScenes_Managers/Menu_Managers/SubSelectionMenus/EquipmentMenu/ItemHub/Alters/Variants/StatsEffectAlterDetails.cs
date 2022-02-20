using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class StatsEffectAlterDetails : ItemAlterDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        [Header("Top Desc Texts.")]
        public TMP_Text consumableEffect_Text;

        [Header("Bottom Alter Texts.")]
        public TMP_Text curCarryingAmount_Text;
        public TMP_Text maxCarryingAmount_Text;
        
    }
}