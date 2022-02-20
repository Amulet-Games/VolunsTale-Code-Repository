using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public interface ISystemMenuDetailUpdatable
    {
        void OnDetailOpen();

        void OnDetailClose();

        void Tick();
    }

    [System.Serializable]
    public class EmptyDetailUpdatable : ISystemMenuDetailUpdatable
    {
        public void OnDetailClose()
        {
        }

        public void OnDetailOpen()
        {
        }

        public void Tick()
        {
        }
    }

    public class SystemMenuManager : MonoBehaviour
    {
        #region Inputs.
        [Header("Inputs.")]
        [ReadOnlyInspector] public bool menu_escape_input;
        [ReadOnlyInspector] public bool menu_up_input;
        [ReadOnlyInspector] public bool menu_down_input;
        [ReadOnlyInspector] public bool menu_left_input;
        [ReadOnlyInspector] public bool menu_right_input;
        [ReadOnlyInspector] public bool menu_scroll_fwd_input;
        [ReadOnlyInspector] public bool menu_scroll_bwd_input;
        [ReadOnlyInspector] public bool menu_select_input;

        [Header("Extra Inputs.")]
        /// Extra Inputs is for slider slots.
        [ReadOnlyInspector] public bool menu_left_input_buttonPressing;
        [ReadOnlyInspector] public bool menu_right_input_buttonPressing;
        #endregion

        #region Menu Tween.
        [Header("Menu Tween Config.")]
        public CanvasGroup _menuGroup;
        public LeanTweenType _menuEaseType;
        public float _menuFadeSpeed;
        #endregion

        #region Detail Tween.
        [Header("Detail Tween Config.")]
        public CanvasGroup _systemDetailGroup;
        public LeanTweenType _detailEaseType;
        public float _detailFadeSpeed;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public bool _isUpdateExtraInputs;
        [ReadOnlyInspector] public bool _isMenuOpening;
        #endregion

        [Header("Slot Color Config.")]
        public Color _normalColor;
        public Color _hoveringColor;
        public Color _pressedColor;

        [Header("Dual Option Sprite.")]
        public Sprite _optionsNotAvabiableSprite;
        public Sprite _1stOptionNormalSprite;
        public Sprite _1stOptionHoveringSprite;
        public Sprite _2ndOptionNormalSprite;
        public Sprite _2ndOptionHoveringSprite;

        #region Drag and Drop Refs.
        [Header("Drag and Drop.")]
        public HorizontalScrollHandler _horizontalScrollHandler;
        #endregion

        #region Refs.
        [Header("Refs.")]
        [ReadOnlyInspector] public BaseSystemDetail _currentSystemDetail;
        public ISystemMenuDetailUpdatable _currentDetailUpdatable;
        [ReadOnlyInspector] public EmptyDetailUpdatable _emptyDetailUpdatable;
        [ReadOnlyInspector] public Canvas _menuCanvas;
        #endregion

        #region Non Serialized.
        int _menuTweenId;
        #endregion

        void Start()
        {
            SetupCanvas();
            SetupHorizontalScrollHandler();
            SetupDetailUpdatable();
        }

        void Update()
        {
            UpdateInputs();

            ShowHideMenuByInput();

            _horizontalScrollHandler.Tick();

            _currentDetailUpdatable.Tick();
        }

        #region Tick.
        void UpdateInputs()
        {
            menu_escape_input = Input.GetButtonDown("escape");
            menu_up_input = Input.GetButtonDown("dual_menu_up");
            menu_down_input = Input.GetButtonDown("dual_menu_down");
            menu_left_input = Input.GetButtonDown("dual_menu_left");
            menu_right_input = Input.GetButtonDown("dual_menu_right");
            menu_scroll_fwd_input = Input.GetButtonDown("menu_select");
            menu_scroll_bwd_input = Input.GetButtonDown("menu_quit");
            menu_select_input = Input.GetButtonDown("menu_switch");
            
            if (_isUpdateExtraInputs)
            {
                menu_left_input_buttonPressing = Input.GetButton("dual_menu_left");
                menu_right_input_buttonPressing = Input.GetButton("dual_menu_right");
            }
        }
        
        void ShowHideMenuByInput()
        {
            if (menu_escape_input)
            {
                if (!_isMenuOpening)
                {
                    _isMenuOpening = true;
                    menu_escape_input = false;
                    /// On Before Menu Open.
                    _horizontalScrollHandler.OnBeforeMenuOpen();

                    /// Show Menu.
                    ShowSystemMenu();

                    /// Show Detail.
                    OnMenuOpenShowCurrentDetailNoTween();
                    _currentSystemDetail.OnDetailOpen();
                }
            }
        }

        public void DisableExtraInputs()
        {
            _isUpdateExtraInputs = false;

            menu_left_input_buttonPressing = false;
            menu_right_input_buttonPressing = false;
        }

        public void CloseMenuByInput()
        {
            if (menu_escape_input)
            {
                if (_isMenuOpening)
                {
                    _isMenuOpening = false;
                    menu_escape_input = false;

                    _currentDetailUpdatable = _emptyDetailUpdatable;
                    HideSystemMenu();
                }
            }
        }
        #endregion

        #region Set Current System Detail.
        public void SetCurrentSystemDetail(BaseSystemDetail _systemDetail)
        {
            _currentSystemDetail = _systemDetail;
        }
        #endregion

        #region Detail Tween.
        public void FadeOutCurrentDetail()
        {
            _currentSystemDetail.OnDetailClose();

            Canvas _prevDetailCanvas = _currentSystemDetail._systemDetailCanvas;
            LeanTween.alphaCanvas(_systemDetailGroup, 0, _detailFadeSpeed).setEase(_detailEaseType).setOnComplete(OnCompleteDisableCanvas);

            void OnCompleteDisableCanvas()
            {
                _prevDetailCanvas.enabled = false;
            }
        }

        public void FadeInCurrentDetail()
        {
            _currentSystemDetail.OnDetailOpen();

            EnableDetailCanvas();
            LeanTween.alphaCanvas(_systemDetailGroup, 1, _detailFadeSpeed).setEase(_detailEaseType);
        }

        void OnMenuOpenShowCurrentDetailNoTween()
        {
            _systemDetailGroup.alpha = 1;
            EnableDetailCanvas();
        }

        void OnMenuCloseHideCurrentDetailNoTween()
        {
            _systemDetailGroup.alpha = 0;
            DisableDetailCanvas();
        }

        void EnableDetailCanvas()
        {
            _currentSystemDetail._systemDetailCanvas.enabled = true;
        }

        void DisableDetailCanvas()
        {
            _currentSystemDetail._systemDetailCanvas.enabled = false;
        }
        #endregion

        #region Menu Tween.
        void ShowSystemMenu()
        {
            CheckCancelTween();
            EnableCanvas();

            _menuTweenId = LeanTween.alphaCanvas(_menuGroup, 1, _menuFadeSpeed).setEase(_menuEaseType).id;
        }

        void HideSystemMenu()
        {
            CheckCancelTween();

            _menuTweenId = LeanTween.alphaCanvas(_menuGroup, 0, _menuFadeSpeed).setEase(_menuEaseType).setOnComplete(OnCompleteHideSystemMenu).id;

            void OnCompleteHideSystemMenu()
            {
                DisableCanvas();

                /// On After Menu Close.
                _horizontalScrollHandler.OnAfterMenuClose();
                OnMenuCloseHideCurrentDetailNoTween();
            }
        }

        void EnableCanvas()
        {
            _menuCanvas.enabled = true;
        }

        void DisableCanvas()
        {
            _menuCanvas.enabled = false;
        }

        void CheckCancelTween()
        {
            LeanTween.cancel(_menuTweenId);
        }
        #endregion

        #region Setup.
        void SetupCanvas()
        {
            _menuCanvas = GetComponent<Canvas>();
        }

        void SetupHorizontalScrollHandler()
        {
            _horizontalScrollHandler.Setup(this);
        }

        void SetupDetailUpdatable()
        {
            _emptyDetailUpdatable = new EmptyDetailUpdatable();
            _currentDetailUpdatable = _emptyDetailUpdatable;
        }
        #endregion
    }
}