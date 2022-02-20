using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RingAbsorbFxOnStopHandler : AbsorbFxOnStopHandler
    {
        public override void OnParticleSystemStopped()
        {
            _runtimeAmulet.OnRingAbsorbEnded_BlinkVolunAmulet();
            gameObject.SetActive(false);
        }
    }
}