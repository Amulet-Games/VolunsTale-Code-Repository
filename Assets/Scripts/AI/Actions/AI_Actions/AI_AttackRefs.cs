using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [System.Serializable]
    public class AI_AttackRefs
    {
        [Header("Attack Config.")]
        public int bfxId;
        public bool isRandomizeBfxId;
        public AIAttackTypeEnum _attackType;
        public AIAttackPhysicalTypeEnum _attackPhysicalType;
        public AIAttackImpactTypeEnum _attackImpactType;
        public float _attackBaseDamage;

        public enum AIAttackTypeEnum
        {
            Melee,
            Projectile,
            AOE
        }

        public enum AIAttackPhysicalTypeEnum
        {
            Strike,
            Slash,
            Thrust,
            AOE
        }
        
        public enum AIAttackImpactTypeEnum
        {
            Normal,         /// Normals, AOEs
            Big,            /// Charged Attacks, Javelin, Bomb
            Knockback
        }
    }
}