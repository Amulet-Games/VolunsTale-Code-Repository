using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class OpenDoorInteractable : PlayerInteractable
    {
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isShowingInterCard;

        [Header("Refs.")]
        [ReadOnlyInspector] public Transform mTransform;

        [Header("Drag and Drop Refs.")]
        [SerializeField] Collider doorTriggerCollider;
        [SerializeField] Animation openDoor_legacyAnimation;

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
                // if player is in opposite side.
                if (Vector3.Dot(_states.mTransform.position - mTransform.position, mTransform.forward) <= 0)
                {
                    // if player is facing the door.
                    if (Vector3.Dot(_states.mTransform.forward, mTransform.forward) > 0.75f)
                    {
                        _mainHudManager.SetNextInterContent_OpenDoor();
                        _isShowingInterCard = true;
                    }
                    else if (_isShowingInterCard)
                    {
                        _states.SetPotentialInteractableToNull();
                        _isShowingInterCard = false;
                    }
                }
                else
                {
                    // if player is facing the door.
                    if (Vector3.Dot(_states.mTransform.forward, mTransform.forward) < -0.75f)
                    {
                        _mainHudManager.SetNextInterContent_OpenDoor();
                        _isShowingInterCard = true;
                    }
                    else if (_isShowingInterCard)
                    {
                        _states.SetPotentialInteractableToNull();
                        _isShowingInterCard = false;
                    }
                }
            }
        }

        public override void OnInteract()
        {
            // if player is in opposite side.
            float dot = Vector3.Dot(_states.mTransform.position - mTransform.position, mTransform.forward);
            if (dot <= 0)
            {
                CreateCantOpenConfirm();
            }
            else
            {
                isInteracted = true;
                
                CreateOpenedShotcutConfirm();
                TurnOffInteraction();
            }

            _states._isPausingSearchInteractables = true;
            _states.OnInteractingClearFoundInteractables();
            _mainHudManager.HideInteractionCard_FadeOut();
        }

        void CreateCantOpenConfirm()
        {
            _states.CantOpenDoorInteractable();
        }

        void CreateOpenedShotcutConfirm()
        {
            _states.OpenDoorInteractable();
        }
        
        void TurnOffInteraction()
        {
            gameObject.layer = LayerManager.singleton.defaultLayer;
            openDoor_legacyAnimation.Play();
            doorTriggerCollider.enabled = false;
            enabled = false;
        }

        public override void OnShowInteractedResult()
        {
            TurnOffInteraction();
        }

        #region Anim Events.
        public void DisableDoorAnimatorStatus()
        {
            openDoor_legacyAnimation.enabled = false;
        }
        #endregion
    }
}