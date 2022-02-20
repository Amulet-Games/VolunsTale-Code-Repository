using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ImpactableWoodObject : ImpactableSceneObject
    {
        public override void SpawnImpactEffectWhenHit(Vector3 _spawnPoint)
        {
            GameManager.singleton._woodSceneObj_pool.Get().SceneObjectsHit_SpawnEffect(_spawnPoint);
        }
    }
}