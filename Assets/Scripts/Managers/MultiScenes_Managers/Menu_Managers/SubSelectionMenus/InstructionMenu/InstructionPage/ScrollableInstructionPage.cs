using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SA
{
    public class ScrollableInstructionPage : RegularInstructionPage
    {
        [Header("Scroll bar (Drops).")]
        public Scrollbar scrollbar;

        [Header("ScrollWheel Speed.")]
        public float ScrollSensitivity = 0.1f;

        public override void OnCurrentPage()
        {
            scrollbar.value = 1;
            ShowPage();
        }

        public void IncrementScrollbarValue()
        {
            scrollbar.value += ScrollSensitivity;
        }

        public void DecrementScrollbarValue()
        {
            scrollbar.value -= ScrollSensitivity;
        }
    }
}