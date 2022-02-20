using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    [Serializable]
    public class FixDirectionMoveMod : AIMod
    {
        [HideInInspector]
        public bool showFixDirectionMoveMod;

        [Header("Anim_Hash")]
        private int e_mod_IsMovingInFixDirection_hash;

        [HideInInspector] public int e_fix_direction_180;
        [HideInInspector] public int e_fix_direction_r90;
        [HideInInspector] public int e_fix_direction_l90;
        [HideInInspector] public int e_fix_direction_end;

        [HideInInspector] public int e_SW_fix_direction_180;
        [HideInInspector] public int e_SW_fix_direction_r90;
        [HideInInspector] public int e_SW_fix_direction_l90;

        [HideInInspector] public int e_FW_fix_direction_180;
        [HideInInspector] public int e_FW_fix_direction_r90;
        [HideInInspector] public int e_FW_fix_direction_l90;

        [SerializeField]
        private FixDirectionTypeEnum targetMoveDirection;

        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy going to keep walking/running towards 1 direction.")]
        private float stdFixDirectionMoveRate = 0;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdMoveRate\".")]
        private float fixDirectionMoveRateRandomizeAmount = 0;

        [SerializeField]
        [Tooltip("The amount of time without randomized that enemy need to wait before execute move in fix direction action again.")]
        private float stdFixDirectionWaitRate = 0;

        [SerializeField]
        [Tooltip("The amount of time to cut or add on \"stdMovingWaitRate\".")]
        private float fixDirectionWaitRateRandomizeAmount = 0;

        [ReadOnlyInspector]
        public Vector3 calculatedFixDirection;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy going to keep walking/running towards 1 direction without randomized.")]
        private float finalizedFixDirectionMoveTime;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("The finalized amount of time that enemy going to wait before execute move in fix direction action again.")]
        private float finalizedFixDirectionWaitTime;

        [ReadOnlyInspector]
        public bool applyFixDirMoveRootMotion = false;

        [ReadOnlyInspector]
        [Tooltip("Enemy will be moving in 1 direction when this true, this will be false once \"moveTimer\" is equal/higher than \"moveRate\".")]
        public bool isMovingInFixDirection = false;

        [ReadOnlyInspector]
        [Tooltip("True when enemy is forced to stop when he faced a wall or enemy finished moving in fix direction action.")]
        public bool isFixDirectionInCooldown;

        [ReadOnlyInspector]
        public bool isHitByChargeAttackWhenMoving;

        [NonSerialized] AIManager _ai;
        [NonSerialized] public float _delta;

        /// INIT

        public void FixDirectionMoveModInit(AIManager _ai)
        {
            this._ai = _ai;
            
            HashManager hashManager = _ai.hashManager;
            e_mod_IsMovingInFixDirection_hash = hashManager.e_mod_IsMovingInFixDirection_hash;
            e_fix_direction_180 = hashManager.e_fix_direction_180;
            e_fix_direction_l90 = hashManager.e_fix_direction_l90;
            e_fix_direction_r90 = hashManager.e_fix_direction_r90;
            e_fix_direction_end = hashManager.e_fix_direction_end;

            e_SW_fix_direction_180 = hashManager.e_SW_fix_direction_180;
            e_SW_fix_direction_l90 = hashManager.e_SW_fix_direction_l90;
            e_SW_fix_direction_r90 = hashManager.e_SW_fix_direction_r90;

            e_FW_fix_direction_180 = hashManager.e_FW_fix_direction_180;
            e_FW_fix_direction_l90 = hashManager.e_FW_fix_direction_l90;
            e_FW_fix_direction_r90 = hashManager.e_FW_fix_direction_r90;
        }

        public void FixDirectionMoveGoesAggroReset()
        {
            RandomizeWithAddonValue(fixDirectionMoveRateRandomizeAmount, stdFixDirectionMoveRate, ref finalizedFixDirectionMoveTime);
            RandomizeWithAddonValue(fixDirectionWaitRateRandomizeAmount, stdFixDirectionWaitRate, ref finalizedFixDirectionWaitTime);

            isFixDirectionInCooldown = true;
        }

        public void FixDirectionMoveExitAggroReset()
        {
            calculatedFixDirection = _ai.vector3Zero;

            isFixDirectionInCooldown = false;
            applyFixDirMoveRootMotion = false;
            isMovingInFixDirection = false;
            _ai.anim.SetBool(e_mod_IsMovingInFixDirection_hash, false);
        }

        /// TICK

        public void FixDirectionMoveTimeCount()
        {
            MonitorMovingInFixDirectionTimer();
            MonitorFixDirectionCoolDownTimer();

            void MonitorMovingInFixDirectionTimer()
            {
                if (isMovingInFixDirection)
                {
                    finalizedFixDirectionMoveTime -= _delta;
                    if (finalizedFixDirectionMoveTime <= 0)
                    {
                        finalizedFixDirectionMoveTime = 0;
                        OffFixDirectionMove();
                    }
                }
            }

            void MonitorFixDirectionCoolDownTimer()
            {
                if (isFixDirectionInCooldown)
                {
                    finalizedFixDirectionWaitTime -= _delta;
                    if (finalizedFixDirectionWaitTime <= 0)
                    {
                        finalizedFixDirectionWaitTime = 0;
                        OffFixDirectionInCooldown();
                    }
                }
            }
        }
        
        /// SWITCH WEAPON SET STATUS

        public void SwitchTo_FW_SetStatus()
        {
            isFixDirectionInCooldown = false;
        }

        public void SwitchTo_SW_SetStatus()
        {
            isFixDirectionInCooldown = true;
            RandomizeWithAddonValue(fixDirectionMoveRateRandomizeAmount, stdFixDirectionMoveRate, ref finalizedFixDirectionMoveTime);
        }

        /// SET METHODS

        void OnFixDirectionMove()
        {
            _ai.OnFixDirectionMove();
        }
        
        public void OffFixDirectionMove()
        {
            isMovingInFixDirection = false;
            applyFixDirMoveRootMotion = false;

            _ai.OffFixDirectionMove();

            _ai.anim.SetBool(e_mod_IsMovingInFixDirection_hash, false);
            _ai.PlayAnimationCrossFade(e_fix_direction_end, 0.2f, true);

            isFixDirectionInCooldown = true;
            RandomizeWithAddonValue(fixDirectionWaitRateRandomizeAmount, stdFixDirectionWaitRate, ref finalizedFixDirectionWaitTime);
        }

        void OffFixDirectionInCooldown()
        {
            isFixDirectionInCooldown = false;
            RandomizeWithAddonValue(fixDirectionMoveRateRandomizeAmount, stdFixDirectionMoveRate, ref finalizedFixDirectionMoveTime);
        }
        
        /// On Hit.

        public void OnChargedAttackHit()
        {
            if (isMovingInFixDirection)
            {
                isMovingInFixDirection = false;
                applyFixDirMoveRootMotion = false;
                isHitByChargeAttackWhenMoving = true;

                _ai.HitByChargeAttack_OffFixDirectionMove(e_mod_IsMovingInFixDirection_hash);

                isFixDirectionInCooldown = true;
                RandomizeWithAddonValue(fixDirectionWaitRateRandomizeAmount, stdFixDirectionWaitRate, ref finalizedFixDirectionWaitTime);
            }
        }
        
        /// AI ACTION METHODS

        public void SetIsMovingFixDirectionToTrue()
        {
            if (!isMovingInFixDirection)
            {
                isMovingInFixDirection = true;
                OnFixDirectionMove();
                SetCurrentFixDirectionEnum(true);
            }
        }

        void SetCurrentFixDirectionEnum(bool isDualWeapon)
        {
            switch (targetMoveDirection)
            {
                case FixDirectionTypeEnum.moveToBack:
                    calculatedFixDirection = StaticHelper.GetNewRotatedVector3(_ai.mTransform.forward, -180);

                    if (isDualWeapon)
                    {
                        if (_ai.GetIsUsingSecondWeaponBool())
                        {
                            _ai.PlayAnimationCrossFade(e_SW_fix_direction_180, 0.1f, true);
                        }
                        else
                        {
                            _ai.PlayAnimationCrossFade(e_FW_fix_direction_180, 0.1f, true);
                        }
                    }
                    else
                    {
                        _ai.PlayAnimationCrossFade(e_fix_direction_180, 0.1f, true);
                    }

                    break;
                case FixDirectionTypeEnum.moveToRight:
                    calculatedFixDirection = _ai.mTransform.right;

                    if (isDualWeapon)
                    {
                        if (_ai.GetIsUsingSecondWeaponBool())
                        {
                            _ai.PlayAnimationCrossFade(e_SW_fix_direction_r90, 0.1f, true);
                        }
                        else
                        {
                            _ai.PlayAnimationCrossFade(e_FW_fix_direction_r90, 0.1f, true);
                        }
                    }
                    else
                    {
                        _ai.PlayAnimationCrossFade(e_fix_direction_r90, 0.1f, true);
                    }

                    break;
                case FixDirectionTypeEnum.moveToLeft:
                    calculatedFixDirection = StaticHelper.GetNewRotatedVector3(_ai.mTransform.right, 180);

                    if (isDualWeapon)
                    {
                        if (_ai.GetIsUsingSecondWeaponBool())
                        {
                            _ai.PlayAnimationCrossFade(e_SW_fix_direction_l90, 0.1f, true);
                        }
                        else
                        {
                            _ai.PlayAnimationCrossFade(e_FW_fix_direction_l90, 0.1f, true);
                        }
                    }
                    else
                    {
                        _ai.PlayAnimationCrossFade(e_fix_direction_l90, 0.1f, true);
                    }

                    break;
            }

            _ai.anim.SetBool(e_mod_IsMovingInFixDirection_hash, true);
        }

        public enum FixDirectionTypeEnum
        {
            moveToBack,
            moveToFront,
            moveToRight,
            moveToLeft
        }
    }
}