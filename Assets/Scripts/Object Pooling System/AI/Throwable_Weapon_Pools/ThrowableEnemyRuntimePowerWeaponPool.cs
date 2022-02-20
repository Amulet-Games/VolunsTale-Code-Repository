using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ThrowableEnemyRuntimePowerWeaponPool : MonoBehaviour
    {
        [SerializeField]
        private ThrowableEnemyRuntimePowerWeapon prefab = null;

        [SerializeField]
        private Queue<ThrowableEnemyRuntimePowerWeapon> objects = new Queue<ThrowableEnemyRuntimePowerWeapon>();
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;

        public ThrowableEnemyRuntimePowerWeapon Get()
        {
            if (_objectsCount == 0)
            {
                return Instantiate(prefab);
            }

            _objectsCount--;
            return objects.Dequeue();
        }

        /// Return To Pool.
        public void ReturnToPool(ThrowableEnemyRuntimePowerWeapon objectToReturn)
        {
            objects.Enqueue(objectToReturn);
            _objectsCount++;
        }
    }
}
