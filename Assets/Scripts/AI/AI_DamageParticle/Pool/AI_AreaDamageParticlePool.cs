using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_AreaDamageParticlePool : MonoBehaviour
    {
        [SerializeField]
        private AI_Poolable_AreaDamageParticle prefab = null;

        [SerializeField]
        private Queue<AI_Poolable_AreaDamageParticle> poolable_areaDps = new Queue<AI_Poolable_AreaDamageParticle>();
        [ReadOnlyInspector, SerializeField] int _objectsCount = 0;

        public AI_Poolable_AreaDamageParticle Get()
        {
            if (_objectsCount == 0)
            {
                AI_Poolable_AreaDamageParticle new_poolable_areaDp = Instantiate(prefab);
                new_poolable_areaDp.PoolableInit(this);
                return new_poolable_areaDp;
            }

            _objectsCount--;
            return poolable_areaDps.Dequeue();
        }

        public void PreWarm()
        {
            AI_Poolable_AreaDamageParticle new_poolable_areaDp = Instantiate(prefab);
            new_poolable_areaDp.PoolableInit(this);

            ReturnToPool(new_poolable_areaDp);
        }

        /// Return To Pool.
        public void ReturnToPool(AI_Poolable_AreaDamageParticle return_poolable_areaDp)
        {
            poolable_areaDps.Enqueue(return_poolable_areaDp);
            _objectsCount++;

            return_poolable_areaDp.ReturnToBackpack_AfterPool();
        }
    }
}