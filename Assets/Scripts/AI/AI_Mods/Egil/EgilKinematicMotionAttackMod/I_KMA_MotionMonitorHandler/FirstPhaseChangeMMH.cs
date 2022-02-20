using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/KMA_MotionMonitorHandler/FirstPhaseChangeMMH")]
    public class FirstPhaseChangeMMH : ScriptableObject, I_KMA_MotionMonitorHandler
    {
        public void KMA_WaitTick(EgilKinematicMotionAttackMod _egil_KMA_Mod)
        {
            _egil_KMA_Mod.FirstPhaseChange_KMA_WaitTick();
        }
    }
}