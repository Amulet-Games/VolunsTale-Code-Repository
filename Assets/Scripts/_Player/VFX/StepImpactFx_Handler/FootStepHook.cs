using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FootStepHook : MonoBehaviour
    {
        [Header("Drag and Drop Refs.")]
        public StepImpactFxHandler _foot_l_footStepParticle;
        public StepImpactFxHandler _foot_r_footStepParticle;

        [Header("Status.")]
        [ReadOnlyInspector] public bool _isleftFootEffectPlayed;
        
        public void ActivateFootStepParticle()
        {
            if (_isleftFootEffectPlayed)
            {
                _foot_r_footStepParticle.PlayStepImpact();
                _isleftFootEffectPlayed = false;
            }
            else
            {
                _foot_l_footStepParticle.PlayStepImpact();
                _isleftFootEffectPlayed = true;
            }
        }

        public void Play_L_FootStep_Immediate()
        {
            _foot_l_footStepParticle.PlayStepImpact();
        }

        public void Play_R_FootStep_Immediate()
        {
            _foot_r_footStepParticle.PlayStepImpact();
        }

        public void Play_Both_FootStep_Immediate()
        {
            _foot_l_footStepParticle.PlayStepImpact();
            _foot_r_footStepParticle.PlayStepImpact();
        }
    }
}