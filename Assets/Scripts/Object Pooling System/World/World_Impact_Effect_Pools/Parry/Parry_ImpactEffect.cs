using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Parry_ImpactEffect : WorldImpactEffect
    {
        public override void ReturnEffectToPool()
        {
            ReturnToBackpack();
        }
    }
}