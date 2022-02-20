using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_Interactable : MonoBehaviour
    {
        [Header("AI Inter Type.")]
        public EnemyInteractableMod.AI_InteractableTypeEnum interactableType;

        public abstract void Execute(AIManager ai);
    }
}