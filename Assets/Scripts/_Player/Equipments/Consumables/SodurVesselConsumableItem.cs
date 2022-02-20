using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Consumables/Sodur Vessel Item")]
    public class SodurVesselConsumableItem : StatsEffectConsumableItem
    {
        [Header("Sodur Vessel Item Config.")]
        public Sprite _sodurVesselEmptySprite;
        public Sprite _sodurVesselEmptyNoBaseSprite;

        public override bool GetIsVessel()
        {
            return true;
        }

        public override bool GetIsVolun()
        {
            return false;
        }

        public override Sprite GetEmptyConsumableSprite()
        {
            return _sodurVesselEmptySprite;
        }

        public override Sprite GetEmptyNoBaseConsumableSprite()
        {
            return _sodurVesselEmptyNoBaseSprite;
        }
    }
}