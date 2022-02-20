using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_Poolable_BFX_Handler : AI_BFX_HandlerBase
    {
        [Header("Pool")]
        [ReadOnlyInspector] public AI_BFX_HandlerPool _referedPool;
        
        public abstract void Setup(AI_BFX_HandlerPool _pool);

        public abstract void Start_AI_Bfx();
        
        protected void SetupRefs()
        {
            aiSessionManager = AISessionManager.singleton;
        }

        protected void ReturnToPool()
        {
            _referedPool.ReturnToPool(this);
        }
    }
}