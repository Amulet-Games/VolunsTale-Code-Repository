using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InteractableSessionManager : MonoBehaviour
    {
        [Header("Array Awake Init.")]
        public PlayerInteractable[] _allPlayerInteractables;

        [Header("Array Late Init.")]
        public OpenDoorInteractable[] _openDoorInters;
        [Space(10)]
        public OpenChestInteractable[] _openChestInters;
        [Space(10)]
        public RingOpenChestInteractable[] _openRingChestInters;
        [Space(10)]
        public TakeChestInteractable[] _takeChestInters;
        [Space(10)]
        public PickupsInteractable[] _pickupInters;
        [Space(10)]
        public IgniteInteractable[] _igniteInters;
        [Space(10)]
        public RestInteractable[] _restInters;

        void Awake()
        {
            AwakeInit();
        }

        #region Awake Init.
        void AwakeInit()
        {
            SavableManager _savableManager = SavableManager.singleton;

            int _length = _allPlayerInteractables.Length;
            for (int i = 0; i < _length; i++)
            {
                _savableManager._playerInteractables.Add(_allPlayerInteractables[i]);
            }
        }
        #endregion

        void Update()
        {
            LateInit();
        }

        #region Late Init.
        void LateInit()
        {
            if (SessionManager.singleton.isCurrentAsyncOperationFinished)
            {
                // Open Doors.
                InitInteractables_IgnoreInteracted(_openDoorInters);

                // Chest.
                InitInteractables_IgnoreInteracted(_openChestInters);
                InitInteractables_IgnoreInteracted(_openRingChestInters);
                InitInteractables_IgnoreInteracted(_takeChestInters);

                // Pickups.
                InitInteractables_IgnoreInteracted(_pickupInters);

                // Ignites.
                InitInteractables_IgnoreInteracted(_igniteInters);

                // Rests.
                InitInteractables_IgnoreNon(_restInters);

                enabled = false;

                // Wait For 5 sec and destroy this script. 
                LeanTween.value(0, 1, 5).setOnComplete(OnCompleteWaitDestroyScript);
            }
        }

        void InitInteractables_IgnoreInteracted(PlayerInteractable[] _playerInteractables)
        {
            for (int i = 0; i < _playerInteractables.Length; i++)
            {
                if (_playerInteractables[i].isInteracted)
                {
                    continue;
                }
                else
                {
                    _playerInteractables[i].Late_Init();
                }
            }
        }

        void InitInteractables_IgnoreNon(PlayerInteractable[] _playerInteractables)
        {
            for (int i = 0; i < _playerInteractables.Length; i++)
            {
                _playerInteractables[i].Late_Init();
            }
        }

        void OnCompleteWaitDestroyScript()
        {
            Destroy(this);
        }
        #endregion
    }
}