using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Unity GameEvent")]

    public class UnityGameEvent : ScriptableObject
    {
        public UnityGameEventListener listener;

        public void Register(UnityGameEventListener _listener)
        {
            listener = _listener;
        }

        public void UnRegister(UnityGameEventListener _listener)
        {
            listener = null;
        }

        public void Raise()
        {
            listener.Raise();
        }
    }
}