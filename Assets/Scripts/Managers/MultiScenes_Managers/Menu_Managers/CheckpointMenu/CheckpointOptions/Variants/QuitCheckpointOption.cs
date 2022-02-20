using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class QuitCheckpointOption : BaseCheckpointOption
    {
        public override void OnSelectOption()
        {
            checkpointMenuManager.OnQuitOptionSelected();
        }
    }
}