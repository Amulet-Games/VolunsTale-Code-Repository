using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BaseEnemyWeaponHook : MonoBehaviour
    {
        [ReadOnlyInspector] public AIManager ai;
        
        public abstract void SetColliderStatusToTrue();

        public abstract void SetColliderStatusToFalse();
    }
}