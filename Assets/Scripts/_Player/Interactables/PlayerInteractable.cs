using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class PlayerInteractable : MonoBehaviour
    {
        [Header("Serialization.")]
        public string interactionId;
        
        [Header("Savable Status.")]
        [ReadOnlyInspector] public bool isInteracted;

        [Header("Refs.")]
        [ReadOnlyInspector] public MainHudManager _mainHudManager;
        [ReadOnlyInspector] public StateManager _states;
        
        public abstract void OnInteract();

        public virtual void OnAnimEventInteract()
        {
        }

        public abstract void SetInteractionContent();

        public abstract void OnShowInteractedResult();
        
        #region Late Init.
        protected void Base_Late_Init()
        {
            _states = SessionManager.singleton._states;
            _mainHudManager = _states._mainHudManager;

            gameObject.layer = _states._layerManager.playerInteractableLayer;
        }

        public abstract void Late_Init();
        #endregion

        #region Serialization.
        public SavableInteractionState SaveInteractionStateToSave()
        {
            SavableInteractionState _savableInteractionState = new SavableInteractionState();
            _savableInteractionState.isInteracted = isInteracted;
            _savableInteractionState.interactionId = interactionId;
            return _savableInteractionState;
        }

        public void LoadInteractionStateFromSave(SavableInteractionState _savableInteractionState)
        {
            if (_savableInteractionState.isInteracted)
            {
                isInteracted = true;
                OnShowInteractedResult();
            }
        }
        #endregion

        public enum InteractionTypeEnum
        {
            Pickup,
            Talk,
            OpenDoor,
            OpenChest,
            TakeChest,
            Rest,
            Ignite,
        }

        public enum InteractionQuitTypeEnum
        {
            timelimit,
            userInput
        }
    }
}