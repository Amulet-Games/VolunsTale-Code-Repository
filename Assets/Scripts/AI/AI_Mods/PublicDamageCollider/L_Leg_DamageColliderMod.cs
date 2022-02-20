using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class L_Leg_DamageColliderMod : AIMod
    {
        [HideInInspector]
        public bool show_L_Leg_DC_Mod;

        public StandardEnemyDamageCollider _l_leg_damageCollider;

        /// Init.
        public void L_Leg_DamageColliderModInit(AIManager ai)
        {
            _l_leg_damageCollider.Setup(ai);
        }

        /// On Player Death / On Enemy Death.
        public void L_Leg_DamageColliderOnDeathReset()
        {
            _l_leg_damageCollider._collider.enabled = false;
        }

        /// Set Status.
        public void Enable_L_Leg_DamageCollider()
        {
            _l_leg_damageCollider._collider.enabled = true;
        }

        public void Disable_L_Leg_DamageCollider()
        {
            _l_leg_damageCollider._collider.enabled = false;
        }
    }
}