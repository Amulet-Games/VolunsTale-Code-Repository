using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Consumables/Volun Vessel Item")]
    public class VolunVesselConsumableItem : StatsEffectConsumableItem
    {
        [Header("Volun Vessel Item Config.")]
        public Sprite _volunVesselEmptySprite;
        public Sprite _volunVesselEmptyNoBaseSprite;

        public override bool GetIsVessel()
        {
            return true;
        }

        public override bool GetIsVolun()
        {
            return true;
        }

        public override Sprite GetEmptyConsumableSprite()
        {
            return _volunVesselEmptySprite;
        }

        public override Sprite GetEmptyNoBaseConsumableSprite()
        {
            return _volunVesselEmptyNoBaseSprite;
        }
    }
}