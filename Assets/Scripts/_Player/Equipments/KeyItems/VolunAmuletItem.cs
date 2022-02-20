using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Volun Amulet")]
    public class VolunAmuletItem : ScriptableObject
    {
        [Header("Drag and Drop Refs.")]
        public RuntimeVolunAmulet modelPrefab;
        public AmuletHeldTransform _amuletSheathTransform;   /// always be the hip
        public AmuletHeldTransform _igniteHeldTransform;
        public AmuletHeldTransform _levelupHeldTransform;

        public RuntimeVolunAmulet GetNewAmuletInstance(SavableInventory _inventory)
        {
            RuntimeVolunAmulet _runtimeAmulet = Instantiate(modelPrefab);
            _runtimeAmulet.Init(_inventory, this);

            return _runtimeAmulet;
        }
    }
}