using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class RegularInstructionPage : MonoBehaviour
    {
        [Header("Is Scrollable.")]
        public bool isScrollable;

        [Header("Canvas (Drops).")]
        public Canvas pageCanvas;
        
        public virtual void OnCurrentPage()
        {
            ShowPage();
        }
        
        public void OffCurrentPage()
        {
            HidePage();
        }

        #region Show / Hide Page.
        protected void ShowPage()
        {
            pageCanvas.enabled = true;
        }

        protected void HidePage()
        {
            pageCanvas.enabled = false;
        }
        #endregion
    }
}