using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "State Actions/Player/FixedTicks/Move With Player While Neglect")]
    public class MoveWithPlayerWhileNeglect : MoveWithPlayer
    {
        public override void Execute(StateManager states)
        {
            states.MoveWithPlayerWhileNeglect();
        }
    }
}