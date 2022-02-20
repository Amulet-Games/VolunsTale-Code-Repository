using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Selection Menus Tick")]
    public class SelectionMenus_Tick : MonoAction
    {
        public override void Execute(StateManager states)
        {
            states._inp.MenuManagersTick();
        }
    }
}