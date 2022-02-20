using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Weapons/Throwable Enemy Weapon")]
    public class ThrowableEnemyWeapon : EnemyWeapon
    {
        /// When if comes to more than 1 throwable enemy Weapon in your game, change this to pool Id!
        // Pool here!

        public override void SetupThrowableRuntimeWeapon(AIManager ai)
        {
            ai.firstThrowableWeaponPool = ai.aISessionManager._bombRuntimeWeaponPool;
            ai.ReacquireFirstThrowable();
        }
    }
}