using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "WeaponAction/Attack Action/Inverse Move Dir Normal Attack")]
    public class InverseMoveDirNormalAttackAction : WeaponAttackAction
    {
        [Header("WA Effect.")]
        public NormalAttack_Effect_Stack _effectStack;

        public override void Execute(StateManager _states)
        {
            SetAttackRootMotionStats();
            SetAttackPowerMultipier();
            SetStateStatus();
            DelpeteStateStamina();
            HandleAttackAnimaion();

            void SetAttackRootMotionStats()
            {
                _states.ignoreAttackRootCalculate = ignoreRootCalculate;
                _states.attackRootMotion = actionRootMotion;
            }

            void SetAttackPowerMultipier()
            {
                _states.statsHandler._attPowMulti_weaponAction = 1;
            }

            void SetStateStatus()
            {
                _states._isAttacking = true;
                _states.applyAttackRootMotion = true;
                _states._currentAttackAction = this;

                _states.SetIsTwoHandFistAttacking();
                _states.OnAttackActionCheckIsRunningStatus();
                _states.OnAttackActionInterruptComment();
            }

            void DelpeteStateStamina()
            {
                _states.statsHandler.DecrementPlayerStaminaWhenAttacking(staminaUsage);
            }

            void HandleAttackAnimaion()
            {
                if (_states.isLockingOn)
                {
                    if (!isTurnWithMoveDirWhenLockon)
                    {
                        _states.PlayLockonDirAttackAnimation(targetAnimState.animStateHash);
                    }
                    else
                    {
                        _states.PlayInverseMoveDirAttackAnimation(targetAnimState.animStateHash);
                    }
                }
                else
                {
                    _states.PlayInverseMoveDirAttackAnimation(targetAnimState.animStateHash);
                }
            }
        }

        #region Play WA Effects.
        public override void Play_1st_Effect()
        {
            GameManager.singleton.Play_WA_Effect(_effectStack._1st_effect_profile);
        }

        public override void Play_2nd_Effect()
        {
            GameManager.singleton.Play_WA_Effect(_effectStack._2nd_effect_profile);
        }

        public override void Play_3rd_Effect()
        {
            GameManager.singleton.Play_WA_Effect(_effectStack._3rd_effect_profile);
        }

        public override void Play_4th_Effect()
        {
            GameManager.singleton.Play_WA_Effect(_effectStack._4th_effect_profile);
        }
        #endregion
    }
}
