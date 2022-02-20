using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WeaponAbsorbFxOnStopHandler : AbsorbFxOnStopHandler
    {
        public override void OnParticleSystemStopped()
        {
            _runtimeAmulet.OnWeaponAbsorb_BlinkVolunAmulet();
            gameObject.SetActive(false);
        }
    }
}