using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class R_Leg_DamageColliderMod : AIMod
    {
        [HideInInspector]
        public bool show_R_Leg_DC_Mod;

        public StandardEnemyDamageCollider _r_leg_damageCollider;

        /// Init.
        public void R_Leg_DamageColliderModInit(AIManager ai)
        {
            _r_leg_damageCollider.Setup(ai);
        }

        /// On Player Death / On Enemy Death.
        public void R_Leg_DamageColliderOnDeathReset()
        {
            Disable_R_Leg_DamageCollider();
        }

        /// Set Status.
        public void Enable_R_Leg_DamageCollider()
        {
            _r_leg_damageCollider._collider.enabled = true;
        }

        public void Disable_R_Leg_DamageCollider()
        {
            _r_leg_damageCollider._collider.enabled = false;
        }
    }
}