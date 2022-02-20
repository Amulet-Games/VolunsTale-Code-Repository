using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Unique Actions/Set SpellItem Action")]
    public class SetSpellAction : ScriptableObject
    {
        public void Execute(StateManager states)
        {
            SpellItem spell = states._savableInventory.GetNextSpellOnSlot(); ;

            if (spell != null)
            {
                if (spell.spellParticleInstance == null)
                    spell.Init(states, states.mTransform);

                states._savableInventory.SetSpell(spell, true);
                states._savableInventory.SetSpellActive();
            }
        }
    }
}