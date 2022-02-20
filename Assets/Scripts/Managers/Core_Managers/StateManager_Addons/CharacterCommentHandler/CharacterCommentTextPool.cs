using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Player/Character Comment Text Pool")]
    public class CharacterCommentTextPool : ScriptableObject
    {
        [SerializeField] string[] _commentTexts;

        public string GetRandomCommentFromPool()
        {
            return _commentTexts[Random.Range(0, _commentTexts.Length - 1)];
        }
    }
}