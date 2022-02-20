using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    ///* Deactivate Effect gameObject in Prefab!

    public class SingleWeaponActionEffect : BaseWeaponActionEffect
    {
        Transform mTransform;

        /// Callback.
        public void OnParticleSystemStopped()
        {
            Return();

            void Return()
            {
                gameObject.SetActive(false);
                mTransform.parent = actionEffectBackpack;
            }
        }

        /// Overrides.
        public override void PlayEffect(WA_Effect_Profile _profile)
        {
            gameObject.SetActive(true);

            mTransform.parent = states.mTransform;
            mTransform.localPosition = _profile.localPos;
            mTransform.localEulerAngles = _profile.localEulers;
        }
        
        public override void Setup(StateManager _states, Transform _actionEffectBackpack)
        {
            states = _states;
            actionEffectBackpack = _actionEffectBackpack;
            mTransform = transform;
        }

        public override void StopEffect()
        {
            Debug.LogError("Effect is not stoppable!");
        }
    }
}