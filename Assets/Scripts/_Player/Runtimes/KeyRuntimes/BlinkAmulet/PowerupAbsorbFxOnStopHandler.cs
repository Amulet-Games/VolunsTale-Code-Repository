using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PowerupAbsorbFxOnStopHandler : AbsorbFxOnStopHandler
    {
        public override void OnParticleSystemStopped()
        {
            _runtimeAmulet.OnPowerupPreviewEnded_BlinkVolunAmulet();
            gameObject.SetActive(false);
        }
    }
}