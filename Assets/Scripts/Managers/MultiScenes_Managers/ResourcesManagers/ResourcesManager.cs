using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA 
{
    [CreateAssetMenu(menuName = "Managers/ResourcesManager")]
    public class ResourcesManager : ScriptableObject
    {
        #region PLAYER.

        #region Defaults.
        [Header("Weapon Default.")]
        public WeaponItem unarmedWeaponItem;
        [Header("Armor Default.")]
        public HeadArmorItem deprivedHeadArmorItem;
        public ChestArmorItem deprivedChestArmorItem;
        public HandArmorItem deprivedHandArmorItem;
        public LegArmorItem deprivedLegArmorItem;
        [Header("Consumable Default.")]
        public StatsEffectConsumableItem volunVesselConsumableItem;
        public StatsEffectConsumableItem sodurVesselConsumableItem;
        [Header("Amulet Default.")]
        public VolunAmuletItem volunAmuletItem;
        #endregion

        #region Weapon List.
        [Header("Weapon List.")]
        public List<WeaponItem> weaponItems = new List<WeaponItem>();
        Dictionary<string, WeaponItem> weaponItemsDict = new Dictionary<string, WeaponItem>();
        #endregion

        #region Armor List.
        [Header("Armor List.")]
        public List<HeadArmorItem> headArmorItems = new List<HeadArmorItem>();
        Dictionary<string, HeadArmorItem> headArmorItemsDict = new Dictionary<string, HeadArmorItem>();
        public List<ChestArmorItem> chestArmorItems = new List<ChestArmorItem>();
        Dictionary<string, ChestArmorItem> chestArmorItemsDict = new Dictionary<string, ChestArmorItem>();
        public List<HandArmorItem> handArmorItems = new List<HandArmorItem>();
        Dictionary<string, HandArmorItem> handArmorItemsDict = new Dictionary<string, HandArmorItem>();
        public List<LegArmorItem> legArmorItems = new List<LegArmorItem>();
        Dictionary<string, LegArmorItem> legArmorItemsDict = new Dictionary<string, LegArmorItem>();
        #endregion

        #region Charm List.
        [Header("Charm List.")]
        public List<CharmItem> charmItems = new List<CharmItem>();
        Dictionary<string, CharmItem> charmItemsDict = new Dictionary<string, CharmItem>();
        #endregion

        #region Powerup List.
        [Header("Powerup List.")]
        public List<PowerupItem> powerupItems = new List<PowerupItem>();
        Dictionary<string, PowerupItem> powerupItemsDict = new Dictionary<string, PowerupItem>();
        #endregion

        #region Ring List.
        [Header("Ring List.")]
        public List<RingItem> ringItems = new List<RingItem>();
        Dictionary<string, RingItem> ringItemsDict = new Dictionary<string, RingItem>();
        #endregion

        #region Consumable List.
        [Header("Consumable List.")]
        public List<StatsEffectConsumableItem> statsEffectConsumableItems = new List<StatsEffectConsumableItem>();
        Dictionary<string, StatsEffectConsumableItem> statsEffectConsumableItemsDict = new Dictionary<string, StatsEffectConsumableItem>();

        public List<ThrowableConsumableItem> throwableConsumableItems = new List<ThrowableConsumableItem>();
        Dictionary<string, ThrowableConsumableItem> throwableConsumableItemsDict = new Dictionary<string, ThrowableConsumableItem>();
        #endregion

        public void InitPlayerResources()
        {
            #region Weapon.
            int weaponItemsCount = weaponItems.Count;
            for (int i = 0; i < weaponItemsCount; i++)
            {
                if (!weaponItemsDict.ContainsKey(weaponItems[i].name))
                {
                    weaponItemsDict.Add(weaponItems[i].name, weaponItems[i]);
                }
            }
            #endregion

            #region Armor.
            int headItemsCount = headArmorItems.Count;
            for (int i = 0; i < headItemsCount; i++)
            {
                if (!headArmorItemsDict.ContainsKey(headArmorItems[i].name))
                {
                    headArmorItemsDict.Add(headArmorItems[i].name, headArmorItems[i]);
                }
            }

            int chestItemsCount = chestArmorItems.Count;
            for (int i = 0; i < chestItemsCount; i++)
            {
                if (!chestArmorItemsDict.ContainsKey(chestArmorItems[i].name))
                {
                    chestArmorItemsDict.Add(chestArmorItems[i].name, chestArmorItems[i]);
                }
            }

            int handItemsCount = handArmorItems.Count;
            for (int i = 0; i < handItemsCount; i++)
            {
                if (!handArmorItemsDict.ContainsKey(handArmorItems[i].name))
                {
                    handArmorItemsDict.Add(handArmorItems[i].name, handArmorItems[i]);
                }
            }

            int legItemsCount = legArmorItems.Count;
            for (int i = 0; i < legItemsCount; i++)
            {
                if (!legArmorItemsDict.ContainsKey(legArmorItems[i].name))
                {
                    legArmorItemsDict.Add(legArmorItems[i].name, legArmorItems[i]);
                }
            }
            #endregion

            #region Charm.
            int charmItemsCount = charmItems.Count;
            for (int i = 0; i < charmItemsCount; i++)
            {
                if (!charmItemsDict.ContainsKey(charmItems[i].name))
                {
                    charmItemsDict.Add(charmItems[i].name, charmItems[i]);
                }
            }
            #endregion

            #region Powerup.
            int powerupItemsCount = powerupItems.Count;
            for (int i = 0; i < powerupItemsCount; i++)
            {
                if (!powerupItemsDict.ContainsKey(powerupItems[i].name))
                {
                    powerupItemsDict.Add(powerupItems[i].name, powerupItems[i]);
                }
            }
            #endregion

            #region Ring.
            int ringItemsCount = ringItems.Count;
            for (int i = 0; i < ringItemsCount; i++)
            {
                if (!ringItemsDict.ContainsKey(ringItems[i].name))
                {
                    ringItemsDict.Add(ringItems[i].name, ringItems[i]);
                }
            }
            #endregion

            #region Consumable.
            int statesEffectConsumableItemsCount = statsEffectConsumableItems.Count;
            for (int i = 0; i < statesEffectConsumableItemsCount; i++)
            {
                if (!statsEffectConsumableItemsDict.ContainsKey(statsEffectConsumableItems[i].name))
                {
                    statsEffectConsumableItemsDict.Add(statsEffectConsumableItems[i].name, statsEffectConsumableItems[i]);
                }
            }

            int throwableConsumableItemsCount = throwableConsumableItems.Count;
            for (int i = 0; i < throwableConsumableItemsCount; i++)
            {
                if (!throwableConsumableItemsDict.ContainsKey(throwableConsumableItems[i].name))
                {
                    throwableConsumableItemsDict.Add(throwableConsumableItems[i].name, throwableConsumableItems[i]);
                }
            }
            #endregion
        }

        #region Weapon.
        public WeaponItem GetPlayerWeaponById(string weaponId)
        {
            weaponItemsDict.TryGetValue(weaponId, out WeaponItem retVal);
            return retVal;
        }
        #endregion

        #region Armor.
        public HeadArmorItem GetPlayerHeadArmorById(string headArmorId)
        {
            headArmorItemsDict.TryGetValue(headArmorId, out HeadArmorItem retVal);
            return retVal;
        }

        public ChestArmorItem GetPlayerChestArmorById(string chestArmorId)
        {
            chestArmorItemsDict.TryGetValue(chestArmorId, out ChestArmorItem retVal);
            return retVal;
        }

        public HandArmorItem GetPlayerHandArmorById(string handArmorId)
        {
            handArmorItemsDict.TryGetValue(handArmorId, out HandArmorItem retVal);
            return retVal;
        }

        public LegArmorItem GetPlayerLegArmorById(string legArmorId)
        {
            legArmorItemsDict.TryGetValue(legArmorId, out LegArmorItem retVal);
            return retVal;
        }
        #endregion

        #region Charm.
        public CharmItem GetPlayerCharmById(string charmId)
        {
            charmItemsDict.TryGetValue(charmId, out CharmItem retVal);
            return retVal;
        }
        #endregion

        #region Powerup.
        public PowerupItem GetPlayerPowerupById(string powerupId)
        {
            powerupItemsDict.TryGetValue(powerupId, out PowerupItem retVal);
            return retVal;
        }
        #endregion

        #region Ring.
        public RingItem GetPlayerRingById(string ringId)
        {
            ringItemsDict.TryGetValue(ringId, out RingItem retVal);
            return retVal;
        }
        #endregion

        #region Cosumable.
        public StatsEffectConsumableItem GetPlayerStatsEffectConsumableById(string statsEffectConsumableId)
        {
            statsEffectConsumableItemsDict.TryGetValue(statsEffectConsumableId, out StatsEffectConsumableItem retVal);
            return retVal;
        }

        public ThrowableConsumableItem GetPlayerThrowableConsumableById(string throwableConsumableId)
        {
            throwableConsumableItemsDict.TryGetValue(throwableConsumableId, out ThrowableConsumableItem retVal);
            return retVal;
        }
        #endregion

        #endregion

        #region ENEMY.
        [Header("Enemy Weapons.")]
        public List<EnemyWeapon> enemyWeapons = new List<EnemyWeapon>();
        Dictionary<string, EnemyWeapon> enemyWeaponDict = new Dictionary<string, EnemyWeapon>();

        public void InitEnemyResources()
        {
            for (int i = 0; i < enemyWeapons.Count; i++)
            {
                if (!enemyWeaponDict.ContainsKey(enemyWeapons[i].name))
                {
                    enemyWeaponDict.Add(enemyWeapons[i].name, enemyWeapons[i]);
                }
            }
        }

        public EnemyWeapon GetEnemyWeapon(string targetId)
        {
            enemyWeaponDict.TryGetValue(targetId, out EnemyWeapon retVal);
            return retVal;
        }
        #endregion
    }
}