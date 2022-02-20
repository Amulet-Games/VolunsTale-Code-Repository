using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PoolableWeaponActionEffect_Pool : BaseWeaponActionEffect
    {
        [Header("Effect.")]
        [SerializeField] PoolableWeaponActionEffect prefab = null;
        
        private Queue<PoolableWeaponActionEffect> effects = new Queue<PoolableWeaponActionEffect>();
        [ReadOnlyInspector, SerializeField] int _effectsCount = 0;

        public PoolableWeaponActionEffect GetEffectFromPool()
        {
            if (_effectsCount == 0)
            {
                PoolableWeaponActionEffect newObject = Instantiate(prefab);
                newObject.Setup(this);
                return newObject;
            }

            _effectsCount--;
            return effects.Dequeue();
        }

        /// Callback.
        public void ReturnToPool(PoolableWeaponActionEffect objectToReturn)
        {
            effects.Enqueue(objectToReturn);
            _effectsCount++;
        }

        /// Overrides.
        public override void PlayEffect(WA_Effect_Profile _profile)
        {
            GetEffectFromPool().PlayEffect(_profile);
        }

        #region Setup.
        public override void Setup(StateManager _states, Transform _actionEffectBackpack)
        {
            states = _states;
            actionEffectBackpack = _actionEffectBackpack;

            Prewarm();
        }

        public void Prewarm()
        {
            PoolableWeaponActionEffect newObject = Instantiate(prefab);
            newObject.Setup(this);

            ReturnToPool(newObject);
        }
        #endregion

        public override void StopEffect()
        {
            Debug.LogError("Effect is not stoppable!");
        }
    }
}