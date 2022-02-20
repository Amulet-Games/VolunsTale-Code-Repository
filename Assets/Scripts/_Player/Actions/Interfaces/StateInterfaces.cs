using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class StateInterfaces
    {
    }

    public interface INeglectInputAction
    {
        void Execute(StateManager _states);
    }
}