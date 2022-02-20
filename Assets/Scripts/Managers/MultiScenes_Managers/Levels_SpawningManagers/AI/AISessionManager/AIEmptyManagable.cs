using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Managable/Empty AI Managable")]
    public class AIEmptyManagable : ScriptableObject, AI_Managable
    {
        ///* This is an empty class use for AI Managable,
        ///* Don't implement any logic here.

        public void FixedTick()
        {
        }

        public void Init(AISessionManager _aiSessionManager)
        {
        }

        public void LateTick()
        {
        }

        public void Setup()
        {
        }

        public void Tick()
        {
        }
    }
}