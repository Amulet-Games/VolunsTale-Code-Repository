using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_Poolable_AreaDamageParticle : AI_AreaDamageParticle_Base
    {
        [Header("Refered Pool.")]
        [ReadOnlyInspector] public AI_AreaDamageParticlePool _referedPool;

        #region Callback Methods.
        public void OnParticleSystemStopped()
        {
            Off_Poolable_AreaDp();
        }
        #endregion

        #region Tick.
        public override void Tick()
        {
            if (_hasHurtPlayer)
                return;

            HandleOverlapSphere();
        }
        #endregion

        #region Off Area Damage Effect.
        void Off_Poolable_AreaDp()
        {
            // Dp Base.
            RemoveFromActiveDpList();

            // Area Dp Base.
            Off_AreaDp_ResetStatus();

            ReturnDpToPool();
        }

        void ReturnDpToPool()
        {
            _referedPool.ReturnToPool(this);
        }

        public void ReturnToBackpack_AfterPool()
        {
            transform.parent = _aiSessionManager._ai_dpHub_Backpack;
            gameObject.SetActive(false);
        }
        #endregion

        #region Init.
        public override void Init()
        {
            throw new System.NotImplementedException();
        }

        public void PoolableInit(AI_AreaDamageParticlePool _pool)
        {
            _referedPool = _pool;

            AreaDamageParticle_Init();
        }
        #endregion
    }
}