using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BaseWeaponActionEffect : MonoBehaviour
    {
        [Header("Effect Marker.")]
        public int id;

        [Header("Base Refs.")]
        [ReadOnlyInspector] public Transform actionEffectBackpack;
        [ReadOnlyInspector] public StateManager states;

        public abstract void Setup(StateManager _states, Transform _actionEffectBackpack);
        
        public abstract void PlayEffect(WA_Effect_Profile _profile);

        public abstract void StopEffect();
    }
}