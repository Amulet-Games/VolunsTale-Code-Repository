using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Starter Class Profile")]
    public class StarterClassProfile : ScriptableObject
    {
        [Header("Starter Class Name.")]
        public string starterClassName;
 
        [Header("Class Weapons.")]
        public string[] rightHandWeaponIds;
        public string[] leftHandWeaponIds;
        
        [Header("Class Armors.")]
        public string headArmorId;
        public string chestArmorId;
        public string handArmorId;
        public string legArmorId;

        [Header("Class Charm.")]
        public string charmId;

        [Header("Class Powerup.")]
        public string powerupId;

        [Header("Class Rings.")]
        public string rightRingIds;
        public string leftRingIds;

        [Header("Class Consumables.")]
        public StarterConsumableInfo[] starterConsumableInfos;

        [Header("Class Spells.")]
        public string[] spellIds;

        [Header("Class Attributes.")]
        public StarterAttributesStats starterAttributeStats;
    }

    [Serializable]
    public class StarterConsumableInfo
    {
        public ConsumableItem.ConsumableTypeEnum starterConsumableType;
        public string starterConsumableId;
        public int carryingAmount;
    }

    [Serializable]
    public class StarterAttributesStats
    {
        public int starter_vigor = 1;
        public int starter_adaptation = 1;
        public int starter_endurance = 1;
        public int starter_vitality = 1;
        public int starter_strength = 1;
        public int starter_hexes = 1;
        public int starter_intelligence = 1;
        public int starter_divinity = 1;
        public int starter_fortune = 1;
    }
}
