using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WorldImpactEffectPool : MonoBehaviour
    {
        [SerializeField]
        private WorldImpactEffect prefab = null;

        Queue<WorldImpactEffect> objects = new Queue<WorldImpactEffect>();
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;

        public WorldImpactEffect Get()
        {
            if (_objectsCount == 0)
            {
                WorldImpactEffect newEffect = Instantiate(prefab);
                newEffect.Setup();
                return newEffect;
            }

            _objectsCount--;
            return objects.Dequeue();
        }

        public void PreWarm()
        {
            WorldImpactEffect newEffect = Instantiate(prefab);
            newEffect.Setup();

            ReturnToPool(newEffect);
        }

        /// Return To Pool.
        public void ReturnToPool(WorldImpactEffect objectToReturn)
        {
            objects.Enqueue(objectToReturn);
            _objectsCount++;

            objectToReturn.ReturnToBackpack();
        }
    }
}