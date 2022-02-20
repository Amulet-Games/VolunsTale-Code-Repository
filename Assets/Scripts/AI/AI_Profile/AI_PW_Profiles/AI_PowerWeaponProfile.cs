using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Profile/PW Profile")]
    public class AI_PowerWeaponProfile : ScriptableObject
    {
        public AI_ActionHolder pw_ActionHolder;
        public int pw_AnimStateLayer;
    }
}