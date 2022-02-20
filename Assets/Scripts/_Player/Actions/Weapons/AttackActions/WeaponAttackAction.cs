using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class WeaponAttackAction : WeaponAction
    {
        [Header("Attack Refs.")]
        public Player_AttackRefs _playerAttackRefs;

        [Header("Combo.")]
        public ComboBranches comboBranches;
        
        [Header("Stats.")]
        public float staminaUsage;
        public bool isTurnWithMoveDirWhenLockon;

        [Header("Root.")]
        public bool ignoreRootCalculate;
        public float actionRootMotion;

        #region Normal Attack WA Effect Funcs.
        public virtual void Play_1st_Effect()
        {
        }

        public virtual void Play_2nd_Effect()
        {
        }

        public virtual void Play_3rd_Effect()
        {
        }

        public virtual void Play_4th_Effect()
        {
        }
        #endregion
        
        /// Attack Direction comes from enemy perspective.
        public enum AttackDirectionTypeEnum
        {
            HitFromLeft,
            HitFromRight,
            HitFromFront,
            Irrelvant           /// Execution is equivalent to None.
        }

        public enum PhysicalAttackTypeEnum
        {
            Strike,
            Slash,
            Thrust,
            Irrelvant           /// Execution is equivalent to None.
        }

        public enum AttackActionTypeEnum
        {
            Normal,
            Hold,
            Charged,
            ParryExecution,
            Irrelvant
        }
    }
}