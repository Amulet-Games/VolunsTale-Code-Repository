using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CommentaryZonesActivator : MonoBehaviour
    {
        [Header("Commentary Zones.")]
        public CommentaryZone[] _zones;

        public static CommentaryZonesActivator _singleton;
        private void Awake()
        {
            if (_singleton != null)
                Destroy(this);
            else
                _singleton = this;
        }

        public void ActivateCommentaryZones(CharacterCommentHandler _commentHandler)
        {
            for (int i = 0; i < _zones.Length; i++)
            {
                _zones[i].Setup(_commentHandler);
            }

            gameObject.SetActive(false);
        }
    }
}