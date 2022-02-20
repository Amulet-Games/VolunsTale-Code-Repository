using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InstructionMenuManager : MonoBehaviour, IMenuManagerUpdatable
    {
        [Header("Slots (Drop).")]
        public BaseInstructionSlot[] instructionSlots;

        [Header("Pages (Drop).")]
        public CanvasGroup instructionPageGroup;

        [Header("Instruction Menu (Drop).")]
        [SerializeField] CanvasGroup instructionMenuGroup;
        [SerializeField] Canvas instructionMenuCanvas;

        [Header("Menu Tween.")]
        public LeanTweenType _menuEaseType = LeanTweenType.easeOutQuint;
        public float _menuFadeInSpeed = 0.4f;
        public float _menuFadeOutSpeed = 0.4f;

        [Header("Slot Tween.")]
        public LeanTweenType _slotEaseType = LeanTweenType.easeOutCirc;
        public float _onSlotClickChangeColorSpeed = 0.1f;
        public Color _slotHoverColor;
        public Color _slotClickInColor;
        public Color _slotClickOutColor;

        [Header("Page Tween.")]
        public LeanTweenType _pageEaseType = LeanTweenType.easeOutCirc;
        public float _pageFadeSpeed;

        [Header("Ref.")]
        [ReadOnlyInspector, SerializeField] BaseInstructionSlot currentSlot;
        [ReadOnlyInspector, SerializeField] BaseInstructionSlot currentSelectedSlot;
        [ReadOnlyInspector] public RegularInstructionPage currentInstructPage;
        [ReadOnlyInspector] public ScrollableInstructionPage currentScrollableInstructPage;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] int menuIndex;
        [ReadOnlyInspector] public bool isCursorOverSelection;
        
        [Header("Managers Refs.")]
        [ReadOnlyInspector, SerializeField] InputManager _inp;

        #region Private.
        int slotsLength;
        int _cur_instructionMenuTweenId;
        int _cur_slotTweenId;
        int _cur_pageTweenId;
        #endregion
        
        #region Tick.
        public void Tick()
        {
            GetCurrentSlotByInput();

            QuitInstructionMenuByInput();

            SelectCurrentSlotByInput();

            SelectCurrentSlotByCursor();

            ScrollPageByScrollWheel();
        }

        void GetCurrentSlotByInput()
        {
            if (_inp.menu_up_input)
            {
                menuIndex--;
                menuIndex = (menuIndex < 0) ? slotsLength - 1 : menuIndex;
                
                if (instructionSlots[menuIndex].isSelected)
                {
                    menuIndex--;
                    menuIndex = (menuIndex < 0) ? slotsLength - 1 : menuIndex;
                }

                SetCurrentSlot();
            }
            else if (_inp.menu_down_input)
            {
                menuIndex++;
                menuIndex = (menuIndex == slotsLength) ? 0 : menuIndex;

                if (instructionSlots[menuIndex].isSelected)
                {
                    menuIndex++;
                    menuIndex = (menuIndex == slotsLength) ? 0 : menuIndex;
                }

                SetCurrentSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(BaseInstructionSlot _targetSlot)
        {
            isCursorOverSelection = true;
            if (currentSlot != _targetSlot)
            {
                menuIndex = _targetSlot.slotNumber;
                SetCurrentSlot();
            }
        }

        void SetCurrentSlot()
        {
            currentSlot.OffCurrentSlot();
            currentSlot = instructionSlots[menuIndex];
            currentSlot.OnCurrentSlot();
        }

        void QuitInstructionMenuByInput()
        {
            if (_inp.menu_quit_input)
            {
                _inp.SetIsInInstructionMenuStatus(false);
            }
        }
        
        void SelectCurrentSlotByInput()
        {
            if (_inp.menu_select_input)
            {
                OnSlotSelect();
            }
        }

        void SelectCurrentSlotByCursor()
        {
            if (_inp.menu_select_mouse)
            {
                if (isCursorOverSelection)
                {
                    OnSlotSelect();
                }
            }
        }

        void ScrollPageByScrollWheel()
        {
            if (currentInstructPage.isScrollable)
            {
                if (_inp.gen_mouse_scroll > 0.01f) // forward
                {
                    currentScrollableInstructPage.IncrementScrollbarValue();
                }
                else if (_inp.gen_mouse_scroll < -0.01f) // backward
                {
                    currentScrollableInstructPage.DecrementScrollbarValue();
                }
            }
        }

        void OnSlotSelect()
        {
            CancelCurrentSlotSelectTweens();
            CanacelCurrentPageTween();

            currentSelectedSlot.OffSelectSlot();
            currentSelectedSlot = currentSlot;
            currentSelectedSlot.OnSelectSlot();

            /// Tween slot's color.
            OnSlotSelectTween();

            /// Tween Page (current Page is set after the page group went 0).
            TweenPageFadeOut();
        }
        #endregion

        #region Open Instruction Menu.
        public void OpenInstructionMenu()
        {
            OnMenuOpenSetStatus();
            OnMenuOpenSetFirstSlot();
            ShowInstructionMenu();
            OnMenuOpenSetFirstSelectSlot();
            OnMenuOpenSetCurrentPage();
        }

        void OnMenuOpenSetStatus()
        {
            menuIndex = 0;
            isCursorOverSelection = false;
        }

        void OnMenuOpenSetFirstSlot()
        {
            currentSlot = instructionSlots[0];
            currentSlot.OnCurrentSlot();
        }

        void OnMenuOpenSetFirstSelectSlot()
        {
            currentSelectedSlot = currentSlot;
            currentSelectedSlot.OnFirstSelectSlot();
        }

        void OnMenuOpenSetCurrentPage()
        {
            currentSelectedSlot.SetCurrentInstructPage();
            currentInstructPage.OnCurrentPage();
        }
        #endregion

        #region Close Instruction Menu.
        void OnInstructionMenuClose()
        {
            DeselectCurrentSlot();
            HideCurrentInstructPage();
            DisableCanvas();
        }

        void DeselectCurrentSlot()
        {
            currentSelectedSlot.OffSelectSlot();
        }

        void HideCurrentInstructPage()
        {
            currentInstructPage.OffCurrentPage();
        }
        #endregion

        #region Show / Hide Instruction Menu.
        void ShowInstructionMenu()
        {
            CheckUnFinishTweeningJob();

            EnableCanvas();
            _cur_instructionMenuTweenId = LeanTween.alphaCanvas(instructionMenuGroup, 1, _menuFadeInSpeed).setEase(_menuEaseType).id;
        }

        public void HideInstructionMenu()
        {
            CheckUnFinishTweeningJob();
            _cur_instructionMenuTweenId = LeanTween.alphaCanvas(instructionMenuGroup, 0, _menuFadeOutSpeed).setEase(_menuEaseType).setOnComplete(OnInstructionMenuClose).id;
        }

        void CheckUnFinishTweeningJob()
        {
            if (LeanTween.isTweening(_cur_instructionMenuTweenId))
                LeanTween.cancel(_cur_instructionMenuTweenId);
        }

        void EnableCanvas()
        {
            instructionMenuCanvas.enabled = true;
        }

        void DisableCanvas()
        {
            instructionMenuCanvas.enabled = false;
        }
        #endregion
        
        #region Slots Tween.
        void OnSlotSelectTween()
        {
            TweenSlotColor_ToClickIn();

            void TweenSlotColor_ToClickIn()
            {
                _cur_slotTweenId = LeanTween.value(0, 1, _onSlotClickChangeColorSpeed)
                    .setEase(_slotEaseType)
                    .setOnUpdate((value) =>
                        {
                            currentSelectedSlot.highlighterImg.color = Color.Lerp(_slotHoverColor, _slotClickInColor, value);
                        }
                    ).setOnComplete(OnCompleteTween).id;
                
                void OnCompleteTween()
                {
                    currentSelectedSlot.ShowSelector();
                    TweenSlotColor_ToClickOut();
                }
            }

            void TweenSlotColor_ToClickOut()
            {
                _cur_slotTweenId = LeanTween.value(0, 1, _onSlotClickChangeColorSpeed)
                    .setEase(_slotEaseType)
                    .setOnUpdate((value) =>
                    {
                        currentSelectedSlot.highlighterImg.color = Color.Lerp(_slotClickInColor, _slotClickOutColor, value);
                    }
                    ).id;
            }
        }

        void CancelCurrentSlotSelectTweens()
        {
            if (LeanTween.isTweening(_cur_slotTweenId))
            {
                LeanTween.cancel(_cur_slotTweenId);
            }
        }
        #endregion

        #region Page Tween.
        void TweenPageFadeOut()
        {
            _cur_pageTweenId = LeanTween.alphaCanvas(instructionPageGroup, 0, _pageFadeSpeed).setEase(_pageEaseType).setOnComplete(OnCompleteFadeOut).id;

            void OnCompleteFadeOut()
            {
                currentInstructPage.OffCurrentPage();
                currentSelectedSlot.SetCurrentInstructPage();
                currentInstructPage.OnCurrentPage();

                _cur_pageTweenId = LeanTween.alphaCanvas(instructionPageGroup, 1, _pageFadeSpeed).setEase(_pageEaseType).id;
            }
        }

        void CanacelCurrentPageTween()
        {
            if (LeanTween.isTweening(_cur_pageTweenId))
            {
                LeanTween.cancel(_cur_pageTweenId);
                currentInstructPage.OffCurrentPage();
            }
        }
        #endregion

        #region Setup.
        public void Setup(InputManager _inp)
        {
            this._inp = _inp;

            SetupSelectionSlots();
        }

        void SetupSelectionSlots()
        {
            slotsLength = instructionSlots.Length;
            for (int i = 0; i < slotsLength; i++)
            {
                instructionSlots[i]._instructionMenuManager = this;
                instructionSlots[i].slotNumber = i;
            }
        }
        #endregion

        /// ON DEATH.
        public void OnDeathOffMenuManager()
        {
            _inp.SetIsInInstructionMenuStatus(false);
        }
    }
}