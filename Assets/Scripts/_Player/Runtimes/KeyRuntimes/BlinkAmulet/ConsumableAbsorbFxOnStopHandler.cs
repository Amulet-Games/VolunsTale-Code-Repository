using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    //ConsumableAbsorbFxOnStopHandler
    public class ConsumableAbsorbFxOnStopHandler : AbsorbFxOnStopHandler
    {
        public override void OnParticleSystemStopped()
        {
            _runtimeAmulet.OnConsumableAbsorb_BlinkVolunAmulet();
            gameObject.SetActive(false);
        }
    }
}