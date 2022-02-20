using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ImpactableSceneObject : MonoBehaviour
{
    public abstract void SpawnImpactEffectWhenHit(Vector3 _spawnPoint);
}
