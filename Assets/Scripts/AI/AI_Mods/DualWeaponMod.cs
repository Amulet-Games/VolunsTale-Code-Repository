using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class DualWeaponMod : AIMod
    {
        [HideInInspector]
        public bool showDualWeaponMod;

        [Header("Anim_Hash")]
        [HideInInspector] public int e_mod_IsUsingSecondWeapon;

        [HideInInspector] public int e_armed_FW_hit_small_r_hash;
        [HideInInspector] public int e_armed_FW_hit_small_l_hash;
        [HideInInspector] public int e_armed_FW_hit_small_f_hash;

        [HideInInspector] public int e_armed_FW_hit_big_r_hash;
        [HideInInspector] public int e_armed_FW_hit_big_l_hash;
        [HideInInspector] public int e_armed_FW_hit_big_f_hash;

        [HideInInspector] public int e_armed_FW_knockDown_hash;
        [HideInInspector] public int e_armed_FW_death_hash;

        [HideInInspector] public int e_armed_SW_hit_small_r_hash;
        [HideInInspector] public int e_armed_SW_hit_small_l_hash;
        [HideInInspector] public int e_armed_SW_hit_small_f_hash;

        [HideInInspector] public int e_armed_SW_hit_big_r_hash;
        [HideInInspector] public int e_armed_SW_hit_big_l_hash;
        [HideInInspector] public int e_armed_SW_hit_big_f_hash;

        [HideInInspector] public int e_armed_SW_knockDown_hash;
        [HideInInspector] public int e_armed_SW_knockDown_HitFromBack_hash;
        [HideInInspector] public int e_armed_SW_death_hash;

        [HideInInspector] public int e_unsheath_Second_hash;
        [HideInInspector] public int e_sheath_Second_hash;

        private int e_armed_FW_turn_left_90_hash = 0;
        private int e_armed_SW_turn_left_90_hash = 0;
        private int e_armed_FW_turn_right_90_hash = 0;
        private int e_armed_SW_turn_right_90_hash = 0;

        private int e_armed_FW_turn_left_inplace_hash = 0;
        private int e_armed_SW_turn_left_inplace_hash = 0;
        private int e_armed_FW_turn_right_inplace_hash = 0;
        private int e_armed_SW_turn_right_inplace_hash = 0;

        // Configuration
        public float switchDistance = 0;
        
        public float switchDistanceBuffer = 0;

        [SerializeField]
        private string secondWeaponId = null;
        
        public AISheathTransform secondSheathTransform = null;             // Parent bone: Hips

        public AI_ActionHolder secondWeaponActionHolder;
        
        // Mod Status
        [ReadOnlyInspector]
        public EnemyRuntimeWeapon secondWeapon;

        [ReadOnlyInspector]
        public ThrowableEnemyRuntimeWeaponPool secondThrowableWeaponPool;

        [ReadOnlyInspector]
        public bool isUsingSecondWeapon;

        [SerializeField, ReadOnlyInspector]
        private bool isSwitchWeaponNeeded;

        [SerializeField, ReadOnlyInspector]
        private bool isFirstWeaponToReacquire;
        
        private bool isSwitchToSecondWeapon = false;

        [NonSerialized]
        AIManager _ai;
        [NonSerialized]
        Animator _anim;

        /// TICK

        public void CheckReacquireThrowableWeapon()
        {
            if (isFirstWeaponToReacquire)
            {
                if (!isSwitchWeaponNeeded && !isUsingSecondWeapon)
                {
                    _ai.Set_ReacquireFirstThrowable_PassiveAction();
                }
                else
                {
                    _ai.ReacquireFirstThrowable();
                }
            }
            else
            {
                if (!isSwitchWeaponNeeded && isUsingSecondWeapon)
                {
                    _ai.Set_ReacquireSecondThrowable_PassiveAction();
                }
                else
                {
                    _ai.ReacquireSecondThrowable();
                }
            }
        }

        public void CheckIsSwitchWeaponNeeded()
        {
            if (isSwitchWeaponNeeded)
            {
                SwitchWeapon();
                return;
            }
            else
            {
                if (_ai.distanceToTarget <= switchDistance - switchDistanceBuffer && !isUsingSecondWeapon)
                {
                    // switch to second action holder
                    isSwitchWeaponNeeded = true;
                    isSwitchToSecondWeapon = true;

                    _ai.Set_SwitchWeaponReady_PassiveAction();
                }
                else if (_ai.distanceToTarget >= switchDistance + switchDistanceBuffer && isUsingSecondWeapon)
                {
                    // switch to first action holder
                    isSwitchWeaponNeeded = true;
                    isSwitchToSecondWeapon = false;

                    _ai.Set_SwitchWeaponReady_PassiveAction();
                }
            }
        }

        public void SetEnemyAnim()
        {
            _anim.SetBool(e_mod_IsUsingSecondWeapon, isUsingSecondWeapon);
        }

        void SwitchWeapon()
        {
            isSwitchWeaponNeeded = false;

            if (isSwitchToSecondWeapon)
            {
                // if enemy's going to switch to second weapon and second weapon isn't null
                if (secondWeapon != null)
                {
                    isUsingSecondWeapon = true;
                    _ai.Set_SwitchToSecondWeapon_PassiveAction();
                }
                // if second weapon is null, reacquire second weapon.
                else
                {
                    CheckReacquireThrowableWeapon();
                }
            }
            else
            {
                // if enemy's going to switch to first weapon and first weapon isn't null
                if (_ai.firstWeapon != null)
                {
                    isUsingSecondWeapon = false;
                    _ai.Set_SwitchToFirstWeapon_PassiveAction();
                }
                // if first weapon is null, reacquire first weapon.
                else
                {
                    CheckReacquireThrowableWeapon();
                }
            }
        }

        /// INIT

        public void DualWeaponModInit(AIManager _ai)
        {
            this._ai = _ai;
            _anim = _ai.anim;

            HashManager hashManager = _ai.hashManager;
            e_mod_IsUsingSecondWeapon = hashManager.e_mod_IsUsingSecondWeapon_hash;

            e_armed_FW_hit_small_r_hash = hashManager.e_armed_FW_hit_small_r_hash;
            e_armed_FW_hit_small_l_hash = hashManager.e_armed_FW_hit_small_l_hash;
            e_armed_FW_hit_small_f_hash = hashManager.e_armed_FW_hit_small_f_hash;

            e_armed_FW_hit_big_r_hash = hashManager.e_armed_FW_hit_big_r_hash;
            e_armed_FW_hit_big_l_hash = hashManager.e_armed_FW_hit_big_l_hash;
            e_armed_FW_hit_big_f_hash = hashManager.e_armed_FW_hit_big_f_hash;

            e_armed_FW_knockDown_hash = hashManager.e_armed_FW_knockDown_hash;
            e_armed_FW_death_hash = hashManager.e_armed_FW_death_hash;

            e_armed_SW_hit_small_r_hash = hashManager.e_armed_SW_hit_small_r_hash;
            e_armed_SW_hit_small_l_hash = hashManager.e_armed_SW_hit_small_l_hash;
            e_armed_SW_hit_small_f_hash = hashManager.e_armed_SW_hit_small_f_hash;

            e_armed_SW_hit_big_r_hash = hashManager.e_armed_SW_hit_big_r_hash;
            e_armed_SW_hit_big_l_hash = hashManager.e_armed_SW_hit_big_l_hash;
            e_armed_SW_hit_big_f_hash = hashManager.e_armed_SW_hit_big_f_hash;

            e_armed_SW_knockDown_hash = hashManager.e_armed_SW_knockDown_hash;
            e_armed_SW_knockDown_HitFromBack_hash = hashManager.e_armed_SW_knockDown_HitFromBack_hash;
            e_armed_SW_death_hash = hashManager.e_armed_SW_death_hash;

            e_sheath_Second_hash = hashManager.e_sheath_Second_hash;
            e_unsheath_Second_hash = hashManager.e_unsheath_Second_hash;

            e_armed_FW_turn_left_90_hash = hashManager.e_armed_FW_turn_left_90_hash;
            e_armed_SW_turn_left_90_hash = hashManager.e_armed_SW_turn_left_90_hash;
            e_armed_FW_turn_right_90_hash = hashManager.e_armed_FW_turn_right_90_hash;
            e_armed_SW_turn_right_90_hash = hashManager.e_armed_SW_turn_right_90_hash;

            e_armed_FW_turn_left_inplace_hash = hashManager.e_armed_FW_turn_left_inplace_hash;
            e_armed_SW_turn_left_inplace_hash = hashManager.e_armed_SW_turn_left_inplace_hash;
            e_armed_FW_turn_right_inplace_hash = hashManager.e_armed_FW_turn_right_inplace_hash;
            e_armed_SW_turn_right_inplace_hash = hashManager.e_armed_SW_turn_right_inplace_hash;
        }

        public void DualWeaponExitAggroReset()
        {
            /// "isUsingSecondWeapon" will be set to false when weapon sheathing.

            isSwitchWeaponNeeded = false;
            _anim.SetBool(e_mod_IsUsingSecondWeapon, false);
        }
        
        /// AI STATE ACTION METHODS

        public string GetSecondWeaponID()
        {
            return secondWeaponId;
        }

        /// GET METHODS

        public bool GetIsFirstWeaponToReacquireBool()
        {
            return isFirstWeaponToReacquire;
        }
        
        /// WEAPON METHODS

        public void ClearThrowableReference()
        {
            if (isUsingSecondWeapon)
            {
                secondWeapon = null;
                isFirstWeaponToReacquire = false;
            }
            else
            {
                _ai.firstWeapon = null;
                isFirstWeaponToReacquire = true;
            }
        }

        /// AI MANAGER - RotateWithRootAnimation
        
        public void RotateWithRootAnimation_TurnLeft()
        {
            if (isUsingSecondWeapon)
            {
                _ai.PlayAnimationCrossFade(e_armed_SW_turn_left_90_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_FW_turn_left_90_hash, 0.2f, true);
            }
        }

        public void RotateWithRootAnimation_TurnRight()
        {
            if (isUsingSecondWeapon)
            {
                _ai.PlayAnimationCrossFade(e_armed_SW_turn_right_90_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_FW_turn_right_90_hash, 0.2f, true);
            }
        }

        /// AI MANAGER - RotateWithInplaceAnimation
        
        public void RotateWithInplaceAnimation_TurnLeft()
        {
            if (isUsingSecondWeapon)
            {
                _ai.PlayAnimationCrossFade(e_armed_SW_turn_left_inplace_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_FW_turn_left_inplace_hash, 0.2f, true);
            }
        }

        public void RotateWithInplaceAnimation_TurnRight()
        {
            if (isUsingSecondWeapon)
            {
                _ai.PlayAnimationCrossFade(e_armed_SW_turn_right_inplace_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_FW_turn_right_inplace_hash, 0.2f, true);
            }
        }

        /// AI MANAGER - PLAY ON HIT ANIM

        public void DualWeapon_PlayRegularOnHitAnim()
        {
            if (isUsingSecondWeapon)
            {
                #region Play Second Weapon Hit Animation.
                switch (_ai._hitSourceAttackRefs._attackActionType)
                {
                    case Player_AttackRefs.AttackActionTypeEnum.Normal:

                        #region Play Second Weapon Small Hit Animation.
                        switch (_ai._hitSourceAttackRefs._attackDirectionType)
                        {
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                _ai.PlayAnimation_NoNeglect(e_armed_SW_hit_small_r_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                _ai.PlayAnimation_NoNeglect(e_armed_SW_hit_small_l_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                _ai.PlayAnimation_NoNeglect(e_armed_SW_hit_small_f_hash, false);
                                break;
                        }
                        #endregion

                        break;

                    case Player_AttackRefs.AttackActionTypeEnum.Hold:

                        if (_ai.playerStates._hasHoldAtkReachedMaximum)
                        {
                            #region Play Second Weapon Big Hit Animation.
                            switch (_ai._hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayFallBackAnim(e_armed_SW_hit_big_r_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayFallBackAnim(e_armed_SW_hit_big_l_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayFallBackAnim(e_armed_SW_hit_big_f_hash);
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Play Second Weapon Small Hit Animation.
                            switch (_ai._hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayAnimation_NoNeglect(e_armed_SW_hit_small_r_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayAnimation_NoNeglect(e_armed_SW_hit_small_l_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayAnimation_NoNeglect(e_armed_SW_hit_small_f_hash, false);
                                    break;
                            }
                            #endregion
                        }
                        break;
                }
                #endregion
            }
            else
            {
                #region Play First Weapon Hit Animation.
                switch (_ai._hitSourceAttackRefs._attackActionType)
                {
                    case Player_AttackRefs.AttackActionTypeEnum.Normal:

                        #region Play First Weapon Small Hit Animation.
                        switch (_ai._hitSourceAttackRefs._attackDirectionType)
                        {
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                _ai.PlayAnimation_NoNeglect(e_armed_FW_hit_small_r_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                _ai.PlayAnimation_NoNeglect(e_armed_FW_hit_small_l_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                _ai.PlayAnimation_NoNeglect(e_armed_FW_hit_small_f_hash, false);
                                break;
                        }
                        #endregion

                        break;

                    case Player_AttackRefs.AttackActionTypeEnum.Hold:

                        if (_ai.playerStates._hasHoldAtkReachedMaximum)
                        {
                            #region Play First Weapon Big Hit Animation.
                            switch (_ai._hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayFallBackAnim(e_armed_FW_hit_big_r_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayFallBackAnim(e_armed_FW_hit_big_l_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayFallBackAnim(e_armed_FW_hit_big_f_hash);
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Play First Weapon Small Hit Animation.
                            switch (_ai._hitSourceAttackRefs._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayAnimation_NoNeglect(e_armed_FW_hit_small_r_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayAnimation_NoNeglect(e_armed_FW_hit_small_l_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayAnimation_NoNeglect(e_armed_FW_hit_small_f_hash, false);
                                    break;
                            }
                            #endregion
                        }
                        break;
                }
                #endregion
            }
        }

        public void DualWeapon_PlayArmedKnockDown_HitFromFront_Anim()
        {
            if (isUsingSecondWeapon)
            {
                _ai.PlaySpecialArmedKnockDownAnim(e_armed_SW_knockDown_hash);
            }
            else
            {
                _ai.PlaySpecialArmedKnockDownAnim(e_armed_FW_knockDown_hash);
            }
        }

        public void DualWeapon_PlayArmedKnockDown_HitFromBack_Anim()
        {
            if (isUsingSecondWeapon)
            {
                _ai.PlaySpecialArmedKnockDownAnim(e_armed_SW_knockDown_HitFromBack_hash);
            }
            else
            {
                _ai.PlaySpecialArmedKnockDownAnim(e_armed_SW_knockDown_HitFromBack_hash);
            }
        }

        public void DualWeapon_PlayOnDeathAnim()
        {
            if (isUsingSecondWeapon)
            {
                _ai.NoSpecificLayer_PlayDeathAnim(e_armed_SW_death_hash);
            }
            else
            {
                _ai.NoSpecificLayer_PlayDeathAnim(e_armed_FW_death_hash);
            }
        }
    }
}