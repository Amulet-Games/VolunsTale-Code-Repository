using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public interface IMenuManagerUpdatable
    {
        void Tick();
        void OnDeathOffMenuManager();
    }

    public class InputManager : MonoBehaviour
    {
        #region Mono Actions.
        public MonoAction[] fixedTick_Actions;
        public MonoAction[] tick_Actions;
        public MonoAction[] lateTick_Actions;
        #endregion

        #region Current Input Type.
        [SerializeField, ReadOnlyInspector]
        InputTypeEnum currentInputType = InputTypeEnum.Main;
        #endregion

        #region General Inputs.
        [ReadOnlyInspector]
        public float gen_mouse_scroll;
        #endregion

        #region Menu Inputs
        [ReadOnlyInspector]
        public bool menu_up_input;
        [ReadOnlyInspector]
        public bool menu_down_input;
        [ReadOnlyInspector]
        public bool menu_right_input;
        [ReadOnlyInspector]
        public bool menu_left_input;
        [ReadOnlyInspector]
        public Vector2 menu_mouse_position;

        [ReadOnlyInspector]
        public bool menu_select_mouse;
        [ReadOnlyInspector]
        public bool menu_select_input;
        [ReadOnlyInspector]
        public bool menu_remove_input;
        [ReadOnlyInspector]
        public bool menu_quit_input;    //included escape button.
        [ReadOnlyInspector]
        public bool menu_switch_input;
        [ReadOnlyInspector]
        public bool menu_hide_input;
        #endregion

        #region Bools
        /// Selection Menus.
        [ReadOnlyInspector] public bool isNeglectSelectionMenu;
        [ReadOnlyInspector] public bool isInMenuManager;
        #endregion

        #region Floats.
        [ReadOnlyInspector]
        public float delta;
        [ReadOnlyInspector]
        public float fixedDelta;
        #endregion
        
        #region Refs.
        /// Core Refs.
        [ReadOnlyInspector] public StateManager _states = null;
        [ReadOnlyInspector] public CameraHandler _camHandler = null;
        [ReadOnlyInspector] public MainHudManager _mainHudManager = null;
        [ReadOnlyInspector] public CharacterRegisterWindow _charRegistWindow = null;
        [ReadOnlyInspector] public SelectionMenuManager _selectionMenuManager = null;
        [ReadOnlyInspector] public EquipmentMenuManager _equipmentMenuManager = null;
        [ReadOnlyInspector] public InstructionMenuManager _instructionMenuManager = null;
        [ReadOnlyInspector] public CheckpointMenuManager _checkpointMenuManager = null;
        [ReadOnlyInspector] public LevelingMenuManager _levelingMenuManager = null;
        public IMenuManagerUpdatable iMenuManagerUpdatable;
        #endregion
        
        #region Privates.
        int tickActionsLength;
        int fixTickActionsLength;
        int lateTickActionsLength;

        [HideInInspector] public bool showFixedTickFoldout;
        [HideInInspector] public bool showTickFoldout;
        [HideInInspector] public bool showLateTickFoldout;
        [HideInInspector] public bool showSkyboxFoldout;
        [HideInInspector] public bool showCurrentInputTypeFoldout;
        [HideInInspector] public bool showGeneralInputFoldout;
        [HideInInspector] public bool showMenuInputFoldout;
        [HideInInspector] public bool showBoolsFoldout;
        [HideInInspector] public bool showFloatsFoldout;
        [HideInInspector] public bool showRefsFoldout;
        [HideInInspector] public bool showDragAndDropRefsFoldout;
        #endregion

        #region Non Serialized.
        public StringBuilder states_strBuilder = null;
        public StringBuilder itemHub_strBuilder = null;
        public StringBuilder damagePreviewer_strBuilder = null;

        public PointerEventData _pointerEventData;
        #endregion

        void Awake()
        {
            InitStrBuilders();
            
            InitCurrentEventSystem();

            InitMainHudManagerRefs();

            InitCharRegistWindowRefs();

            InitSelectionMenuManagersRefs();

            InitCheckpointMenuManagerRefs();

            InitStateManagerRefs();
            _states.Init();
        }

        void Start()
        {
            _states.Setup();

            SetupCameraHandler();

            SetupMainHudManager();

            SetupCharRegistWindow();

            _states._setupPlayerGearActions.Execute(_states);

            SetupSelectionMenuManagers();

            SetupCheckpointMenuManagers();

            SetupEnablePlayerInputs_AfterWait();

            GetActionsLength();
        }

        void Update()
        {
            HandleDeltaTime();

            GetTicksInputsFromType();

            Tick_ActionsExecute();
            
            _states.Tick();

            _camHandler.Tick();
        }

        void FixedUpdate()
        {
            HandleFixedDeltaTime();
            
            FixedTick_ActionsExecute();

            _states.FixedTick();

            _camHandler.FixedTick();
        }

        void LateUpdate()
        {
            HandleDeltaTime();

            _states.LateTick();

            LateTick_ActionsExecute();
        }

        /// AWAKE
        
        void InitStrBuilders()
        {
            states_strBuilder = new StringBuilder();
            itemHub_strBuilder = new StringBuilder();
            damagePreviewer_strBuilder = new StringBuilder();
        }
        
        void InitCurrentEventSystem()
        {
            _pointerEventData = new PointerEventData(EventSystem.current);
        }

        void InitMainHudManagerRefs()
        {
            _mainHudManager = MenuSingletonsStack.singleton.mainHudManager;
        }
        
        void InitCharRegistWindowRefs()
        {
            _charRegistWindow = MenuSingletonsStack.singleton.charRegistWindow;
        }

        void InitSelectionMenuManagersRefs()
        {
            _selectionMenuManager = MenuSingletonsStack.singleton.selectionMenuManager;

            _equipmentMenuManager = MenuSingletonsStack.singleton.equipmentMenuManager;

            _instructionMenuManager = MenuSingletonsStack.singleton.instructionMenuManager;
        }

        void InitCheckpointMenuManagerRefs()
        {
            _checkpointMenuManager = MenuSingletonsStack.singleton.checkpointMenuManager;

            _levelingMenuManager = LevelingMenuManager.singleton;
        }

        void InitStateManagerRefs()
        {
            _states = GetComponent<StateManager>();
            _states._inp = this;
        }
        
        /// START

        void SetupCameraHandler()
        {
            _camHandler = SessionManager.singleton._camHandler;
            _camHandler.Setup(_states);
        }

        void GetActionsLength()
        {
            tickActionsLength = tick_Actions.Length;
            fixTickActionsLength = fixedTick_Actions.Length;
            lateTickActionsLength = lateTick_Actions.Length;
        }

        void SetupMainHudManager()
        {
            _mainHudManager.Setup();
            SetupIsInMainHudToTrue();

            /// This won't change current input type to 'main'.
            void SetupIsInMainHudToTrue()
            {
                _camHandler.Off_MainHud_SetInputStatus();

                _states._isInMainHud = true;
                _mainHudManager.ShowMainHud();
            }
        }
        
        void SetupCharRegistWindow()
        {
            _charRegistWindow.Setup(this);
        }

        void SetupSelectionMenuManagers()
        {
            _selectionMenuManager.Setup(this);
            _equipmentMenuManager.Setup(this);
            _instructionMenuManager.Setup(this);
        }

        void SetupCheckpointMenuManagers()
        {
            _checkpointMenuManager.Setup(this);
            _levelingMenuManager.Setup(this);
        }
        
        void SetupEnablePlayerInputs_AfterWait()
        {
            LeanTween.value(0, 1, 0.6f).setOnComplete(OnCompleteWait);

            void OnCompleteWait()
            {
                if (_states._savableManager.isNewGame)
                {
                    /// Show Character Regist Window.
                    SetIsInCharRegistWindowStatus(true);
                }
                else
                {
                    /// Enable Player Inputs.
                    currentInputType = InputTypeEnum.Main;
                }
            }
        }

        /// UPDATES

        void HandleDeltaTime()
        {
            delta = Time.deltaTime;
            _states._delta = delta;
            _camHandler._delta = delta;
            _mainHudManager._delta = delta;
        }

        void GetTicksInputsFromType()
        {
            switch (currentInputType)
            {
                case InputTypeEnum.Main:
                    
                    _states.UpdateTickInputs_Main();
                    _camHandler.UpdateInputs_Main();
                    break;

                case InputTypeEnum.SelectionMenu:
                    
                    _states.UpdateInputs_Menu();

                    UpdateInputs_SelectionMenu();
                    break;

                case InputTypeEnum.EquipmentMenu:
                    
                    _states.UpdateInputs_Menu();

                    UpdateInputs_EquipmentMenu();
                    break;

                case InputTypeEnum.InstructionMenu:

                    _states.UpdateInputs_Menu();

                    UpdateInputs_InstructionMenu();
                    break;

                case InputTypeEnum.CheckpointMenu:
                    
                    _camHandler.UpdateInputs_CheckpointMenu();
                    UpdateCheckpointInputs();
                    break;

                case InputTypeEnum.LevelingMenu:
                    
                    UpdateLevelingMenuInputs();
                    break;

                case InputTypeEnum.OnDeath:

                    _camHandler.UpdateInputs_Main();
                    break;

                case InputTypeEnum.OnPause:
                    break;
            }
        }

        void Tick_ActionsExecute()
        {
            for (int i = 0; i < tickActionsLength; i++)
            {
                tick_Actions[i].Execute(_states);
            }
        }

        /// FIX UPDATES

        void HandleFixedDeltaTime()
        {
            fixedDelta = Time.fixedDeltaTime;
            _states._fixedDelta = fixedDelta;
            _camHandler._fixedDelta = fixedDelta;
        }
        
        void FixedTick_ActionsExecute()
        {
            for (int i = 0; i < fixTickActionsLength; i++)
            {
                fixedTick_Actions[i].Execute(_states);
            }
        }

        /// LATE UPDATES
        
        void LateTick_ActionsExecute()
        {
            for (int i = 0; i < lateTickActionsLength; i++)
            {
                lateTick_Actions[i].Execute(_states);
            }
        }

        /// INPUTS METHODS
        
        void UpdateInputs_SelectionMenu()
        {
            //Menu
            menu_left_input = Input.GetButtonDown("menu_left");
            menu_right_input = Input.GetButtonDown("menu_right");

            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
            menu_quit_input = Input.GetButtonDown("menu_quit");
        }

        void UpdateInputs_EquipmentMenu()
        {
            //General
            gen_mouse_scroll = Input.GetAxis("mouse_scroll");

            //Menu
            menu_up_input = Input.GetButtonDown("menu_up");
            menu_down_input = Input.GetButtonDown("menu_down");
            menu_left_input = Input.GetButtonDown("menu_left");
            menu_right_input = Input.GetButtonDown("menu_right");

            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
            menu_remove_input = Input.GetButtonDown("menu_remove");
            menu_quit_input = Input.GetButtonDown("menu_quit");
            menu_switch_input = Input.GetButtonDown("menu_switch");
            menu_hide_input = Input.GetButtonDown("menu_hide");
        }

        void UpdateInputs_InstructionMenu()
        {
            //General
            gen_mouse_scroll = Input.GetAxis("mouse_scroll");

            //Menu
            menu_up_input = Input.GetButtonDown("menu_up");
            menu_down_input = Input.GetButtonDown("menu_down");

            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
            menu_quit_input = Input.GetButtonDown("menu_quit");
        }

        void UpdateCheckpointInputs()
        {
            menu_up_input = Input.GetButtonDown("dual_menu_up");
            menu_down_input = Input.GetButtonDown("dual_menu_down");
            menu_left_input = Input.GetButtonDown("menu_left");
            menu_right_input = Input.GetButtonDown("menu_right");

            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
            menu_switch_input = Input.GetButtonDown("menu_switch");
        }

        void UpdateLevelingMenuInputs()
        {
            menu_up_input = Input.GetButtonDown("dual_menu_up");
            menu_down_input = Input.GetButtonDown("dual_menu_down");
            menu_left_input = Input.GetButtonDown("dual_menu_left");
            menu_right_input = Input.GetButtonDown("dual_menu_right");
            menu_mouse_position = Input.mousePosition;

            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
            menu_quit_input = Input.GetButtonDown("menu_quit");
            menu_switch_input = Input.GetButtonDown("menu_switch");
            menu_hide_input = Input.GetButtonDown("menu_hide");
        }
        
        /// SET STATUS
        
        public void SetIsInMainHudStatus(bool _isInMainHud)
        {
            if (_isInMainHud)
            {
                _camHandler.Off_MainHud_SetInputStatus();

                _states._isInMainHud = true;
                currentInputType = InputTypeEnum.Main;
                _mainHudManager.ShowMainHud();
            }
            else
            {
                _states._isInMainHud = false;
                _mainHudManager.HideMainHud();
            }
        }

        public void SetIsInSelectionMenuStatus(bool _isInSelectionMenu)
        {
            if (_isInSelectionMenu)
            {
                SetIsInMainHudStatus(false);

                _states.OnMenuResetInputsStatus();
                _camHandler.On_SelectionMenu_SetInputStatus();
                
                SetIsInMenuManagerToTrue(_selectionMenuManager);

                currentInputType = InputTypeEnum.SelectionMenu;
                _selectionMenuManager.OpenSelectionMenu();
            }
            else
            {
                SetIsInMenuManagerToFalse();
                _selectionMenuManager.HideSelectionMenu();
            }
        }

        public void SetIsInEquipmentMenuStatus(bool _isInEquipmentMenu)
        {
            if (_isInEquipmentMenu)
            {
                SetIsInSelectionMenuStatus(false);

                OnMenuResetInput();

                SetIsInMenuManagerToTrue(_equipmentMenuManager);

                currentInputType = InputTypeEnum.EquipmentMenu;
                _equipmentMenuManager.ShowEquipmentMenu();
            }
            else
            {
                SetIsInMainHudStatus(true);

                OffMenuResetInput();

                SetIsInMenuManagerToFalse();
                _equipmentMenuManager.HideEquipmentMenu();
            }

            void OnMenuResetInput()
            {
                menu_select_input = false;
                menu_select_mouse = false;
            }

            void OffMenuResetInput()
            {
                menu_quit_input = false;
            }
        }
        
        public void SetIsInInstructionMenuStatus(bool _isInInstructionMenu)
        {
            if (_isInInstructionMenu)
            {
                SetIsInSelectionMenuStatus(false);

                OnMenuResetInput();

                SetIsInMenuManagerToTrue(_instructionMenuManager);

                currentInputType = InputTypeEnum.InstructionMenu;
                _instructionMenuManager.OpenInstructionMenu();
            }
            else
            {
                SetIsInMainHudStatus(true);

                OffMenuResetInput();

                SetIsInMenuManagerToFalse();
                _instructionMenuManager.HideInstructionMenu();
            }

            void OnMenuResetInput()
            {
                menu_select_input = false;
                menu_select_mouse = false;
            }
            
            void OffMenuResetInput()
            {
                menu_quit_input = false;
            }
        }

        public void SetIsInCheckpointMenuStatus(bool _isInCheckpointMenu)
        {
            if (_isInCheckpointMenu)
            {
                _camHandler.On_CheckpointMenu_SetInputStatus();

                SetIsInMenuManagerToTrue(_checkpointMenuManager);
                currentInputType = InputTypeEnum.CheckpointMenu;

                _checkpointMenuManager.ShowCheckpointMenu();
            }
            else
            {
                _camHandler.Off_CheckpointMenu_SetInputStatus();

                SetIsInMenuManagerToFalse();
                _checkpointMenuManager.HideCheckpointMenu();
            }
        }
        
        public void SetIsInLevelingMenuStatus(bool _isInLevelingMenu)
        {
            if (_isInLevelingMenu)
            {
                _camHandler.On_LevelingMenu_SetInputStatus();

                OnMenuResetInput();

                SetIsInMenuManagerToTrue(_levelingMenuManager);

                currentInputType = InputTypeEnum.LevelingMenu;
                _levelingMenuManager.ShowLevelingMenu();
            }
            else
            {
                _levelingMenuManager.HideLevelingMenu();

                SetIsInMenuManagerToTrue(_checkpointMenuManager);
                currentInputType = InputTypeEnum.CheckpointMenu;

                _states.PlayLevelUpEndAnim();
            }

            void OnMenuResetInput()
            {
                menu_select_input = false;
                menu_select_mouse = false;
            }
        }

        public void SetIsInCharRegistWindowStatus(bool _isInCharRegistWindow)
        {
            if (_isInCharRegistWindow)
            {
                _camHandler.UnlockScreenCursor();

                currentInputType = InputTypeEnum.OnPause;
                _charRegistWindow.ShowRegistWindow();
            }
            else
            {
                SetIsInMenuManagerToTrue(_instructionMenuManager);

                currentInputType = InputTypeEnum.InstructionMenu;
                _instructionMenuManager.OpenInstructionMenu();
            }
        }

        public void PausePlayerInput()
        {
            currentInputType = InputTypeEnum.OnPause;
        }
        
        public void Debug_SetIsInLevelingMenuStatus(bool _isInLevelingMenu)
        {
            //if (_isInLevelingMenu)
            //{
            //    SetIsInMainHudStatus(false);

            //    _camHandler.OnMenuResetInputsStatus();

            //    SetIsInMenuManagerToTrue(levelingMenuManager);

            //    currentInputType = InputTypeEnum.LevelingMenu;
            //    levelingMenuManager.ShowLevelingMenu();
            //}
            //else
            //{
            //    SetIsInMenuManagerToFalse();
            //    levelingMenuManager.HideLevelingMenu();
            //}
        }
        
        /// IS IN MENU MANAGER
        
        public void SetIsInMenuManagerToTrue(IMenuManagerUpdatable _iMenuManagerUpdatable)
        {
            isInMenuManager = true;
            iMenuManagerUpdatable = _iMenuManagerUpdatable;
        }

        public void SetIsInMenuManagerToFalse()
        {
            isInMenuManager = false;
            iMenuManagerUpdatable = null;
        }

        /// IS NEGLECT SELECTION MENU

        public void SetIsNeglectSelectionMenuToTrue()
        {
            SetIsInMainHudStatus(false);
            isNeglectSelectionMenu = true;
        }

        public void SetIsNeglectSelectionMenuToFalse()
        {
            isNeglectSelectionMenu = false;
        }

        /// ON DEATH
           
        public void OnDeathHideMainHud()
        {
            if (isInMenuManager)
            {
                iMenuManagerUpdatable.OnDeathOffMenuManager();
            }
            else
            {
                SetIsInMainHudStatus(false);
            }
        }

        public void OnDeathChangeInput()
        {
            currentInputType = InputTypeEnum.OnDeath;
        }
        
        /// UTILITY METHODS

        public string GetErrorMessage(string str1 = null, string str2 = null, string str3 = null, string str4 = null)
        {
            states_strBuilder.Clear();
            states_strBuilder.Append(str1).Append(str2).Append(str3).Append(str4);
            return states_strBuilder.ToString();
        }

        /// MONO ACTIONS
        
        public void MenuManagersTick()
        {
            if (_states.p_esacpe_input)
            {
                _states.p_esacpe_input = false;
                
                if (!isNeglectSelectionMenu)
                {
                    SetIsInSelectionMenuStatus(true);
                }
            }

            if (isInMenuManager)
                iMenuManagerUpdatable.Tick();
        }

        //public void RotateLevelSkybox()
        //{
        //    _currentSkyBox.SetFloat(_skyboxRotationId, _skyboxRotateSpeed * Time.time);
        //}

        enum InputTypeEnum
        {
            Main,
            SelectionMenu,
            EquipmentMenu,
            InstructionMenu,
            CheckpointMenu,
            LevelingMenu,
            OnDeath,
            OnPause
        }
    }
}