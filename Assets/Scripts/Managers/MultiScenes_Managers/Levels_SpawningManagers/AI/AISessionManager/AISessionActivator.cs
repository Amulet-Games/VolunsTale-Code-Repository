using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AISessionActivator : MonoBehaviour
    {
        public AISessionManager _aiSessionManager;
        
        void Update()
        {
            if (SessionManager.singleton._states)
            {
                _aiSessionManager.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}