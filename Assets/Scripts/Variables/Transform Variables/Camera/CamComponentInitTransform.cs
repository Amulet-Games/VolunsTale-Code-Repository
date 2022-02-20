using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Camera Handler/Variables/Camera Component Init Transform")]
    public class CamComponentInitTransform : ScriptableObject
    {
        [Header("Cam Component Transform.")]
        public Vector3 _componentLocalPos;
        public Vector3 _componentLocalEulers;
    }
}