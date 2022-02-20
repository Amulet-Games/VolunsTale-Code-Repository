using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class BFX_Handler_SetBase : BFX_Handler
    {
        #region HideInInspectors.
        /// DecalUpdater Propery IDs
        [ReadOnlyInspector] public int cutoutPropertyID;
        [ReadOnlyInspector] public int forwardDirPropertyID;
        #endregion
    }
}