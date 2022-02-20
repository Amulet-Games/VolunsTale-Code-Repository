using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
    public abstract class Item : ScriptableObject
    {
        [Header("Item Configs.")]
        public ItemTypeEnum itemType;
        public Sprite itemIcon;
        public string itemName;
        [TextArea(1, 20)]
        public string itemDescription_1;

        public abstract void CreateInstanceForPickups(SavableInventory _inventory);

        public abstract void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount);

        public virtual WeaponItem GetWeaponItem()
        {
            return null;
        }

        public virtual HeadArmorItem GetHeadArmorItem()
        {
            return null;
        }

        public virtual ChestArmorItem GetChestArmorItem()
        {
            return null;
        }

        public virtual HandArmorItem GetHandArmorItem()
        {
            return null;
        }

        public virtual LegArmorItem GetLegArmorItem()
        {
            return null;
        }

        public virtual RingItem GetRingItem()
        {
            return null;
        }

        public virtual CharmItem GetCharmItem()
        {
            return null;
        }

        public virtual PowerupItem GetPowerupItem()
        {
            return null;
        }

        public virtual StatsEffectConsumableItem GetStatsEffectConsumableItem()
        {
            return null;
        }

        public virtual ThrowableConsumableItem GetThrowableConsumableItem()
        {
            return null;
        }
        
        public enum ItemTypeEnum
        {
            MeleeWeapon,
            Shield,
            Catalysts,
            Spell,
            RangedWeapon,
            Arrow,
            Head,
            Chest,
            Hand,
            Leg,
            Charm,
            Powerup,
            Consumable,
            Ring,
        }
    }
}
