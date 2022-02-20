using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Unique Actions/Set SpellItem Action InitSpawnContainerDict")]
    public class SetSpellActionInit : ScriptableObject
    {
        public void Execute(SpellItem targetItem, StateManager states)
        {
            if (targetItem != null)
            {
                if (targetItem.spellParticleInstance == null)
                    targetItem.Init(states, states.mTransform);

                states._savableInventory.SetSpell(targetItem, true);
                states._savableInventory.SetSpellActive();
            }
        }
    }
}