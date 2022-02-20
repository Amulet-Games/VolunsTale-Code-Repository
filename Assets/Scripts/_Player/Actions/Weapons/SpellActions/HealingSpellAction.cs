using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Spell Actions/Healing Spell Action")]
    public class HealingSpellAction : SpellAction
    {
        public int healAmount = 25;
        public float healthRecoverTime = 2;
        public float totalEffectTime;
        public AnimStateVariable prepareAnim;

        private float internalTime = 0;

        public override void PrepareCast(StateManager states)
        {
            states.CrossFadeAnimWithMoveDir(prepareAnim.animStateHash, false, true);
            states.currentState = GameManager.singleton.playerWaitForAnimState;

            //state.forceEndActions = true;
            states._savableInventory._spell.spellParticleInstance.SetActive(true);
        }

        public override bool Cast(StateManager states)
        {
            bool r = false;

            //states.generalTime += states._delta;
            //if (states.generalTime >= totalEffectTime)
            //{
            //    states.savableInventory._spell.spellParticleInstance.SetActive(false);
            //    r = true;
            //}
            //else
            //{
            //    internalTime += Time.deltaTime;
            //    if (internalTime >= healthRecoverTime)
            //    {
            //        internalTime = 0;
            //        states.statsHandler._hp += healAmount;
            //    }

            //}
            return r;
        }
    }
}
