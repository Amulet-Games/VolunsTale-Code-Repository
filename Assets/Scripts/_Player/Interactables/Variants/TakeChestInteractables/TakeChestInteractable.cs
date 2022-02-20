using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class TakeChestInteractable : PlayerInteractable
    {
        [Header("Drag and Drop Refs.")]
        [SerializeField] Collider takeTriggerCollider = null;

        public override void Late_Init()
        {
            Base_Late_Init();
        }

        public override void SetInteractionContent()
        {
            if (Time.frameCount % 25 == 0)
            {
                Vector3 _dirToPlayer = _states.mTransform.position - transform.position;
                if (Vector3.Dot(_dirToPlayer.normalized, transform.forward) > 0.75f)
                {
                    if (Vector3.SqrMagnitude(_dirToPlayer) < 1)
                    {
                        _mainHudManager.SetNextInterContent_TakeChest();
                    }
                }
            }
        }

        public override void OnAnimEventInteract()
        {
            isInteracted = true;
            CollectChestItems();

            TurnOffInteraction();
            
            _states.OnInteractingClearFoundInteractables();
            _mainHudManager.HideInteractionCard_FadeOut();
        }

        protected abstract void CollectChestItems();

        public void TurnOnInteraction_ByOpenChest()
        {
            gameObject.layer = _states._layerManager.playerInteractableLayer;
            takeTriggerCollider.enabled = true;
            gameObject.SetActive(true);
        }

        public void TurnOnInteraction_BySavedState()
        {
            takeTriggerCollider.enabled = true;
            gameObject.SetActive(true);
        }

        void TurnOffInteraction()
        {
            takeTriggerCollider.enabled = false;
            gameObject.SetActive(false);
        }
        
        public override void OnShowInteractedResult()
        {
            TurnOffInteraction();
        }
    }
}