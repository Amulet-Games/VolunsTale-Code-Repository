using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SA
{
    public class UnityGameEventListener : MonoBehaviour
    {
        public UnityGameEvent targetEvent;

        public UnityEvent response;

        private void OnEnable()
        {
            targetEvent.UnRegister(this);
            targetEvent.Register(this);
        }

        public virtual void Raise()
        {
            response.Invoke();
        }
    }
}