using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class Player_AttackRefs
    {
        [Header("Attack Config.")]
        public AttackDirectionTypeEnum _attackDirectionType;
        public AttackActionTypeEnum _attackActionType;
        public AttackPhysicalTypeEnum _attackPhysicalType;

        /// Attack Direction comes from enemy perspective.
        public enum AttackDirectionTypeEnum
        {
            HitFromLeft,
            HitFromRight,
            HitFromFront,
            Irrelvant       // e.g. Shield Parry
        }
        
        public enum AttackActionTypeEnum
        {
            Normal,
            Hold,
            Charged,
            Irrelvant       // e.g. Shield Parry
        }

        public enum AttackPhysicalTypeEnum
        {
            Strike,
            Slash,
            Thrust,
            Irrelvant       // e.g. Shield Parry
        }
    }
}