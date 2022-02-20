using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Init/LoadPlayerGearsFromSave")]
    public class LoadPlayerGearsFromSave : StateActions
    {
        public override void Execute(StateManager _states)
        {
            /// GET ALL REFERENCES.
            ResourcesManager rm = GameManager.singleton._resourcesManager;
            SavableInventory _inventory = _states._savableInventory;

            /// PRE RUNTIME STATS INIT.
            StatsAttributeHandler _statsHandler = _states.statsHandler;
            _statsHandler.InitRuntimeStats_BaseNonChangeable();

            /// INIT DEFAULTS.
            rm.unarmedWeaponItem.InitUnarmedItem(_inventory);
            _inventory.SetIsLeftUnarmedStatus(true);
            _inventory.SetIsRightUnarmedStatus(true);

            rm.deprivedHeadArmorItem.InitDeprivedHead(_inventory);
            rm.deprivedChestArmorItem.InitDeprivedChest(_inventory);
            rm.deprivedHandArmorItem.InitDeprivedHand(_inventory);
            rm.deprivedLegArmorItem.InitDeprivedLeg(_inventory);

            rm.volunAmuletItem.GetNewAmuletInstance(_inventory);

            /// INIT ITEMS.
            _inventory.LoadItemsFromSave(rm);

            /// CONSUMABLE ITEMS QSLOT.
            _inventory.RefreshNextTwoConsumables();

            /// REGISTER DEFUALT ARMOR.
            if (_inventory.headArmorSlot == null)
                _inventory.RegisterDeprivedHead();

            if (_inventory.chestArmorSlot == null)
                _inventory.RegisterDeprivedChest();

            if (_inventory.handArmorSlot == null)
                _inventory.RegisterDeprivedHand();

            if (_inventory.legArmorSlot == null)
                _inventory.RegisterDeprivedLeg();

            /// COMBINE SKIN MESH.
            _states.CombineSkinnedMesh();

            /// INIT PLAYER RUNTIME STATS.
            _statsHandler.InitRuntimeStats_LoadGame_BaseChangeable();

            /// RENEW DEFENSE IK GOAL.
            _states._playerIKHandler.PostInitStateActionSetup();
            
            Debug.Log("Player Starter Class Gear Loaded Successfully.");
        }
        
        public override void AIExecute(AIStateManager aIState)
        {
            throw new System.NotImplementedException();
        }
    }
}