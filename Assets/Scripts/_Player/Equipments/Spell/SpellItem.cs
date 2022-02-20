using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Equipments/Spell/Spell Item")]
    public class SpellItem : Item
    {
        public SpellAction spellAction;
        public GameObject spellParticle;
        public GameObject spellProjectile;
        //public ConsumableItem.EffectTypeEnum effectType;
        public int focusCost;

        [HideInInspector] public GameObject spellParticleInstance;

        public override void CreateInstanceForPickups(SavableInventory _inventory)
        {
        }

        public override void CreateInstanceForPickupsWithAmount(SavableInventory _inventory, int _amount)
        {
        }

        public void Init(StateManager states, Transform parentTransform)
        {
            if(spellParticle != null)
            {
                spellParticleInstance = Instantiate(spellParticle) as GameObject;
                if(parentTransform != null)
                {
                    spellParticleInstance.transform.parent = parentTransform;
                    spellParticleInstance.transform.localPosition = states.vector3Zero;
                    spellParticleInstance.transform.localEulerAngles = states.vector3Zero;
                }
            }
        }

        public void InitCast(StateManager states)
        {
            spellAction.PrepareCast(states);
        }
    }
}