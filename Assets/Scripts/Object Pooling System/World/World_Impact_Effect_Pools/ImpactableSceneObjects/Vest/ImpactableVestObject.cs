using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ImpactableVestObject : ImpactableSceneObject
    {
        public override void SpawnImpactEffectWhenHit(Vector3 _spawnPoint)
        {
            GameManager.singleton._vestSceneObj_pool.Get().SceneObjectsHit_SpawnEffect(_spawnPoint);
        }
    }
}