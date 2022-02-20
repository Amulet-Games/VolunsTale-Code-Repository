using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Variables/Anim State Variable")]
    public class AnimStateVariable : ScriptableObject
    {
        [TextArea]
        public string Note = "This scriptable object name has to match the animation state's name it represents!";

        [Space(10)]
        public string animStateName;

        [NonSerialized, ReadOnlyInspector]
        public int animStateHash;
    }
}