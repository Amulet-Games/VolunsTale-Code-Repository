using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class WeaponPickupsInteractable : PickupsInteractable
    {
        [Header("Weapon Item.")]
        public WeaponItem _pickupItem;

        [Header("Linked Scene Object.")]
        public GameObject _linkedSceneWeaponObj;
        public MeshRenderer _linkedSceneWeaponMesh;
        public float _dissolveFullOpaqueValue;
        public float _dissolveFullTransparentValue;
        Material _linkedSceneWeaponMaterial;

        public override void Late_Init()
        {
            Base_Late_Init();
            
            InitDissolveMaterial();
            
            void InitDissolveMaterial()
            {
                _linkedSceneWeaponMaterial = _linkedSceneWeaponMesh.material;
                _linkedSceneWeaponMaterial.SetFloat(_states._dissolveCutoffPropertyId, _dissolveFullOpaqueValue);
            }
        }

        public override void OnInteract()
        {
            _states.PickupInteractable(_pickupPos);
            _states._commentHandler._currentPickupCommentType = PickupCommentaryTypeEnum.Weapons;
        }

        public override void OnAnimEventInteract()
        {
            isInteracted = true;

            CollectWeaponPickups();
            ShowItemInfoCard();
            DissolveLinkedSceneWeapon();

            TurnOffInteraction();

            _states.OnInteractingClearFoundInteractables();
            _mainHudManager.HideInteractionCard_FadeOut();

            void CollectWeaponPickups()
            {
                _pickupItem.CreateInstanceForPickupsWithAmount(_states._savableInventory, _pickupAmount);
            }

            void ShowItemInfoCard()
            {
                _mainHudManager._itemInfoCard.OnItemInfoCard_Weapon(_pickupItem.itemName);
            }

            void DissolveLinkedSceneWeapon()
            {
                LeanTween.value(_dissolveFullOpaqueValue, _dissolveFullTransparentValue, 1f).setOnUpdate
                    (
                        (value) => _linkedSceneWeaponMaterial.SetFloat(_states._dissolveCutoffPropertyId, value)
                    )
                    .setOnComplete(OnCompleteDissolveLinkedSceneWeapon);

                void OnCompleteDissolveLinkedSceneWeapon()
                {
                    _states._savableInventory.RefreshAmuletEmissionWhenPickupWeapon();
                    _linkedSceneWeaponObj.SetActive(false);
                }
            }
        }

        public override void OnShowInteractedResult()
        {
            TurnOffInteraction();
            _linkedSceneWeaponObj.SetActive(false);
        }
    }
}