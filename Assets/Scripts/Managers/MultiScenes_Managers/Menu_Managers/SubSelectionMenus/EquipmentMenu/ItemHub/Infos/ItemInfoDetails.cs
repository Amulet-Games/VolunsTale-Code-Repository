using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ItemInfoDetails : MonoBehaviour
    {
        [Header("Canvas (Drops.)")]
        [SerializeField] protected Canvas infoCanvas;

        public void ShowInfoDetails()
        {
            infoCanvas.enabled = true;
        }

        public void HideInfoDetails()
        {
            infoCanvas.enabled = false;
        }
    }
}