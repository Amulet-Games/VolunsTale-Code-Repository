using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PowerupTakeChestInteractable : TakeChestInteractable
    {
        [Header("Powerup Take Chest Config.")]
        public Item _chestItem;

        public override void OnInteract()
        {
            _states.PowerupTakeChestInteractable();
        }

        protected override void CollectChestItems()
        {
            ShowItemInfoCard();

            _chestItem.CreateInstanceForPickups(_states._savableInventory);

            LeanTween.value(0, 1, 2.5f).setOnComplete(OnCompletePreviewWait);

            void ShowItemInfoCard()
            {
                _mainHudManager._itemInfoCard.OnItemInfoCard_Powerup(_chestItem.itemName);
            }

            void OnCompletePreviewWait()
            {
                _states.OnPowerupPreivewComplete();
            }
        }
    }
}