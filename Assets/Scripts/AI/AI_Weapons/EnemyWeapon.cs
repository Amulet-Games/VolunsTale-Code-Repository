using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class EnemyWeapon : ScriptableObject
    {
        public virtual EnemyRuntimeWeapon SetupRuntimeWeapon(AIManager ai)
        {
            return null;
        }

        public virtual void SetupThrowableRuntimeWeapon(AIManager ai)
        {
        }
    }
}