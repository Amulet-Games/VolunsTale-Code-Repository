using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Actions/AI Multi Stage Attacks/MSA KMA")]
    public class MSA_KMA : MSA_BaseAICombo
    {
        [Header("KMA Config.")]
        public _KMA_ActionData _KMA_profile;

        public override void Execute(AIManager ai)
        {
            if (!CheckIfNextAIComboAvaliable())
                return;

            ai.isPausingTurnWithAgent = true;

            ai.Execute_KMJ(_KMA_profile);

            // Camera effects
            ai.aIStates.applyControllerCameraYMovement = applyControllerCameraYMovement;
            ai.aIStates.applyControllerCameraZoom = applyControllerCameraZoom;
            
            /// Local Funcs.
            bool CheckIfNextAIComboAvaliable()
            {
                if (ai.GetIsEnemyTiredBool())
                {
                    return false;
                }

                return true;
            }
        }
    }
}