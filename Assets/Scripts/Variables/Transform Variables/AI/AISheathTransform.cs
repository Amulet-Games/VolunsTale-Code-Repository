using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Sheath Transform")]
    public class AISheathTransform : ScriptableObject
    {
        [Header("Transform")]
        public Vector3 pos;
        public Vector3 eulers;
        public Vector3 scale;
    }
}