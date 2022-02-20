using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class RuntimeItem : MonoBehaviour
    {
        [Header("SavableInventory.")]
        [ReadOnlyInspector] public string runtimeName;
        [ReadOnlyInspector] public int slotNumber;

        [Header("Savable.")]
        [ReadOnlyInspector] public int uniqueId;

        public static int uniqueIdGenerator = -1;

        protected void InitRuntimeItem()
        {
            uniqueIdGenerator++;
            uniqueId = uniqueIdGenerator;
        }

        public abstract Item GetReferedItem();
    }
}