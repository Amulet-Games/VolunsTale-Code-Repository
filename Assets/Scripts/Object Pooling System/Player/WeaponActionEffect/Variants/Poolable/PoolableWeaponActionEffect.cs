using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    ///* Deactivate Effect gameObject in Prefab!

    public class PoolableWeaponActionEffect : MonoBehaviour
    {
        [Header("Refs")]
        [ReadOnlyInspector] public PoolableWeaponActionEffect_Pool referedPool;
        Transform mTransform;

        /// Callback.
        public void OnParticleSystemStopped()
        {
            Return();
            referedPool.ReturnToPool(this);

            void Return()
            {
                gameObject.SetActive(false);
                mTransform.parent = referedPool.actionEffectBackpack;
            }
        }

        public void PlayEffect(WA_Effect_Profile _profile)
        {
            gameObject.SetActive(true);

            mTransform.parent = referedPool.states.mTransform;
            mTransform.localPosition = _profile.localPos;
            mTransform.localEulerAngles = _profile.localEulers;
        }

        /// Setup.
        public void Setup(PoolableWeaponActionEffect_Pool _referedPool)
        {
            referedPool = _referedPool;
            mTransform = transform;
            mTransform.parent = referedPool.actionEffectBackpack;
        }
    }
}