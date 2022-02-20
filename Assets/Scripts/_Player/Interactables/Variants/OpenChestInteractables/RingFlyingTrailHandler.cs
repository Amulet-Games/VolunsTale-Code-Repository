using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RingFlyingTrailHandler : MonoBehaviour
    {
        public RingOpenChestInteractable _linkedRingOpenChestInteractable;
        
        public void BringRingCloserToPlayerHand()
        {
            LeanTween.move(gameObject, _linkedRingOpenChestInteractable._states.rightIndexIntermediateTransform.position, 0.35f)
                .setEaseOutCirc()
                .setOnComplete(_linkedRingOpenChestInteractable.OnRingCloseToHand);

            _linkedRingOpenChestInteractable.DisableChestInteractableRefs();
        }
    }
}