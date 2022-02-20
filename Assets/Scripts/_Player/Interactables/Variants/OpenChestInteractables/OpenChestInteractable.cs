using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class OpenChestInteractable : PlayerInteractable
    {
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isShowingInterCard;

        [Header("Refs.")]
        [ReadOnlyInspector] public Transform mTransform;

        [Header("Drag and Drop Refs.")]
        public Collider chestTriggerCollider;
        public Animation openChest_legacyAnimation;
        public TakeChestInteractable _takeChestInteractable;
        [ReadOnlyInspector] public UnOpenChest_CommentaryZone _unOpenChestCommentaryZone;

        public override void Late_Init()
        {
            Base_Late_Init();
            
            InitGetRefs();
            
            void InitGetRefs()
            {
                mTransform = transform;
            }
        }

        public override void SetInteractionContent()
        {
            if (Time.frameCount % 25 == 0)
            {
                Vector3 _dirToPlayer = _states.mTransform.position - mTransform.position;
                if (Vector3.Dot(_dirToPlayer.normalized, mTransform.forward) > 0.75f)
                {
                    if (Vector3.Dot(_states.mTransform.forward, mTransform.forward) < -0.75f)
                    {
                        if (Vector3.SqrMagnitude(_dirToPlayer) < 1)
                        {
                            _mainHudManager.SetNextInterContent_OpenChest();
                            _isShowingInterCard = true;
                            return;
                        }
                    }
                }

                if (_isShowingInterCard)
                {
                    _states.SetPotentialInteractableToNull();
                    _isShowingInterCard = false;
                }
            }
        }

        public override void OnInteract()
        {
            _states.OpenChestInteractable();

            if (_unOpenChestCommentaryZone != null)
                _unOpenChestCommentaryZone.OnChestOpened();
        }

        public override void OnAnimEventInteract()
        {
            isInteracted = true;
            TurnOffInteraction();

            _states.OnInteractingClearFoundInteractables();
            _mainHudManager.HideInteractionCard_FadeOut();

            _takeChestInteractable.TurnOnInteraction_ByOpenChest();
        }
        
        void TurnOffInteraction()
        {
            gameObject.layer = LayerManager.singleton.defaultLayer;
            openChest_legacyAnimation.Play();
            chestTriggerCollider.enabled = false;
            enabled = false;
        }

        #region Anim Events.
        public void DisableChestInteractableRefs()
        {
            openChest_legacyAnimation.enabled = false;
        }
        #endregion
        
        public override void OnShowInteractedResult()
        {
            TurnOnTakeChest();
            TurnOffInteraction();
        }

        void TurnOnTakeChest()
        {
            /// If player opened the chest but haven't take the chest.
            if (!_takeChestInteractable.isInteracted)
            {
                _takeChestInteractable.TurnOnInteraction_BySavedState();
            }
        }
    }
}