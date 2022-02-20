using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AbsorbFxOnStopHandler : MonoBehaviour
    {
        public RuntimeVolunAmulet _runtimeAmulet;

        public abstract void OnParticleSystemStopped();
    }
}