using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class RuntimeArmor : RuntimeItem
    {
        [Header("Armor Modifiable Stats.")]
        [ReadOnlyInspector] public float _durability;

        [Header("Status")]
        [ReadOnlyInspector] public ArmorSlotSideTypeEnum currentSlotSideType;
        [ReadOnlyInspector] public ArmorTypeEnum armorType;
        [ReadOnlyInspector] public bool isDeprivedArmor;
        
        /// VIRTUAL
        public virtual RuntimeHeadArmor GetHeadArmor()
        {
            return null;
        }

        public virtual RuntimeChestArmor GetChestArmor()
        {
            return null;
        }

        public virtual RuntimeHandArmor GetHandArmor()
        {
            return null;
        }

        public virtual RuntimeLegArmor GetLegArmor()
        {
            return null;
        }

        public abstract ArmorItem GetReferedArmorItem();

        public enum ArmorTypeEnum
        {
            Head,
            Chest,
            Hand,
            Leg
        }

        public enum ArmorSlotSideTypeEnum
        {
            Head,
            Chest,
            Hand,
            Leg,
            Backpack
        }
    }
}