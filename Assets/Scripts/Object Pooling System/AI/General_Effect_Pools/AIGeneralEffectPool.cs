using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{ 
    public class AIGeneralEffectPool<T> : MonoBehaviour where T : AIGeneralEffect
    {
        [SerializeField]
        private T prefab = null;

        Queue<T> objects = new Queue<T>();
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;

        public T Get()
        {
            if (_objectsCount == 0)
            {
                T newEffect = Instantiate(prefab);
                newEffect.Setup();
                return newEffect;
            }

            _objectsCount--;
            return objects.Dequeue();
        }

        public void PreWarm()
        {
            T newEffect = Instantiate(prefab);
            newEffect.Setup();

            ReturnToPool(newEffect);
        }

        /// Return To Pool.
        public void ReturnToPool(T objectToReturn)
        {
            objects.Enqueue(objectToReturn);
            _objectsCount++;

            objectToReturn.ReturnToBackpack();
        }
    }
}
