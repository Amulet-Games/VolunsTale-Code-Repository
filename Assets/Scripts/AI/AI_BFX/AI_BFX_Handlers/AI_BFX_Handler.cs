using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class AI_BFX_Handler : AI_BFX_HandlerBase
    {
        [Header("Id.")]
        public int _ai_Bfx_ID;
        
        public abstract void Setup(AISessionManager _aiSessionManager);
        
        public abstract void Start_AI_Bfx();
    }
}