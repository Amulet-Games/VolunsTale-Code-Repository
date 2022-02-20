using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Mods/KMA_MotionMonitorHandler/Default_KMA_MMH")]
    public class Default_KMA_MMH : ScriptableObject, I_KMA_MotionMonitorHandler
    {
        public void KMA_WaitTick(EgilKinematicMotionAttackMod _egil_KMA_Mod)
        {
            _egil_KMA_Mod.Defualt_KMA_WaitTick();
        }
    }
}
