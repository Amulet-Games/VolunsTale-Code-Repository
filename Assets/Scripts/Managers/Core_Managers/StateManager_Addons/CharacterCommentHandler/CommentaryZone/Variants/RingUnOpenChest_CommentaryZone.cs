using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RingUnOpenChest_CommentaryZone : CommentaryZone
    {
        [Header("Ring UnOpen Chest Interactable.")]
        public RingOpenChestInteractable _referingRingUnOpenChestInteractable;

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
            _referingRingUnOpenChestInteractable._ringUnOpenChestCommentaryZone = this;

            if (!_referingRingUnOpenChestInteractable.isInteracted)
            {
                gameObject.SetActive(true);
            }
        }
    }
}