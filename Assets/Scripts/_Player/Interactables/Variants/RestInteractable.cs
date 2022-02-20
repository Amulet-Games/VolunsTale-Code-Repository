using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RestInteractable : PlayerInteractable
    {
        public Transform bonfireSeatTrans;
        public int _linkedSpawnPointId;

        [Header("Drag and Drop Refs.")]
        public Collider restTriggerCollider = null;
        public IgniteInteractable igniteInteractable;
        [ReadOnlyInspector] public SecuredCamp_CommentaryZone securedCampCommentZone;
        [ReadOnlyInspector] public PlayerSpawnPoint _linkedSpawnPoint;

        public override void Late_Init()
        {
            Base_Late_Init();

            Init_LayerByInteractedStatus();
            Init_GetLinkedSpawnPoint();

            void Init_LayerByInteractedStatus()
            {
                if (isInteracted)
                {
                    gameObject.layer = _states._layerManager.playerInteractableLayer;
                }
            }

            void Init_GetLinkedSpawnPoint()
            {
                _linkedSpawnPoint = LevelManager.singleton.Get_WF_SpawnPointFromDict(_linkedSpawnPointId);
            }
        }

        public override void SetInteractionContent()
        {
            _mainHudManager.SetNextInterContent_Rest();
        }

        public override void OnInteract()
        {
            _states._currentWalkTowardRestInteractable = this;
            _states.OnInteractWithRestInteractable();
        }

        public void TurnOnInteraction_ByIgnite()
        {
            isInteracted = true;

            gameObject.layer = _states._layerManager.playerInteractableLayer;
            igniteInteractable.ActivateBonfireParticles();
            restTriggerCollider.enabled = true;
            enabled = true;

            securedCampCommentZone.OnCampSecured();
        }

        void TurnOnInteraction_BySavedState()
        {
            /// Don't put any "_state" reference code here!
            /// Don't put any "OnCampSecured" reference code here!
            igniteInteractable.ActivateBonfireParticles();
            restTriggerCollider.enabled = true;
            enabled = true;
        }

        void TurnOffInteraction()
        {
            gameObject.layer = _states._layerManager.playerInteractableLayer;
            igniteInteractable.DeactivateBonfireParticles();
            restTriggerCollider.enabled = true;
            enabled = true;
        }
        
        public override void OnShowInteractedResult()
        {
            TurnOnInteraction_BySavedState();
        }
    }
}