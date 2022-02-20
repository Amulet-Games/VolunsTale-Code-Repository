using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ArrowItem : Item
    {
        [Header("Drag and Drop Refs.")]
        public RuntimeArrow modelPrefab;

        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
        }
    }
}