using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Monitor Combining Skinned Mesh")]
    public class MonitorCombiningSkinnedMesh : MonoAction
    {
        public override void Execute(StateManager states)
        {
            states.CombineSkinnedMesh();
        }
    }
}