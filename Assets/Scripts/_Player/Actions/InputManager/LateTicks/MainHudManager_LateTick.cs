using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/MainHud Manager LateTick.")]
    public class MainHudManager_LateTick : MonoAction
    {
        public override void Execute(StateManager states)
        {
            states._inp._mainHudManager.LateTick();
        }
    }
}