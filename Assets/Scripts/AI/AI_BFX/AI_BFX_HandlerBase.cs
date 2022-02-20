using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_BFX_HandlerBase : MonoBehaviour
    {
        // Hide In Inspector.
        [HideInInspector]
        public Vector2 orig_offset = new Vector2(0, 0);

        [Header("Base Ref.")]
        [ReadOnlyInspector] public AISessionManager aiSessionManager;
        
        public abstract void Tick();

        public abstract void End_AI_Bfx();

        protected void AddToActiveList()
        {
            aiSessionManager.OnActiveListAdd_AI_Bfx_Handler(this);
        }

        protected void RemoveFromActiveList()
        {
            aiSessionManager.OnActiveListRemove_AI_Bfx_Handler(this);
        }
        
        public void ReturnToBackpack()
        {
            transform.parent = aiSessionManager._ai_Bfx_Backpack;
            gameObject.SetActive(false);
        }
    }
}