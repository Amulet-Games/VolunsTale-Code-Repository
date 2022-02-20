using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Execution Profile")]
    public class Player_ExecutionProfile : ScriptableObject
    {
        public float _executionMoveDistance;
        public float _executionHeightBuffer;
        public float _executionRootMotion;
        public bool _isExecutionDividedInThreeSection;

        [Header("AI Bfx Type.")]
        public ExecutionPhysicalTypeEnum _1st_hit_phys_type;
        public ExecutionPhysicalTypeEnum _2nd_hit_phys_type;
        public ExecutionPhysicalTypeEnum _3rd_hit_phys_type;

        public enum ExecutionPhysicalTypeEnum
        {
            Strike,
            Slash,
            Thrust
        }
    }
}