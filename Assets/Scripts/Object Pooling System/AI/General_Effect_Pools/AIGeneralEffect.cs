using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    ///* GameObject needs to be DEACTIVATED in Prefab.

    public abstract class AIGeneralEffect : MonoBehaviour
    {
        [Header("Effect (Drops).")]
        public ParticleSystem aiGeneralFx;

        [Header("Refs.")]
        [ReadOnlyInspector] public AISessionManager aiSessionManager;
        [ReadOnlyInspector] public Transform mTransform;

        /// Callback.
        public void OnParticleSystemStopped()
        {
            ReturnEffectToPool();
        }

        /// Return To Pool.
        public void ReturnToBackpack()
        {
            gameObject.SetActive(false);
            mTransform.parent = aiSessionManager._ai_generalFx_Backpack;
        }

        /// Setup.
        public void Setup()
        {
            SetupRefs();

            void SetupRefs()
            {
                aiSessionManager = AISessionManager.singleton;
                mTransform = transform;
            }
        }

        /// Abstract.
        public abstract void ReturnEffectToPool();
    }
}