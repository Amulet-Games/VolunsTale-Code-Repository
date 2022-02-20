using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LevelingCheckpointOption : BaseCheckpointOption
    {
        public override void OnSelectOption()
        {
            checkpointMenuManager.OnLevelingOptionSelected();
        }
    }
}