using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ArmorTakeChestInteractable : TakeChestInteractable
    {
        [Header("Armor Take Chest Config.")]
        public string _specificItemInfoName;
        public Item[] _chestItems;
        
        public override void OnInteract()
        {
            _states.ArmorTakeChestInteractable();
        }

        protected override void CollectChestItems()
        {
            ShowItemInfoCard();

            for (int i = 0; i < _chestItems.Length; i++)
            {
                _chestItems[i].CreateInstanceForPickups(_states._savableInventory);
            }

            LeanTween.value(0, 1, 2.5f).setOnComplete(OnCompletePreviewWait);

            void ShowItemInfoCard()
            {
                _mainHudManager._itemInfoCard.OnItemInfoCard_Armor(_specificItemInfoName);
            }

            void OnCompletePreviewWait()
            {
                _states.OnArmorPreviewDissolve();
            }
        }
    }
}