using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    ///* Deactivate Effect gameObject in Prefab!

    public class HoldAttackWeaponActionEffect : BaseWeaponActionEffect
    {
        [Header("Effect Fx.")]
        public ParticleSystem _fx;

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

            if (states._isLhHoldLoopEffect)
                mTransform.parent = states.leftHandTransform;
            else
                mTransform.parent = states.rightHandTransform;

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
            _fx.Stop();
        }
    }
}