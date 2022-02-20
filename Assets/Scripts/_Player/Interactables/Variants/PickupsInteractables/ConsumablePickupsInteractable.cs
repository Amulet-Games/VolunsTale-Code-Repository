using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ConsumablePickupsInteractable : PickupsInteractable
    {
        [Header("Consumable Item.")]
        public ConsumableItem _pickupItem;

        public override void Late_Init()
        {
            Base_Late_Init();
        }

        public override void OnInteract()
        {
            _states.PickupInteractable(_pickupPos);
            _states._commentHandler._currentPickupCommentType = PickupCommentaryTypeEnum.Consumables;
        }

        public override void OnAnimEventInteract()
        {
            isInteracted = true;
            
            CollectConsumablePickups();
            ShowItemInfoCard();
            PlayConsumableAbsorbFx();

            TurnOffInteraction();

            _states.OnInteractingClearFoundInteractables();
            _mainHudManager.HideInteractionCard_FadeOut();

            void CollectConsumablePickups()
            {
                _pickupItem.CreateInstanceForPickupsWithAmount(_states._savableInventory, _pickupAmount);
            }

            void ShowItemInfoCard()
            {
                _mainHudManager._itemInfoCard.OnItemInfoCard_Consumable(_pickupItem.itemName, 1, _pickupItem._noBaseItemIcon);
            }

            void PlayConsumableAbsorbFx()
            {
                _states._savableInventory.RefreshAmuletEmissionWithPickupConsums();
            }
        }
        
        public override void OnShowInteractedResult()
        {
            TurnOffInteraction();
        }
    }
}