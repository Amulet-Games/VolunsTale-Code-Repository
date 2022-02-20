using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/LateTicks/MonitorInteractables")]
    public class MonitorInteractables : StateActions
    {
        #region Overlap Sphere Configs.
        [SerializeField] LayerMask playerInteractionlayer;
        [SerializeField] private float overlapSphereRadius = 1f;
        [SerializeField] private float monitorRate = 2f;
        #endregion
        
        #region Interactables Refs.
        [NonSerialized] float monitorTimer = 0;
        #endregion
        
        public override void Execute(StateManager _states)
        {
            if (!_states._isPausingSearchInteractables && _states._isInMainHud)
            {
                MonitorNearInteractables();
                _states.HandleFoundInteractables();
            }

            void MonitorNearInteractables()
            {
                monitorTimer += _states._delta;
                if (monitorTimer >= monitorRate)
                {
                    monitorTimer = 0;
                    _states.ClearFoundInteractables();

                    int totalHitNum = Physics.OverlapSphereNonAlloc(_states.mTransform.position, overlapSphereRadius, _states.interactHitColliders, playerInteractionlayer);
                    if (totalHitNum > 0)
                    {
                        for (int i = 0; i < totalHitNum; i++)
                        {
                            _states.AddToFoundInteractables(i);
                        }
                    }
                    else
                    {
                        _states.SetPotentialInteractableToNull();
                    }
                }
            }
        }
        
        public override void AIExecute(AIStateManager aIState)
        {
        }
    }
}