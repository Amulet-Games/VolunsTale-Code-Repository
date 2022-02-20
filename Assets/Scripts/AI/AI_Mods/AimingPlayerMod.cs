using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class AimingPlayerMod
    {
        [HideInInspector]
        public bool showAimingPlayerMod;

        /// Anim Hash.
        [NonSerialized] int e_javelin_isAiming_hash;
        [NonSerialized] int e_aim_quit_hash;
        [NonSerialized] int e_aim_turn_left_45_hash = 0;
        [NonSerialized] int e_aim_turn_right_45_hash = 0;
        [NonSerialized] int e_aim_turn_left_inplace_hash = 0;
        [NonSerialized] int e_aim_turn_right_inplace_hash = 0;
        
        public ThrowAimingProjectile throwAimingProjectile = null;
        
        public WeaponOpposedHandTransform aimingHandPosition;

        [SerializeField]
        [Tooltip("The time enemy takes to throw/shoot the aiming projectile.")]
        private float aimingRate = 0;

        [SerializeField]
        private float aimingQuitDistance = 4;

        [SerializeField, ReadOnlyInspector]
        [Tooltip("Counting when to throw/shoot the aiming projectile while enemy is aiming.")]
        private float aimingTimer = 0;

        [ReadOnlyInspector]
        public bool isAiming = false;

        [NonSerialized]
        AIManager _ai;
        [NonSerialized]
        public float _delta;

        /// TICK

        public void AimingPlayerTimeDistanceCheck()
        {
            if (isAiming)
            {
                aimingTimer += _delta;
                if (aimingTimer > aimingRate)
                {
                    throwAimingProjectile.Execute(_ai);
                }

                if (_ai.distanceToTarget <= aimingQuitDistance)
                {
                    OffAiming();
                    _ai.PlayAnimation(e_aim_quit_hash, true);
                }
            }
        }
        
        /// INIT

        public void AimingPlayerModInit(AIManager _ai)
        {
            this._ai = _ai;

            HashManager hashManager = _ai.hashManager;
            e_javelin_isAiming_hash = hashManager.e_javelin_isAiming_hash;
            e_aim_quit_hash = hashManager.e_aim_quit_hash;
            e_aim_turn_left_45_hash = hashManager.e_aim_turn_left_45_hash;
            e_aim_turn_right_45_hash = hashManager.e_aim_turn_right_45_hash;
            e_aim_turn_left_inplace_hash = hashManager.e_aim_turn_left_inplace_hash;
            e_aim_turn_right_inplace_hash = hashManager.e_aim_turn_right_inplace_hash;
        }

        public void AimingPlayerExitAggroReset()
        {
            OffAiming();
        }
        
        /// ON / OFF Aiming

        public void OnAiming()
        {
            isAiming = true;
            _ai.anim.SetBool(e_javelin_isAiming_hash, true);
            _ai.isPausingTurnWithAgent = false;
        }

        public void OffAiming()
        {
            isAiming = false;
            _ai.anim.SetBool(e_javelin_isAiming_hash, false);
            _ai.skippingScoreCalculation = false;
            aimingTimer = 0;
        }

        /// AI MANAGER - RotateWithRootAnimation

        public void RotateWithRootAnimation_TurnLeft()
        {
            if (isAiming)
            {
                _ai.PlayAnimationCrossFade(e_aim_turn_left_45_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(_ai.e_armed_turn_left_90_hash, 0.2f, true);
            }
        }

        public void RotateWithRootAnimation_TurnRight()
        {
            if (isAiming)
            {
                _ai.PlayAnimationCrossFade(e_aim_turn_right_45_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(_ai.e_armed_turn_right_90_hash, 0.2f, true);
            }
        }

        /// AI MANAGER - RotateWithInplaceAnimation

        public void RotateWithInplaceAnimation_TurnLeft()
        {
            if (isAiming)
            {
                _ai.PlayAnimationCrossFade(e_aim_turn_left_inplace_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(_ai.e_armed_turn_left_inplace_hash, 0.2f, true);
            }
        }

        public void RotateWithInplaceAnimation_TurnRight()
        {
            if (isAiming)
            {
                _ai.PlayAnimationCrossFade(e_aim_turn_right_inplace_hash, 0.2f, true);
            }
            else
            {
                _ai.PlayAnimationCrossFade(_ai.e_armed_turn_right_inplace_hash, 0.2f, true);
            }
        }
    }
}