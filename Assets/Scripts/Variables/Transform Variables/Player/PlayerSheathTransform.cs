using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Player Variables/Player Sheath Transform")]
    public class PlayerSheathTransform : ScriptableObject
    {
        [Header("Transform")]
        public Vector3 pos;
        public Vector3 eulers;
        public SheathParentBoneTypeEnum parentBoneType;

        public enum SheathParentBoneTypeEnum
        {
            Spine,      // HumanBodyBones.Spine_01
            Hips        // HumanBodyBones.Hips
        }
    }
}