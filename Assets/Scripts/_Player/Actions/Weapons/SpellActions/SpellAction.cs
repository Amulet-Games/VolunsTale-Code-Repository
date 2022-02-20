using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class SpellAction : ScriptableObject
    {
        public abstract void PrepareCast(StateManager state);

        public abstract bool Cast(StateManager state);

        /*
        public virtual void CreateSpellParticles(SpellItem spellItem, Transform parentTransform)
        {
            if (spellItem.spellParticle == null)
                return;

            GameObject go = Instantiate(spellItem.spellParticle) as GameObject;
            if(parentTransform != null)
            {
                go.SetActive(false);
                go.transform.parent = parentTransform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localEulerAngles = Vector3.zero;
                go.SetActive(true);
            }
        }
        */
    }
}