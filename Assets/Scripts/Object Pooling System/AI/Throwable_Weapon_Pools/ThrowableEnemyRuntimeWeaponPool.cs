using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ThrowableEnemyRuntimeWeaponPool : MonoBehaviour
    {
        public ThrowableEnemyRuntimeWeapon prefab = null;
        
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;
        Queue<ThrowableEnemyRuntimeWeapon> objects = new Queue<ThrowableEnemyRuntimeWeapon>();

        public ThrowableEnemyRuntimeWeapon Get()
        {
            if (_objectsCount == 0)
            {
                return Instantiate(prefab);
            }

            _objectsCount--;
            return objects.Dequeue();
        }
        
        /// Return To Pool.
        public void ReturnToPool(ThrowableEnemyRuntimeWeapon objectToReturn)
        {
            objects.Enqueue(objectToReturn);
            _objectsCount++;
        }
    }
}