using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class TimedWeaponBuffAction : WeaponBuffAction
    {
        [Header("Config.")]
        public float durationRate = 0;
        
        [NonSerialized] StateManager _states;

        public override void Execute(StateManager _states)
        {
            this._states = _states;

            _states.applyAttackRootMotion = false;

            /// Play Animation.
            PlayWeaponBuffAnimation();

            /// Play VFX
            //weaponBuffEffect.Get();

            /// Register Weapon Buff Job.
            _states.statsHandler.RegisterNewWeaponBuffJob(this);
        }

        public abstract void ExecuteWeaponBuffEffect(StatsAttributeHandler _statsHandler);

        public abstract void OnCompleteReverseEffect(StatsAttributeHandler _statsHandler);

        void PlayWeaponBuffAnimation()
        {
            _states.CrossFadeAnimWithMoveDir(targetAnimState.animStateHash, false, true);
        }
    }
}