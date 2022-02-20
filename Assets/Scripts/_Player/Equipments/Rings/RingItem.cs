using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Rings/Ring Item")]
    public class RingItem : Item
    {
        #region Item Stats.
        [Header("Item Stats.")]
        public int weight;

        [Header("Effect Texts."), TextArea(1, 30)]
        public string ringEffectText;
        #endregion

        [Header("Drag and Drop Refs.")]
        public RuntimeRing modelPrefab;

        public RuntimeRing GetNewRuntimeRingInstance(SavableInventory _inventory)
        {
            RuntimeRing _runtimeRing = Instantiate(modelPrefab);
            _runtimeRing.InitRuntimeRing(this);
            _runtimeRing.runtimeName = _inventory.GetFortifiedRingName(_runtimeRing);

            _inventory.ReturnRingToBackpack(_runtimeRing);
            _inventory.AddRingToCarrying(_runtimeRing);
            return _runtimeRing;
        }

        public RuntimeRing GetSaveFileRuntimeRingInstance(SavableRingState _savableRingState, SavableInventory _inventory)
        {
            RuntimeRing _runtimeRing = Instantiate(modelPrefab);
            _runtimeRing.InitRuntimeRingFromSave(_savableRingState, this);
            _runtimeRing.runtimeName = _inventory.GetFortifiedRingName(_runtimeRing);

            _inventory.AddRingToCarrying(_runtimeRing);
            return _runtimeRing;
        }

        #region Pickups.
        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
            GetNewRuntimeRingInstance(_inventory);
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
            GetNewRuntimeRingInstance(_inventory);
        }
        #endregion

        public override RingItem GetRingItem()
        {
            return this;
        }
    }
}