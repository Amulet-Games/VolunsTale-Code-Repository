using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class RingAlterDetails : ItemAlterDetails
    {
        [Header("Generals.")]
        public TMP_Text itemTitle_Text;
        public Image itemIcon_Image;

        //[SerializeField] Text weight_Text;

        [Header("Bottom Desc Texts.")]
        [SerializeField] TMP_Text ringEffect_Text;

        RuntimeRing runtimeRing;
        RingItem referedRingItem;

        public void RedrawRingAlterDetails(RuntimeRing _runtimeRing)
        {
            runtimeRing = _runtimeRing;
            referedRingItem = runtimeRing._referedRingItem;
            
            UpdateRingAlterDetails();
        }

        void UpdateRingAlterDetails()
        {
            /// General Info
            itemTitle_Text.text = runtimeRing.runtimeName;
            itemIcon_Image.sprite = referedRingItem.itemIcon;

            /// Bottom Desc Text
            //weight_Text.text = referedRingItem.weight.ToString();
            ringEffect_Text.text = referedRingItem.ringEffectText;
        }
    }
}