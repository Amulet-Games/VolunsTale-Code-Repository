using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RingOpenChestInteractable : PlayerInteractable
    {
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isShowingInterCard;

        [Header("Refs.")]
        [ReadOnlyInspector] public Transform mTransform;

        [Header("Drag and Drop Refs.")]
        public Collider chestTriggerCollider;
        public Animation openChest_legacyAnimation;
        [ReadOnlyInspector] public RingUnOpenChest_CommentaryZone _ringUnOpenChestCommentaryZone;

        [Header("Ring Item.")]
        public RingItem _ringItem;
        public ParticleSystem _ringChestTakeInteractableFx;

        [Header("Rings Flying Fx.")]
        public ParticleSystem _ringFlyingFx;
        public RingFlyingTrailHandler _linkedRingFlyingHandler;

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

            if (_ringUnOpenChestCommentaryZone != null)
                _ringUnOpenChestCommentaryZone.OnChestOpened();
        }

        public override void OnAnimEventInteract()
        {
            TurnOffInteraction();

            _states.OnInteractingClearFoundInteractables();
            _mainHudManager.HideInteractionCard_FadeOut();

            _ringChestTakeInteractableFx.Play();
        }

        void TurnOffInteraction()
        {
            gameObject.layer = LayerManager.singleton.defaultLayer;
            openChest_legacyAnimation.Play();
            chestTriggerCollider.enabled = false;
            enabled = false;
        }

        /// Anim Event.
        public void BeginRingFlying()
        {
            if (!isInteracted)
            {
                isInteracted = true;

                _ringFlyingFx.Play();
                openChest_legacyAnimation.Play("ChestRingFlying");
            }
        }

        public void BringRingCloserToPlayerHand()
        {
            _linkedRingFlyingHandler.BringRingCloserToPlayerHand();
        }

        public void OnRingCloseToHand()
        {
            CollectRing();
            OnRingPreview();
            ShowItemInfoCard();
            LeaveTakeRingComment();
            StartReversePreviewWaitCounter();

            void CollectRing()
            {
                _ringItem.CreateInstanceForPickups(_states._savableInventory);
            }

            void OnRingPreview()
            {
                _states._savableInventory.PreviewRing(_ringFlyingFx.transform);
            }

            void ShowItemInfoCard()
            {
                _mainHudManager._itemInfoCard.OnItemInfoCard_Ring(_ringItem.itemName);
            }

            void LeaveTakeRingComment()
            {
                _states.TakeRingChestItemLeaveComment();
            }

            void StartReversePreviewWaitCounter()
            {
                LeanTween.value(0, 1, 1.5f).setOnComplete(OnCompleteWait);

                void OnCompleteWait()
                {
                    _ringFlyingFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    _states._savableInventory.runtimeAmulet.PlayRingAbsorbFx();
                }
            }
        }

        public void DisableChestInteractableRefs()
        {
            openChest_legacyAnimation.enabled = false;
            _ringChestTakeInteractableFx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        public override void OnShowInteractedResult()
        {
            TurnOffInteraction();

            _ringChestTakeInteractableFx.gameObject.SetActive(false);
            _ringFlyingFx.gameObject.SetActive(false);
        }
    }
}