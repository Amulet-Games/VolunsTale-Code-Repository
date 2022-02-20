using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BossArenaSnowFXOnStopHandler : MonoBehaviour
    {
        public void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
        }
    }
}