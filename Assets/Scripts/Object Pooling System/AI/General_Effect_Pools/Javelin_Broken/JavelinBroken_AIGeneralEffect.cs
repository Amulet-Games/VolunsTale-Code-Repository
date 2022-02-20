using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class JavelinBroken_AIGeneralEffect : AIGeneralEffect
    {
        /// Spawn.
        public void SpawnEffect_AfterThrowableHitPlayer(Transform _transform)
        {
            mTransform.parent = null;
            mTransform.transform.position = _transform.position + _transform.forward * 2f;
            mTransform.transform.eulerAngles = _transform.eulerAngles;

            gameObject.SetActive(true);
            aiGeneralFx.Play();
        }

        public void SpawnEffect_AfterDuabilityEmpty(Transform _transform)
        {
            mTransform.parent = null;
            mTransform.position = _transform.position;
            mTransform.eulerAngles = _transform.eulerAngles;

            gameObject.SetActive(true);
            aiGeneralFx.Play();
        }

        /// Return.
        public override void ReturnEffectToPool()
        {
            aiSessionManager._javelinBroken_pool.ReturnToPool(this);
        }
    }
}