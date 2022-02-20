using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_IndicatorOnStopHandler : MonoBehaviour
    {
        /// Callback.
        public void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
        }
    }
}