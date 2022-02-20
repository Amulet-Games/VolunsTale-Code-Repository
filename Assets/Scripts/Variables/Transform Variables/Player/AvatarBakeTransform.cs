using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Player Variables/Avatar Bake Transform")]
    public class AvatarBakeTransform : ScriptableObject
    {
        /// This is a local Pos & Eulers for Bake Camera
        [Header("Transform")]
        public Vector3 localPos;
        public Vector3 localEulers;
    }
}