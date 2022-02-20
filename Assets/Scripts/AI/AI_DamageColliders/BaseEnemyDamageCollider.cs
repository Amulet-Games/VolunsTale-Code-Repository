using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BaseEnemyDamageCollider : MonoBehaviour
    {
        public Collider _collider;
        [ReadOnlyInspector] public AIManager _ai;
        [ReadOnlyInspector] public StateManager playerStates;
        [ReadOnlyInspector] public LayerManager _layerManager;

        public abstract void Setup(AIManager ai);
    }
}