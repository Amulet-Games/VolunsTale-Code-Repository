using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class IgniteInteractable : PlayerInteractable
    {
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isShowingInterCard;

        [Header("Drag and Drop Refs.")]
        [SerializeField] Collider igniteTriggerCollider = null;
        [SerializeField] GameObject igniteImpactParticle;
        [SerializeField] GameObject bonfireParticle;
        [SerializeField] RestInteractable restInteractable;
        [ReadOnlyInspector] public UnSecuredCamp_CommentaryZone unSecuredCampCommentZone;

        public override void Late_Init()
        {
            Base_Late_Init();
            
            DeactivateIgniteParticles();
        }

        public override void SetInteractionContent()
        {
            if (Time.frameCount % 25 == 0)
            {
                // if player is facing the door.
                if (Vector3.Dot((transform.position - _states.mTransform.position).normalized, _states.mTransform.forward) > 0.75f)
                {
                    _mainHudManager.SetNextInterContent_Ignite();
                    _isShowingInterCard = true;
                }
                else if (_isShowingInterCard)
                {
                    _states.SetPotentialInteractableToNull();
                    _isShowingInterCard = false;
                }
            }
        }

        public override void OnInteract()
        {
            LooksTowardBonfire();

            _states.IgniteBonfireInteractable();

            _mainHudManager.HideInteractionCard_FadeOut();

            void LooksTowardBonfire()
            {
                Vector3 fireDir = transform.position - _states.mTransform.position;
                fireDir.y = 0;
                _states.mTransform.rotation = Quaternion.LookRotation(fireDir);
            }
        }

        public override void OnAnimEventInteract()
        {
            isInteracted = true;

            _mainHudManager.ShowBonfireLitScreen();
            ActivateIgniteParticles();

            restInteractable.TurnOnInteraction_ByIgnite();
            TurnOffInteraction();

            unSecuredCampCommentZone.OnBonfireIgnited();

            _states.OnInteractingClearFoundInteractables();
        }

        public void TurnOffInteraction()
        {
            gameObject.layer = LayerManager.singleton.defaultLayer;
            igniteTriggerCollider.enabled = false;
            enabled = false;
        }

        #region Activate Ignite Particles.
        void ActivateIgniteParticles()
        {
            igniteImpactParticle.SetActive(true);
        }

        void DeactivateIgniteParticles()
        {
            igniteImpactParticle.SetActive(false);
        }
        #endregion

        #region Activate Bonfire Particles.
        public void ActivateBonfireParticles()
        {
            bonfireParticle.SetActive(true);
        }

        public void DeactivateBonfireParticles()
        {
            bonfireParticle.SetActive(false);
        }
        #endregion

        #region Commentary Zone.
        void OnInteractDeactivateCommentaryZone()
        {
            unSecuredCampCommentZone.gameObject.SetActive(false);
        }
        #endregion

        public override void OnShowInteractedResult()
        {
            TurnOffInteraction();
        }
    }
}