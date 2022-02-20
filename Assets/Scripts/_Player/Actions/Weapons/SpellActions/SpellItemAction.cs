using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SpellItemAction : ItemActions
    {
        // SPELL ITEM ACTION INIT THE CURRENT SPELL
        // SPELL ITEM ACTION --> SPELL ITEM --> SPELL ACTION(FIRE BALL

        public override void Execute(StateManager states)
        {
            if(states._savableInventory._spell != null)
            {
                //states.statsHandler._fp -= states._savableInventory._spell.focusCost;
                states._savableInventory._spell.InitCast(states);
                //statesVariable.currentState = targetState;
            }
            else
            {
                //states.PlayAnimationWithCrossFade(HashManager.singleton.p_cant_spell_hash, false, true);
                states.currentState = GameManager.singleton.playerWaitForAnimState;
            }
        }
    }
}