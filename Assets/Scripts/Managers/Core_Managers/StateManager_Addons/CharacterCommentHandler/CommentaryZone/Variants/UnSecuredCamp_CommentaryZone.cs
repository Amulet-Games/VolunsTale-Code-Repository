using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class UnSecuredCamp_CommentaryZone : CommentaryZone
    {
        [Header("Ignite Interactable.")]
        public IgniteInteractable _referingIgniteInteractable;
        public UnsecuredCampCommentaryTypeEnum _unsecuredCampCommentType;

        private void OnTriggerEnter(Collider other)
        {
            if (_commentHandler._campAroundCommentaryMoment._isInCoolDown)
                return;

            if (_isOneDirection)
            {
                /// If player is facing almost the same direction as the zone.
                if (Vector3.Dot(transform.forward, transform.position - other.transform.position) > 0)
                {
                    _commentHandler.RegisterUnSecuredCampCommentMoment(_unsecuredCampCommentType);
                }
            }
            else
            {
                _commentHandler.RegisterUnSecuredCampCommentMoment(_unsecuredCampCommentType);
            }
        }
        
        public void OnBonfireIgnited()
        {
            _commentHandler.Add_CampAround_ToCoolDownables();
            gameObject.SetActive(false);
        }

        /// Setup.
        public override void Setup(CharacterCommentHandler _commentHandler)
        {
            this._commentHandler = _commentHandler;
            _referingIgniteInteractable.unSecuredCampCommentZone = this;

            if (!_referingIgniteInteractable.isInteracted)
            {
                gameObject.SetActive(true);
            }
        }
    }
}