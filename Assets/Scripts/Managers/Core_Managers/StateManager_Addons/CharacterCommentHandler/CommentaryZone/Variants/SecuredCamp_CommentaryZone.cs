using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SecuredCamp_CommentaryZone : CommentaryZone
    {
        [Header("Ignite Interactable.")]
        public RestInteractable _referingRestInteractable;

        private void OnTriggerEnter(Collider other)
        {
            if (_commentHandler._campAroundCommentaryMoment._isInCoolDown)
                return;

            if (_isOneDirection)
            {
                /// If player is facing almost the same direction as the zone.
                if (Vector3.Dot(transform.forward, transform.position - other.transform.position) > 0)
                {
                    _commentHandler.RegisterSecuredCampCommentMoment();
                }
            }
            else
            {
                _commentHandler.RegisterSecuredCampCommentMoment();
            }
        }
        
        public void OnCampSecured()
        {
            gameObject.SetActive(true);
        }

        /// Setup.
        public override void Setup(CharacterCommentHandler _commentHandler)
        {
            this._commentHandler = _commentHandler;
            _referingRestInteractable.securedCampCommentZone = this;

            if (_referingRestInteractable.isInteracted)
            {
                gameObject.SetActive(true);
            }
        }
    }
}