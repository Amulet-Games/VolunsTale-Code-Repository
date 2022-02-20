using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace SA
{
    public class SelectionMenuManager : MonoBehaviour, IMenuManagerUpdatable
    {
        [Header("Slots (Drop).")]
        public BaseSelectionSlot[] selectionSlots = new BaseSelectionSlot[4];

        [Header("Selection Menu (Drop).")]
        [SerializeField] CanvasGroup selectionMenuGroup;
        [SerializeField] Canvas _selectionMenuCanvas;

        [Header("TMP Text (Drop).")]
        public TMP_Text titleText;

        [Header("Menu Tween.")]
        public LeanTweenType _menuEaseType = LeanTweenType.easeOutQuint;
        public float _menuFadeInSpeed = 0.4f;
        public float _menuFadeOutSpeed = 0.4f;

        [Header("Slot Tween.")]
        public LeanTweenType _slotEaseType = LeanTweenType.easeOutSine;
        public float _slotHighlightFadeSpeed = 1;
        public float _slotPingPongMinValue = 0.2f;
        public float _slotPingPongMaxValue = 0.6f;

        [Header("Status.")]
        [ReadOnlyInspector, SerializeField] BaseSelectionSlot currentSlot;
        [ReadOnlyInspector, SerializeField] int menuIndex;
        [ReadOnlyInspector] public bool isCursorOverSelection;

        [Header("Managers Refs.")]
        [ReadOnlyInspector, SerializeField] InputManager _inp;

        #region Private.
        int slotsLength;
        int _cur_selectionMenuTweenId;
        int _cur_slotTweenId;
        #endregion

        #region Tick.
        public void Tick()
        {
            GetCurrentSlotByInput();

            QuitSelectionMenuByInput();

            SelectCurrentSlotByInput();

            SelectCurrentSlotByCursor();

            currentSlot.Tick();
        }

        void GetCurrentSlotByInput()
        {
            if (_inp.menu_right_input)
            {
                menuIndex++;
                menuIndex = (menuIndex == slotsLength) ? 0 : menuIndex;

                SetCurrentSlot();
            }
            else if (_inp.menu_left_input)
            {
                menuIndex--;
                menuIndex = (menuIndex < 0) ? slotsLength - 1 : menuIndex;

                SetCurrentSlot();
            }
        }

        /// Pointer Event.
        public void GetCurrentSlotByPointerEvent(BaseSelectionSlot _targetSlot)
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
            currentSlot = selectionSlots[menuIndex];
            currentSlot.OnCurrentSlot();
        }

        void QuitSelectionMenuByInput()
        {
            if (_inp.menu_quit_input)
            {
                _inp.menu_quit_input = false;
                QuitSelectionMenu_To_MainHud();
            }
        }

        void SelectCurrentSlotByInput()
        {
            if (_inp.menu_select_input)
            {
                currentSlot.OnSelectSlot();
            }
        }

        void SelectCurrentSlotByCursor()
        {
            if (_inp.menu_select_mouse)
            {
                if (isCursorOverSelection)
                {
                    currentSlot.OnSelectSlot();
                }
            }
        }
        #endregion

        #region Open Selection Menu.
        public void OpenSelectionMenu()
        {
            OnMenuOpenSetStatus();
            OnMenuOpenSetFirstSlot();
            ShowSelectionMenu();
        }

        void OnMenuOpenSetStatus()
        {
            menuIndex = 0;
            isCursorOverSelection = false;
        }

        void OnMenuOpenSetFirstSlot()
        {
            currentSlot = selectionSlots[0];
            currentSlot.OnCurrentSlot();
        }
        #endregion

        #region Show / Hide Selection Menu.
        void ShowSelectionMenu()
        {
            CheckUnFinishTweeningJob();

            EnableCanvas();
            _cur_selectionMenuTweenId = LeanTween.alphaCanvas(selectionMenuGroup, 1, _menuFadeInSpeed).setEase(_menuEaseType).id;
        }

        public void HideSelectionMenu()
        {
            CheckUnFinishTweeningJob();
            _cur_selectionMenuTweenId = LeanTween.alphaCanvas(selectionMenuGroup, 0, _menuFadeOutSpeed).setEase(_menuEaseType).setOnComplete(DisableCanvas).id;
        }

        void CheckUnFinishTweeningJob()
        {
            if (LeanTween.isTweening(_cur_selectionMenuTweenId))
                LeanTween.cancel(_cur_selectionMenuTweenId);
        }

        void EnableCanvas()
        {
            _selectionMenuCanvas.enabled = true;
        }

        void DisableCanvas()
        {
            _selectionMenuCanvas.enabled = false;
        }
        #endregion

        #region Quit Selection Menu.

        #region Equipment Menu.
        public void QuitSelectionMenu_To_EquipmentMenu()
        {
            _inp.SetIsInEquipmentMenuStatus(true);
        }
        #endregion

        #region System Menu.
        public void QuitSelectionMenu_ExitGame()
        {
            HideMenuBeforeQuitGame();
            ResetStatusBeforeQuiteGame();

            SessionManager.singleton.QuitGame_UnLoadLevel();

            void HideMenuBeforeQuitGame()
            {
                DisableCanvas();
                selectionMenuGroup.alpha = 0;
            }

            void ResetStatusBeforeQuiteGame()
            {
                _inp._mainHudManager.damagePreviewer.OnQuitGame();
            }
        }
        #endregion

        #region Instruction Menu.
        public void QuitSelectionMenu_To_InstructionMenu()
        {
            _inp.SetIsInInstructionMenuStatus(true);
        }
        #endregion

        #region Inventory / Status Menu.
        public void ResetToFirstSlot()
        {
            currentSlot = selectionSlots[0];
            currentSlot.OnCurrentSlot();
        }
        #endregion

        #region Main Hud.
        void QuitSelectionMenu_To_MainHud()
        {
            currentSlot.OffCurrentSlot();
            _inp.SetIsInSelectionMenuStatus(false);
            _inp.SetIsInMainHudStatus(true);
        }
        #endregion

        #endregion

        #region Slots Tween.
        public void CurrentSlotPingPongTween()
        {
            _cur_slotTweenId = LeanTween.alpha(currentSlot._highlighterRect, _slotPingPongMinValue, _slotHighlightFadeSpeed).setLoopPingPong(1).setEase(_slotEaseType).setOnComplete(currentSlot.RequestNewPingPongLoop).id;
        }

        public void CancelCurrentSlotHighlighterTween()
        {
            LeanTween.cancel(_cur_slotTweenId);
            LeanTween.alpha(currentSlot._highlighterRect, _slotPingPongMaxValue, 0);
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
            slotsLength = selectionSlots.Length;
            for (int i = 0; i < slotsLength; i++)
            {
                selectionSlots[i]._selectionMenuManager = this;
                selectionSlots[i].slotNumber = i;
            }
        }
        #endregion
        
        /// ON DEATH.
        public void OnDeathOffMenuManager()
        {
            _inp.SetIsInSelectionMenuStatus(false);
        }
    }
}