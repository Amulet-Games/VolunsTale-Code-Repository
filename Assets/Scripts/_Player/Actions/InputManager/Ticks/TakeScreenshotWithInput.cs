using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "Mono Actions/Take Screenshot With Input")]
    public class TakeScreenshotWithInput : MonoAction
    {
        public int _shotIndex = 0;

        public override void Execute(StateManager states)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                //ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
                ScreenCapture.CaptureScreenshot($"Screenshot{_shotIndex}.png", 2);
                _shotIndex++;
            }
        }
    }
}