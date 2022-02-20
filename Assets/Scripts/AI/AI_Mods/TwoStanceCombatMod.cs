using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace SA
{
    [Serializable]
    public class TwoStanceCombatMod : AIMod
    {
        [HideInInspector]
        public bool showTwoStanceCombatMod;

        #region Anim_Hash.
        [HideInInspector] private int e_mod_IsRightStance_hash;

        [HideInInspector] public int e_armed_RS_hit_small_r_hash;
        [HideInInspector] public int e_armed_RS_hit_small_l_hash;
        [HideInInspector] public int e_armed_RS_hit_small_f_hash;

        [HideInInspector] public int e_armed_RS_hit_big_r_hash;
        [HideInInspector] public int e_armed_RS_hit_big_l_hash;
        [HideInInspector] public int e_armed_RS_hit_big_f_hash;

        [HideInInspector] public int e_armed_RS_knockDown_hash;
        [HideInInspector] public int e_armed_RS_death_hash;

        [HideInInspector] public int e_armed_LS_hit_small_r_hash;
        [HideInInspector] public int e_armed_LS_hit_small_l_hash;
        [HideInInspector] public int e_armed_LS_hit_small_f_hash;

        [HideInInspector] public int e_armed_LS_hit_big_r_hash;
        [HideInInspector] public int e_armed_LS_hit_big_l_hash;
        [HideInInspector] public int e_armed_LS_hit_big_f_hash;

        [HideInInspector] public int e_armed_LS_knockDown_hash;
        [HideInInspector] public int e_armed_LS_death_hash;

        private int e_armed_RS_turn_left_90_hash = 0;
        private int e_armed_LS_turn_left_90_hash = 0;
        private int e_armed_RS_turn_right_90_hash = 0;
        private int e_armed_LS_turn_right_90_hash = 0;

        private int e_armed_RS_turn_left_inplace_hash = 0;
        private int e_armed_LS_turn_left_inplace_hash = 0;
        private int e_armed_RS_turn_right_inplace_hash = 0;
        private int e_armed_LS_turn_right_inplace_hash = 0;
        #endregion

        public float _changeStanceMaxRate = 7.5f;
        public float _changeStanceMinRate = 3f;
        [ReadOnlyInspector] public float _changeStanceTimer;
        [ReadOnlyInspector] public float _finalizedChangeStanceRate;

        [ReadOnlyInspector]
        [Tooltip("This shows if enemy currently is in right attack stance.")]
        public bool isRightStance;

        [NonSerialized] AIManager _ai;
        [NonSerialized] public float _delta;

        /// INIT

        public void TwoStanceCombatModInit(AIManager _ai)
        {
            this._ai = _ai;

            HashManager hashManager = _ai.hashManager;
            e_mod_IsRightStance_hash = hashManager.e_mod_IsRightStance_hash;

            e_armed_RS_hit_small_r_hash = hashManager.e_armed_RS_hit_small_r_hash;
            e_armed_RS_hit_small_l_hash = hashManager.e_armed_RS_hit_small_l_hash;
            e_armed_RS_hit_small_f_hash = hashManager.e_armed_RS_hit_small_f_hash;

            e_armed_RS_hit_big_r_hash = hashManager.e_armed_RS_hit_big_r_hash;
            e_armed_RS_hit_big_l_hash = hashManager.e_armed_RS_hit_big_l_hash;
            e_armed_RS_hit_big_f_hash = hashManager.e_armed_RS_hit_big_f_hash;

            e_armed_RS_knockDown_hash = hashManager.e_armed_RS_knockDown_hash;
            e_armed_RS_death_hash = hashManager.e_armed_RS_death_hash;

            e_armed_LS_hit_small_r_hash = hashManager.e_armed_LS_hit_small_r_hash;
            e_armed_LS_hit_small_l_hash = hashManager.e_armed_LS_hit_small_l_hash;
            e_armed_LS_hit_small_f_hash = hashManager.e_armed_LS_hit_small_f_hash;

            e_armed_LS_hit_big_r_hash = hashManager.e_armed_LS_hit_big_r_hash;
            e_armed_LS_hit_big_l_hash = hashManager.e_armed_LS_hit_big_l_hash;
            e_armed_LS_hit_big_f_hash = hashManager.e_armed_LS_hit_big_f_hash;

            e_armed_LS_knockDown_hash = hashManager.e_armed_LS_knockDown_hash;
            e_armed_LS_death_hash = hashManager.e_armed_LS_death_hash;

            e_armed_RS_turn_left_90_hash = hashManager.e_armed_RS_turn_left_90_hash;
            e_armed_LS_turn_left_90_hash = hashManager.e_armed_LS_turn_left_90_hash;
            e_armed_RS_turn_right_90_hash = hashManager.e_armed_RS_turn_right_90_hash;
            e_armed_LS_turn_right_90_hash = hashManager.e_armed_LS_turn_right_90_hash;

            e_armed_RS_turn_left_inplace_hash = hashManager.e_armed_RS_turn_left_inplace_hash;
            e_armed_LS_turn_left_inplace_hash = hashManager.e_armed_LS_turn_left_inplace_hash;
            e_armed_RS_turn_right_inplace_hash = hashManager.e_armed_RS_turn_right_inplace_hash;
            e_armed_LS_turn_right_inplace_hash = hashManager.e_armed_LS_turn_right_inplace_hash;

            isRightStance = true;
            _ai.anim.SetBool(e_mod_IsRightStance_hash, true);
        }

        public void TwoStanceCombatModEnterAggroReset()
        {
            RandomizeWithSpecificRange();
        }

        public void TwoStanceCombatExitAggroReset()
        {
            SetIsRightStanceBool(true);
        }

        /// TICK

        public void MonitorChangeStanceTimer()
        {
            _changeStanceTimer += _delta;
            if (_changeStanceTimer > _finalizedChangeStanceRate)
            {
                _changeStanceTimer = 0;
                SetIsRightStanceBool(!isRightStance);
                RandomizeWithSpecificRange();
            }
        }

        /// SET METHODS

        public void SetIsRightStanceBool(bool isRightStance)
        {
            this.isRightStance = isRightStance;
            _ai.anim.SetBool(e_mod_IsRightStance_hash, isRightStance);
        }
        
        /// AI MANAGER - RotateWithRootAnimation
        
        public void RotateWithRootAnimation_TurnLeft()
        {
            if (isRightStance)
            {
                _ai.PlayAnimationCrossFade(e_armed_RS_turn_left_90_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_LS_turn_left_90_hash, 0.2f, true);
            }
        }

        public void RotateWithRootAnimation_TurnRight()
        {
            if (isRightStance)
            {
                _ai.PlayAnimationCrossFade(e_armed_RS_turn_right_90_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_LS_turn_right_90_hash, 0.2f, true);
            }
        }

        /// AI MANAGER - RotateWithInplaceAnimation

        public void RotateWithInplaceAnimation_TurnLeft()
        {
            if (isRightStance)
            {
                _ai.PlayAnimationCrossFade(e_armed_RS_turn_left_inplace_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_LS_turn_left_inplace_hash, 0.2f, true);
            }
        }

        public void RotateWithInplaceAnimation_TurnRight()
        {
            if (isRightStance)
            {
                _ai.PlayAnimationCrossFade(e_armed_RS_turn_right_inplace_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(e_armed_LS_turn_right_inplace_hash, 0.2f, true);
            }
        }

        /// AI MANAGER - On Hit
        
        public void TwoStanceMod_OnHitAnim()
        {
            Player_AttackRefs _currentGetHitAttackAction = _ai._hitSourceAttackRefs;

            if (isRightStance)
            {
                #region Play Right Stance Animation States.
                switch (_currentGetHitAttackAction._attackActionType)
                {
                    case Player_AttackRefs.AttackActionTypeEnum.Normal:

                        #region Play Armed RS Small Hit Animation.
                        switch (_currentGetHitAttackAction._attackDirectionType)
                        {
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                _ai.PlayAnimation_NoNeglect(e_armed_RS_hit_small_r_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                _ai.PlayAnimation_NoNeglect(e_armed_RS_hit_small_l_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                _ai.PlayAnimation_NoNeglect(e_armed_RS_hit_small_f_hash, false);
                                break;
                        }
                        #endregion

                        break;

                    case Player_AttackRefs.AttackActionTypeEnum.Hold:

                        if (_ai.playerStates._hasHoldAtkReachedMaximum)
                        {
                            #region Play Armed RS Big Hit Animation.
                            switch (_currentGetHitAttackAction._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayFallBackAnim(e_armed_RS_hit_big_r_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayFallBackAnim(e_armed_RS_hit_big_l_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayFallBackAnim(e_armed_RS_hit_big_f_hash);
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Play Armed RS Small Hit Animation.
                            switch (_currentGetHitAttackAction._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayAnimation_NoNeglect(e_armed_RS_hit_small_r_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayAnimation_NoNeglect(e_armed_RS_hit_small_l_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayAnimation_NoNeglect(e_armed_RS_hit_small_f_hash, false);
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
                #region Play Left Stance Animation States.
                switch (_currentGetHitAttackAction._attackActionType)
                {
                    case Player_AttackRefs.AttackActionTypeEnum.Normal:

                        #region Play Armed LS Small Hit Animation.
                        switch (_currentGetHitAttackAction._attackDirectionType)
                        {
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                _ai.PlayAnimation_NoNeglect(e_armed_LS_hit_small_r_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                _ai.PlayAnimation_NoNeglect(e_armed_LS_hit_small_l_hash, false);
                                break;
                            case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                _ai.PlayAnimation_NoNeglect(e_armed_LS_hit_small_f_hash, false);
                                break;
                        }
                        #endregion

                        break;

                    case Player_AttackRefs.AttackActionTypeEnum.Hold:

                        if (_ai.playerStates._hasHoldAtkReachedMaximum)
                        {
                            #region Play Armed LS Big Hit Animation.
                            switch (_currentGetHitAttackAction._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayFallBackAnim(e_armed_LS_hit_big_r_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayFallBackAnim(e_armed_LS_hit_big_l_hash);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayFallBackAnim(e_armed_LS_hit_big_f_hash);
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Play Armed LS Small Hit Animation.
                            switch (_currentGetHitAttackAction._attackDirectionType)
                            {
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromLeft:
                                    _ai.PlayAnimation_NoNeglect(e_armed_LS_hit_small_r_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromRight:
                                    _ai.PlayAnimation_NoNeglect(e_armed_LS_hit_small_l_hash, false);
                                    break;
                                case Player_AttackRefs.AttackDirectionTypeEnum.HitFromFront:
                                    _ai.PlayAnimation_NoNeglect(e_armed_LS_hit_small_f_hash, false);
                                    break;
                            }
                            #endregion
                        }
                        break;
                }
                #endregion
            }
        }

        public void TwoStanceMod_PlayArmedKnockDownAnim()
        {
            if (isRightStance)
            {
                _ai.PlaySpecialArmedKnockDownAnim(e_armed_RS_knockDown_hash);
            }
            else
            {
                _ai.PlaySpecialArmedKnockDownAnim(e_armed_LS_knockDown_hash);
            }
        }

        public void PlayTwoStanceOnDeathAniation()
        {
            if (isRightStance)
            {
                _ai.NoSpecificLayer_PlayDeathAnim(e_armed_RS_death_hash);
            }
            else
            {
                _ai.NoSpecificLayer_PlayDeathAnim(e_armed_LS_death_hash);
            }
        }

        /// RANDOMIZE.

        public void RandomizeWithSpecificRange()
        {
            _finalizedChangeStanceRate = Random.Range(_changeStanceMinRate, _changeStanceMaxRate);
        }
    }
}