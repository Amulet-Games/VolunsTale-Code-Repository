using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class CommentaryCooldownHandler
    {
        public float _timeRate;
        protected float _timer;

        [NonSerialized] public bool _isInCoolDown;
        [NonSerialized] public CharacterCommentHandler _commentHandler;

        public void MonitorCommentaryCoolDown()
        {
            _timer += _commentHandler._delta;
            if (_timer > _timeRate)
            {
                _timer = 0;
                SetIsInCoolDownToFalse();
            }
        }
        
        protected virtual void SetIsInCoolDownToFalse()
        {
            _isInCoolDown = false;
            _commentHandler.RemoveFromCoolDownables(this);
        }

        public virtual void HandleInterrupt()
        {
        }
    }
}