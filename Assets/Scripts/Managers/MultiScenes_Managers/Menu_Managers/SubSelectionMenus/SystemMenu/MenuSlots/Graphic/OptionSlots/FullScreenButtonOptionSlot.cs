using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class FullScreenButtonOptionSlot : ButtonOptionSlot
    {
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isFullScreenWindowed;

        #region On Button Select.
        public override void OnButtonSelect()
        {
            base.OnButtonSelect();
            SwitchFullScreenMode();
        }

        void SwitchFullScreenMode()
        {
            if (_isFullScreenWindowed)
            {
                SetScreenModeToMinimizeWindow();
            }
            else
            {
                SetScreenModeToFullScreenWindow();
            }
        }
        #endregion

        #region Set Screen Mode.
        void SetScreenModeToFullScreenWindow()
        {
            FitScreenAndResolutionToFullScreenWindow();
            SetOptionSlotStatus();
            
            void FitScreenAndResolutionToFullScreenWindow()
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
            }

            void SetOptionSlotStatus()
            {
                _isFullScreenWindowed = true;
                _checkerImage.SetActive(true);
            }
        }

        void SetScreenModeToMinimizeWindow()
        {
            FitScreenAndResolutionToMinimizeWindow();
            SetOptionSlotStatus();

            void FitScreenAndResolutionToMinimizeWindow()
            {
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

                Resolution currentResolution = Screen.currentResolution;
                Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.Windowed);
            }

            void SetOptionSlotStatus()
            {
                _isFullScreenWindowed = false;
                _checkerImage.SetActive(false);
            }
        }
        #endregion

        #region Setup.
        public override void Setup(int _slotIndex, BaseSystemDetail _systemDetail)
        {
            base.Setup(_slotIndex, _systemDetail);

            SetupFullScreen();
        }

        void SetupFullScreen()
        {
            _isFullScreenWindowed = Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? true : false;
            _checkerImage.SetActive(_isFullScreenWindowed);
        }
        #endregion
    }
}