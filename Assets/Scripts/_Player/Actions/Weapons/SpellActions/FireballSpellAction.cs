using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Spell Actions/Fireball Spell Action")]
    public class FireballSpellAction : SpellAction
    {
        public override void PrepareCast(StateManager states)
        {
            //states.PlayAnimationWithCrossFade(HashManager.singleton.p_throw_hash, true, true);
            states.currentState = GameManager.singleton.playerWaitForAnimState;

            //state.forceEndActions = true;
            states._savableInventory._spell.spellParticleInstance.SetActive(true);
        }

        public override bool Cast(StateManager states)
        {
            return true;
        }
    }
}
