using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Player Variables/Weapon Opposed Hand Transform")]
    public class WeaponOpposedHandTransform : ScriptableObject
    {
        public Vector3 pos;
        public Vector3 eulers;
    }
}
