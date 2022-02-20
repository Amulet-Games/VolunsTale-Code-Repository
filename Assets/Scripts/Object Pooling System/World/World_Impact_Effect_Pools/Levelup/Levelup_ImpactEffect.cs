using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Levelup_ImpactEffect : WorldImpactEffect
    {
        public override void ReturnEffectToPool()
        {
            ReturnToBackpack();
        }
    }
}