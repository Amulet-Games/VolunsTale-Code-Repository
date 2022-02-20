using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GenericObjectPoolWithQueue<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private T prefab = null;

        [SerializeField]
        private Queue<T> objects = new Queue<T>();

        public T Get()
        {
            if (objects.Count < 1)
            {
                return Instantiate(prefab);
            }
            else
            {
                return objects.Dequeue();
            }
        }

        public void PreWarm()
        {
            ReturnToPool(Instantiate(prefab));
        }
        
        public void ReturnToPool(T objectToReturn)
        {
            objects.Enqueue(objectToReturn);
        }
    }
}