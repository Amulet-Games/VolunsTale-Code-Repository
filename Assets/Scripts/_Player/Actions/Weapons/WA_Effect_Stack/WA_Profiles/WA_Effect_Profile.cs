using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/WA Effect Profile")]
    public class WA_Effect_Profile : ScriptableObject
    {
        [Header("Id.")]
        public int _id;

        [Header("Transform")]
        public Vector3 localPos;
        public Vector3 localEulers;

        public virtual bool GetIsLeftHand()
        {
            return false;
        }
    }
}