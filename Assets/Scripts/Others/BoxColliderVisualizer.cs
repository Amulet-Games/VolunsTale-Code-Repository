using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class BoxColliderVisualizer : MonoBehaviour
    {
        public bool _enable;
        public Color _blenderColor;

        private void OnDrawGizmos()
        {
            if (_enable)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.color = _blenderColor;
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
            }
        }
    }
}