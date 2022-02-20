using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SA
{
    public class NavigationDebugger : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent agentToDebug = null;

        private LineRenderer linerenderer;

        private void Start()
        {
            linerenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if(agentToDebug.hasPath)
            {
                linerenderer.positionCount = agentToDebug.path.corners.Length;
                linerenderer.SetPositions(agentToDebug.path.corners);
                linerenderer.enabled = true;
            }
            else
            {
                linerenderer.enabled = false;
            }
        }
    }
}