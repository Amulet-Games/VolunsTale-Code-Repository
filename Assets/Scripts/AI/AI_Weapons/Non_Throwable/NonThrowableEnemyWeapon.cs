using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Weapons/NonThrowable Enemy Weapon")]
    public class NonThrowableEnemyWeapon : EnemyWeapon
    {
        [Header("Weapon Prefab")]
        public NonThrowableEnemyRuntimeWeapon modelPrefab;

        [Header("Weapon Sidearm")]
        public EnemySidearm linkedEnemySidearm;

        public override EnemyRuntimeWeapon SetupRuntimeWeapon(AIManager ai)
        {
            NonThrowableEnemyRuntimeWeapon runtimeWeapon = Instantiate(modelPrefab);
            runtimeWeapon._ai = ai;
            
            runtimeWeapon.SetupNonThrowableRuntimeWeapon(this);

            if (linkedEnemySidearm)
            {
                runtimeWeapon.SetupLinkedRuntimeSideArm(linkedEnemySidearm);
            }

            return runtimeWeapon;
        }
    }
}