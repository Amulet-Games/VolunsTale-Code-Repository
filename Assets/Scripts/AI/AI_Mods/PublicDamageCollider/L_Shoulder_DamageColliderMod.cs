using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class L_Shoulder_DamageColliderMod : AIMod
    {
        [HideInInspector]
        public bool show_L_Shoulder_DC_Mod;

        public StandardEnemyDamageCollider _l_shoulder_damageCollider;

        /// Init.
        public void L_Shoulder_DamageColliderModInit(AIManager ai)
        {
            _l_shoulder_damageCollider.Setup(ai);
        }

        /// On Player Death / On Enemy Death.
        public void L_Shoulder_DamageColliderOnDeathReset()
        {
            Disable_L_Shoulder_DamageCollider();
        }

        /// Set Status.
        public void Enable_L_Shoulder_DamageCollider()
        {
            _l_shoulder_damageCollider._collider.enabled = true;
        }

        public void Disable_L_Shoulder_DamageCollider()
        {
            _l_shoulder_damageCollider._collider.enabled = false;
        }
    }
}