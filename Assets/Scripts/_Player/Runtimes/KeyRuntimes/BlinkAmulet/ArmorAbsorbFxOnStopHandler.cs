using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ArmorAbsorbFxOnStopHandler : AbsorbFxOnStopHandler
    {
        public override void OnParticleSystemStopped()
        {
            _runtimeAmulet.OnArmorPreviewEnded_BlinkVolunAmulet();
            gameObject.SetActive(false);
        }
    }
}