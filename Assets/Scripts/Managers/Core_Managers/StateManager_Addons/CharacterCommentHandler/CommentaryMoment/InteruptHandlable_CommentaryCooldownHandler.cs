using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [Serializable]
    public class InteruptHandlable_CommentaryCooldownHandler : CommentaryCooldownHandler
    {
        bool _isInterrupted;

        protected override void SetIsInCoolDownToFalse()
        {
            if (_isInterrupted)
            {
                _commentHandler._states.InterruptedComment_TriggerCommentTrigger();
                _isInterrupted = false;
            }

            _isInCoolDown = false;
            _commentHandler.RemoveFromCoolDownables(this);
        }

        public override void HandleInterrupt()
        {
            _isInterrupted = true;

            _commentHandler.commentDisplayTimer = 0;
            _timer = _timeRate - 8;
        }
    }
}