using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Stone_SceneObj_ImpactEffect : WorldImpactEffect
    {
        public override void ReturnEffectToPool()
        {
            gameManager._stoneSceneObj_pool.ReturnToPool(this);
        }
    }
}