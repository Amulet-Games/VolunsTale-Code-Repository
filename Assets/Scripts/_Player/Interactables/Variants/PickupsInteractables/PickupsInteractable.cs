using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class PickupsInteractable : PlayerInteractable
    {
        [Header("Base Pickup Config.")]
        public PlayerPickupIntactablePosType _pickupPos;
        public int _pickupAmount = 1;

        [Header("Drag and Drop Refs.")]
        public Collider pickupTriggerCollider = null;

        public override void SetInteractionContent()
        {
            _mainHudManager.SetNextInterContent_Pickup();
        }

        protected void TurnOffInteraction()
        {
            pickupTriggerCollider.enabled = false;
            gameObject.SetActive(false);
            enabled = false;
        }
    }

    public enum PlayerPickupIntactablePosType
    {
        High,
        Mid,
        Down
    }
}