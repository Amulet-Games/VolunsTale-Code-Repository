using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class CommentaryZone : MonoBehaviour
    {
        [Header("One Direction.")]
        public bool _isOneDirection;

        [Header("Refs.")]
        [ReadOnlyInspector] public CharacterCommentHandler _commentHandler;
        
        public abstract void Setup(CharacterCommentHandler _commentHandler);
    }
}