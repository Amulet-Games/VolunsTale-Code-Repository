using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class CameraAction : ScriptableObject
    {
        public abstract void Execute(CameraHandler camHandler);
    }
}