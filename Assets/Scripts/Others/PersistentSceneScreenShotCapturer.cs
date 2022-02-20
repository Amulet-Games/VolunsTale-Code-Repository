using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSceneScreenShotCapturer : MonoBehaviour
{
    private int _shotIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            //ScreenshotHandler.TakeScreenshot_Static(Screen.width, Screen.height);
            ScreenCapture.CaptureScreenshot($"Screenshot{_shotIndex}.png", 2);
            _shotIndex++;
        }
    }
}
