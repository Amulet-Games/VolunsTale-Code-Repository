using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "WeaponAction/Parry Action")]
    public class WeaponParryAction : WeaponAttackAction
    {
        ///* There Enemy Attacks are not parryable.
        ///* Roll Attacks.
        ///* Throw Attacks. (Bombs / Javelin /Returnal Projectile)
        ///* Damage Particles Attacks.
        ///* Marksman Kick.
        ///* Shieldman Jump Attack.
        ///* Shieldman Shield Attack.
        
        public override void Execute(StateManager _states)
        {
            SetParryRootMotionStats();
            SetStateStatus();
            DelpeteStateStamina();
            PlayParryAnimation();

            void SetParryRootMotionStats()
            {
                _states.ignoreAttackRootCalculate = ignoreRootCalculate;
                _states.attackRootMotion = actionRootMotion;
            }

            void SetStateStatus()
            {
                _states._isParrying = true;
                _states.applyAttackRootMotion = true;
                _states._currentAttackAction = this;

                _states.OnAttackActionCheckIsRunningStatus();
                _states.OnAttackActionInterruptComment();
            }

            void DelpeteStateStamina()
            {
                _states.statsHandler.DecrementPlayerStaminaWhenAttacking(staminaUsage);
            }

            void PlayParryAnimation()
            {
                if (_states.isLockingOn)
                {
                    _states.CrossFadeAnimWithLockonDir(targetAnimState.animStateHash, true);
                }
                else
                {
                    _states.CrossFadeAnimWithMoveDir(targetAnimState.animStateHash, true, true);
                }
            }
        }
    }
}