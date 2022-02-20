using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class ItemAlterDetails : MonoBehaviour
    {
        [Header("Canvas (Drops.)")]
        [SerializeField] protected Canvas alterCanvas;

        public void ShowAlterDetails()
        {
            alterCanvas.enabled = true;
        }

        public void HideAlterDetails()
        {
            alterCanvas.enabled = false;
        }
    }
}