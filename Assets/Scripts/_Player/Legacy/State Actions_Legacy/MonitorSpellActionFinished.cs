using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/Ticks/Monitor Spell Action Finished")]
    public class MonitorSpellActionFinished : StateActions
    {
        public override void Execute(StateManager states)
        {
            SpellItem currentSpell = states._savableInventory._spell;
            if (currentSpell != null)
            {
                //if(currentSpell.effectType == ConsumableItem.EffectTypeEnum.Instant)
                //{
                //    if(states.isCastingFinished == false)
                //    {
                //        states.isCastingFinished = currentSpell.spellAction.Cast(states);
                //    }

                //    bool isAninFinished = !states.anim.GetBool(states.p_IsNeglecting_hash);
                //    if (isAninFinished && states.isCastingFinished)
                //    {
                //        states.generalTime = 0;

                //        //Turn Particle off

                //        states.isCastingFinished = false;
                //        currentSpell = null;
                //    }
                //}
                //else
                //{
                //    bool isPeriodCastingFinished = currentSpell.spellAction.Cast(states);

                //    bool isAnimFinished = !states.anim.GetBool(states.p_IsNeglecting_hash);
                //    if (isAnimFinished && isPeriodCastingFinished)
                //    {
                //        states.generalTime = 0;

                //        //Turn Particle off

                //        currentSpell = null;
                //    }
                //}
            }
        }

        public override void AIExecute(AIStateManager aIState)
        {
            //throw new NotImplementedException();
        }
    }
}