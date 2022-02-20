using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SecondPhaseChangeMMH
namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/KMA_MotionMonitorHandler/SecondPhaseChangeMMH")]
    public class SecondPhaseChangeMMH : ScriptableObject, I_KMA_MotionMonitorHandler
    {
        public void KMA_WaitTick(EgilKinematicMotionAttackMod _egil_KMA_Mod)
        {
            _egil_KMA_Mod.SecondPhaseChange_KMA_WaitTick();
        }
    }
}