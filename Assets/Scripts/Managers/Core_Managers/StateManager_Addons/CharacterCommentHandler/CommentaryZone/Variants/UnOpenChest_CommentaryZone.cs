using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class UnOpenChest_CommentaryZone : CommentaryZone
    {
        [Header("UnOpen Chest Interactable.")]
        public OpenChestInteractable _referingOpenChestInteractable;
        
        private void OnTriggerEnter(Collider other)
        {
            if (_commentHandler._chestAroundCommentaryMoment._isInCoolDown)
                return;

            if (_isOneDirection)
            {
                /// If player is facing almost the same direction as the zone.
                if (Vector3.Dot(transform.forward, transform.position - other.transform.position) > 0)
                {
                    _commentHandler.RegisterUnOpenedChestCommentMoment();
                }
            }
            else
            {
                _commentHandler.RegisterUnOpenedChestCommentMoment();
            }
        }

        /// Interactable Interacted.
        public void OnChestOpened()
        {
            gameObject.SetActive(false);
        }

        /// Setup.
        public override void Setup(CharacterCommentHandler _commentHandler)
        {
            this._commentHandler = _commentHandler;
            _referingOpenChestInteractable._unOpenChestCommentaryZone = this;

            if (!_referingOpenChestInteractable.isInteracted)
            {
                gameObject.SetActive(true);
            }
        }
    }
}