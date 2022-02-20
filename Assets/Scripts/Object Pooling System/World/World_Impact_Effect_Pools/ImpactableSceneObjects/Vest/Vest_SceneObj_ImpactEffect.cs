using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Vest_SceneObj_ImpactEffect : WorldImpactEffect
    {
        public override void ReturnEffectToPool()
        {
            gameManager._vestSceneObj_pool.ReturnToPool(this);
        }
    }
}