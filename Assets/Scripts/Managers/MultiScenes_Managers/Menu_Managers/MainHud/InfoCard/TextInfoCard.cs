using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class TextInfoCard : InfoCard
    {
        public string info;
        public Text infoText;

        public void OnTextInfoCard()
        {
            Default_ShowInfoCard();
            infoText.text = info;
        }

        public override void OffInfoCard()
        {
            Default_HideInfoCard();
        }
    }
}