using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Init/CreatePlayerStarterClassGears")]
    public class CreatePlayerStarterClassGears : StateActions
    {
        public override void Execute(StateManager _states)
        {
            /// GET ALL REFERENCES.
            ResourcesManager rm = GameManager.singleton._resourcesManager;
            StarterClassProfile profile = _states._currentProfile;
            SavableInventory _inventory = _states._savableInventory;

            /// PRE RUNTIME STATS INIT.
            StatsAttributeHandler _statsHandler = _states.statsHandler;
            _statsHandler.InitRuntimeStats_BaseNonChangeable();

            /// INIT ITEMS.

            #region Weapons.
            rm.unarmedWeaponItem.InitUnarmedItem(_inventory);

            InitWeaponsFromBothEmptyHands();

            void InitWeaponsFromBothEmptyHands()
            {
                if (!_inventory._leftHandWeapon)
                    _inventory.SetIsLeftUnarmedStatus(true);

                if (!_inventory._rightHandWeapon)
                    _inventory.SetIsRightUnarmedStatus(true);

                for (int i = 0; i < 3; i++)
                {
                    if (!string.IsNullOrEmpty(profile.rightHandWeaponIds[i]))
                    {
                        WeaponItem rhWeapon = rm.GetPlayerWeaponById(profile.rightHandWeaponIds[i]);
                        _inventory.InitRhWeaponSlot(rhWeapon.GetNewRuntimeWeaponInstance(_inventory), i);
                    }
                }
                
                for (int i = 0; i < 3; i++)
                {
                    if (!string.IsNullOrEmpty(profile.leftHandWeaponIds[i]))
                    {
                        WeaponItem lhWeapon = rm.GetPlayerWeaponById(profile.leftHandWeaponIds[i]);
                        _inventory.InitLhWeaponSlot(lhWeapon.GetNewRuntimeWeaponInstance(_inventory), i);
                    }
                }
            }

            void InitWeaponsFromBothHoldingHands()
            {
                for (int i = 0; i < 3; i++)
                {
                    if (!string.IsNullOrEmpty(profile.rightHandWeaponIds[i]))
                    {
                        WeaponItem rhWeapon = rm.GetPlayerWeaponById(profile.rightHandWeaponIds[i]);
                        _inventory.InitRhWeaponSlot(rhWeapon.GetNewRuntimeWeaponInstance(_inventory), i);
                    }
                }

                if (!_inventory._rightHandWeapon)
                    _inventory.SetIsRightUnarmedStatus(true);

                for (int i = 0; i < 3; i++)
                {
                    if (!string.IsNullOrEmpty(profile.leftHandWeaponIds[i]))
                    {
                        WeaponItem lhWeapon = rm.GetPlayerWeaponById(profile.leftHandWeaponIds[i]);
                        _inventory.InitLhWeaponSlot(lhWeapon.GetNewRuntimeWeaponInstance(_inventory), i);
                    }
                }

                if (!_inventory._leftHandWeapon)
                    _inventory.SetIsLeftUnarmedStatus(true);
            }
            #endregion

            #region Armors.
            rm.deprivedHeadArmorItem.InitDeprivedHead(_inventory);
            rm.deprivedChestArmorItem.InitDeprivedChest(_inventory);
            rm.deprivedHandArmorItem.InitDeprivedHand(_inventory);
            rm.deprivedLegArmorItem.InitDeprivedLeg(_inventory);

            if (!string.IsNullOrEmpty(profile.headArmorId))
            {
                HeadArmorItem headArmorItem = rm.GetPlayerHeadArmorById(profile.headArmorId);
                _inventory.InitHeadArmorEmptySlot(headArmorItem.GetNewRuntimeHeadInstance(_inventory));
            }
            else
            {
                _inventory.RegisterDeprivedHead();
            }

            if (!string.IsNullOrEmpty(profile.chestArmorId))
            {
                ChestArmorItem chestArmorItem = rm.GetPlayerChestArmorById(profile.chestArmorId);
                _inventory.InitChestArmorEmptySlot(chestArmorItem.GetNewRuntimeChestInstance(_inventory));
            }
            else
            {
                _inventory.RegisterDeprivedChest();
            }

            if (!string.IsNullOrEmpty(profile.handArmorId))
            {
                HandArmorItem handArmorItem = rm.GetPlayerHandArmorById(profile.handArmorId);
                _inventory.InitHandArmorEmptySlot(handArmorItem.GetNewRuntimeHandInstance(_inventory));
            }
            else
            {
                _inventory.RegisterDeprivedHand();
            }

            if (!string.IsNullOrEmpty(profile.legArmorId))
            {
                LegArmorItem legArmorItem = rm.GetPlayerLegArmorById(profile.legArmorId);
                _inventory.InitLegArmorEmptySlot(legArmorItem.GetNewRuntimeLegInstance(_inventory));
            }
            else
            {
                _inventory.RegisterDeprivedLeg();
            }

            /// COMBINE SKIN MESH.
            _states.CombineSkinnedMesh();
            #endregion

            #region Charm.
            if (!string.IsNullOrEmpty(profile.charmId))
            {
                CharmItem charmItem = rm.GetPlayerCharmById(profile.charmId);
                _inventory.SetupCharmEmptySlot(charmItem.GetNewRuntimeCharmInstance(_inventory));
            }
            #endregion

            #region Powerup.
            if (!string.IsNullOrEmpty(profile.powerupId))
            {
                PowerupItem powerupItem = rm.GetPlayerPowerupById(profile.powerupId);
                _inventory.SetupPowerupEmptySlot(powerupItem.GetNewRuntimePowerupInstance(_inventory));
            }
            #endregion

            #region Rings.
            if (!string.IsNullOrEmpty(profile.rightRingIds))
            {
                RingItem rightRingItem = rm.GetPlayerRingById(profile.rightRingIds);
                _inventory.InitRightRingSlot(rightRingItem.GetNewRuntimeRingInstance(_inventory));
            }

            if (!string.IsNullOrEmpty(profile.leftRingIds))
            {
                RingItem leftRingItem = rm.GetPlayerRingById(profile.leftRingIds);
                _inventory.InitLeftRingSlot(leftRingItem.GetNewRuntimeRingInstance(_inventory));
            }
            #endregion

            #region Consumables.
            rm.volunVesselConsumableItem.GetNewVesselsInstance(_inventory);
            _inventory.AddCarryConsumableToDictionary(_inventory.runtimeVolunVessel);

            rm.sodurVesselConsumableItem.GetNewVesselsInstance(_inventory);
            _inventory.AddCarryConsumableToDictionary(_inventory.runtimeSodurVessel);

            for (int i = 0; i < 8; i++)
            {
                if (profile.starterConsumableInfos == null)
                {
                    Debug.LogError("Start Infos is null");
                }

                StarterConsumableInfo _starterConsumableInfo = profile.starterConsumableInfos[i];
                
                if (!string.IsNullOrEmpty(_starterConsumableInfo.starterConsumableId))
                {
                    switch (_starterConsumableInfo.starterConsumableType)
                    {
                        case ConsumableItem.ConsumableTypeEnum.StatsEffectConsumable:
                            GetStatsEffectConsumableById(_starterConsumableInfo, i);
                            break;

                        case ConsumableItem.ConsumableTypeEnum.ThrowableConsumable:
                            GetThrowableConsumableById(_starterConsumableInfo, i);
                            break;
                    }
                }
            }

            _inventory.RefreshNextTwoConsumables();

            void GetStatsEffectConsumableById(StarterConsumableInfo _starterConsumableInfo, int _i)
            {
                StatsEffectConsumableItem _statsEffectConsumableItem = rm.GetPlayerStatsEffectConsumableById(_starterConsumableInfo.starterConsumableId);
                StatsEffectConsumable _statsEffectConsumable = _statsEffectConsumableItem.GetNewStatsEffectConsumableInstance(_inventory);

                _inventory.InitConsumableSlot(_statsEffectConsumable, _i + 2);
                _inventory.AddCarryConsumableToDictionary(_statsEffectConsumable);
                _statsEffectConsumable.IncrementCarryingAmount(_starterConsumableInfo.carryingAmount);
            }

            void GetThrowableConsumableById(StarterConsumableInfo _starterConsumableInfo, int _i)
            {
                ThrowableConsumableItem _throwableConsumableItem = rm.GetPlayerThrowableConsumableById(_starterConsumableInfo.starterConsumableId);
                ThrowableConsumable _throwableConsumable = _throwableConsumableItem.GetNewThrowableConsumableInstance(_inventory);

                _inventory.InitConsumableSlot(_throwableConsumable, _i + 2);
                _inventory.AddCarryConsumableToDictionary(_throwableConsumable);
                _throwableConsumable.IncrementCarryingAmount(_starterConsumableInfo.carryingAmount);
            }
            #endregion

            #region Amulet.
            rm.volunAmuletItem.GetNewAmuletInstance(_inventory);
            #endregion

            #region Spell.
            //for (int i = 0; i < states.profile.spellId.Length; i++)
            //{
            //    SpellItem spell = (SpellItem)rm.GetItemNewInstance(states.profile.spellId[i]);
            //    if (spell != null)
            //    {
            //        if (i > states.savableInventory.spellSlot.Length - 1)
            //            break;

            //        states.savableInventory.spellSlot[i] = spell;
            //        setSpellAction.Execute(spell, states);
            //        states.savableInventory.carryingItems.Add(spell);
            //    }
            //}
            #endregion

            /// POST RUNTIME STATS INIT.
            _statsHandler.InitRuntimeStats_NewGame_BaseChangeable();

            /// RENEW DEFENSE IK GOAL.
            _states._playerIKHandler.PostInitStateActionSetup();
            
            Debug.Log("Player Starter Class Gear Created Successfully.");
        }
        
        public override void AIExecute(AIStateManager aIState)
        {
            //throw new System.NotImplementedException();
        }
    }
}
