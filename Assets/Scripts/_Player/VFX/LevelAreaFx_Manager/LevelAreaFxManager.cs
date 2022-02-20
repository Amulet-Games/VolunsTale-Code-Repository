using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LevelAreaFxManager : MonoBehaviour
    {
        public ParticleSystem _defaultSnowFx;
        public ParticleSystem _egil_2ndPhase_SnowFx;
        public ParticleSystem _egil_3rdPhase_SnowFx;
        public ParticleSystem _egil_3rdPhase_CloudFx;

        public static LevelAreaFxManager singleton;

        private void Awake()
        {
            if (singleton == null)
                singleton = this;
            else
                Destroy(this);
        }

        public void PlayDefaultSnowFx()
        {
            _egil_3rdPhase_SnowFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _egil_3rdPhase_CloudFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            _defaultSnowFx.gameObject.SetActive(true);
            _defaultSnowFx.Play();
        }

        public void PlayEgil2ndPhaseSnowFx()
        {
            _defaultSnowFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            _egil_2ndPhase_SnowFx.gameObject.SetActive(true);
        }

        public void PlayEgil3rdPhaseSnowFx()
        {
            _egil_2ndPhase_SnowFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            _egil_3rdPhase_SnowFx.gameObject.SetActive(true);
            _egil_3rdPhase_CloudFx.gameObject.SetActive(true);
        }

        public void BossFightEnded_ReverseSnowFx()
        {
            if (_egil_3rdPhase_SnowFx.isPlaying)
            {
                _egil_3rdPhase_SnowFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _egil_3rdPhase_CloudFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            else if (_egil_2ndPhase_SnowFx.isPlaying)
            {
                _egil_2ndPhase_SnowFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }

            _defaultSnowFx.gameObject.SetActive(true);
            _defaultSnowFx.Play();
        }
    }
}