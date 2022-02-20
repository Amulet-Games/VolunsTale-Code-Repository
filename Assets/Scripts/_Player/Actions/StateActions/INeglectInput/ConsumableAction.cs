using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "INeglect Input Action/Consumable Action")]
    public class ConsumableAction : ScriptableObject, INeglectInputAction
    {
        public void Execute(StateManager _states)
        {
            _states._savableInventory.PrepareConsumable();
        }
    }
}