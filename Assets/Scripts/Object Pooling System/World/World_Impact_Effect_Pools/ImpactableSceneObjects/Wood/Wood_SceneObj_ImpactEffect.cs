using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Wood_SceneObj_ImpactEffect : WorldImpactEffect
    {
        public override void ReturnEffectToPool()
        {
            gameManager._woodSceneObj_pool.ReturnToPool(this);
        }
    }
}