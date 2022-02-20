using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class FullBodyDamageColliderMod : AIMod
    {
        [HideInInspector]
        public bool show_FullBody_DC_Mod;

        public StandardEnemyDamageCollider _r_arm_damageCollider;
        public StandardEnemyDamageCollider _l_arm_damageCollider;
        public StandardEnemyDamageCollider _r_leg_damageCollider;
        public StandardEnemyDamageCollider _l_leg_damageCollider;

        [ReadOnlyInspector] public StandardEnemyDamageCollider _prev_DamageCollider;

        /// Init.
        public void FullBodyDamageColliderModInit(AIManager ai)
        {
            LayerManager _layerManager = LayerManager.singleton;
            
            /// L Arm.
            FullBodyDamageColliderInit(_l_arm_damageCollider);

            /// R Arm.
            FullBodyDamageColliderInit(_r_arm_damageCollider);

            /// L Leg.
            FullBodyDamageColliderInit(_l_leg_damageCollider);

            /// R Leg.
            FullBodyDamageColliderInit(_r_leg_damageCollider);

            void FullBodyDamageColliderInit(StandardEnemyDamageCollider collider)
            {
                collider._ai = ai;
                collider.playerStates = ai.playerStates;
                collider._layerManager = _layerManager;
                collider.gameObject.layer = _layerManager.enemyDamageColliderLayer;
                collider._collider.enabled = false;
                collider._collider.isTrigger = true;
            }
        }

        /// On Player Death / On Enemy Death.
        public void FullBodyDamageColliderOnDeathReset()
        {
            DisablePreviousDamageCollider();
        }

        /// Set Status.
        // R Arm.
        public void Enable_R_Arm_DamageCollider()
        {
            _r_arm_damageCollider._collider.enabled = true;
            _prev_DamageCollider = _r_arm_damageCollider;
        }

        public void Disable_R_Arm_DamageCollider()
        {
            _r_arm_damageCollider._collider.enabled = false;
        }

        // L Arm.
        public void Enable_L_Arm_DamageCollider()
        {
            _l_arm_damageCollider._collider.enabled = true;
            _prev_DamageCollider = _l_arm_damageCollider;
        }

        public void Disable_L_Arm_DamageCollider()
        {
            _l_arm_damageCollider._collider.enabled = false;
        }

        // R Leg.
        public void Enable_R_Leg_DamageCollider()
        {
            _r_leg_damageCollider._collider.enabled = true;
            _prev_DamageCollider = _r_leg_damageCollider;
        }

        public void Disable_R_Leg_DamageCollider()
        {
            _r_leg_damageCollider._collider.enabled = false;
        }

        // L Leg.
        public void Enable_L_Leg_DamageCollider()
        {
            _l_leg_damageCollider._collider.enabled = true;
            _prev_DamageCollider = _l_leg_damageCollider;
        }

        public void Disable_L_Leg_DamageCollider()
        {
            _l_leg_damageCollider._collider.enabled = false;
        }

        // Previous.
        public void DisablePreviousDamageCollider()
        {
            if (_prev_DamageCollider != null)
                _prev_DamageCollider._collider.enabled = false;
        }
    }
}