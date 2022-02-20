using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ImpactableStoneObject : ImpactableSceneObject
    {
        public override void SpawnImpactEffectWhenHit(Vector3 _spawnPoint)
        {
            GameManager.singleton._stoneSceneObj_pool.Get().SceneObjectsHit_SpawnEffect(_spawnPoint);
        }
    }
}