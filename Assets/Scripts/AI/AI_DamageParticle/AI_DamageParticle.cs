using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_DamageParticle : MonoBehaviour
    {
        [Header("Drag and Drops.")]
        public ParticleSystem _particleSystem;

        [Header("Refs.")]
        [ReadOnlyInspector] public StateManager _playerStates;
        [ReadOnlyInspector] public AISessionManager _aiSessionManager;
        [ReadOnlyInspector] public AIManager ai;

        public abstract void Init();

        public abstract void Tick();

        #region On Damage Particle Effect.
        public abstract void OnDamageParticleEffect();
        
        protected void AddToActiveDpList()
        {
            _aiSessionManager.AddDamageParticleToActiveList(this);
        }
        #endregion

        #region Off Damage Particle Effect.
        protected void RemoveFromActiveDpList()
        {
            _aiSessionManager.RemoveDamageParticleToActiveList(this);
        }
        #endregion

        #region Init.
        protected void Base_InitReference()
        {
            _aiSessionManager = AISessionManager.singleton;
            _playerStates = _aiSessionManager._playerState;

            transform.parent = _aiSessionManager._ai_dpHub_Backpack;

            _particleSystem = GetComponent<ParticleSystem>();
        }
        #endregion

        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawWireSphere(transform.position, _affectRadius);
        //}
    }
}