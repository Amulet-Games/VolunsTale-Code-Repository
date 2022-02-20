using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UISoftMask;
using UnityEngine.EventSystems;

namespace SA
{
    public class MainMenuManager : MonoBehaviour
    {
        #region Inputs.
        [Header("Inputs.")]
        [ReadOnlyInspector]
        public bool menu_up_input;
        [ReadOnlyInspector]
        public bool menu_down_input;
        [ReadOnlyInspector]
        public bool menu_right_input;
        [ReadOnlyInspector]
        public bool menu_left_input;

        [ReadOnlyInspector]
        public bool menu_select_mouse;
        [ReadOnlyInspector]
        public bool menu_select_input;
        [ReadOnlyInspector]
        public bool menu_remove_input;
        #endregion

        #region Smooth Damp Camera.
        [Header("Smooth Damp Camera")]
        public Transform targetCameraTransitionTransform;
        public Camera menuCamera;
        public LeanTweenType cameraMoveEaseType;
        public float cameraRotateTweenSpeed;
        public float cameraMoveTweenSpeed;
        #endregion

        #region Menu Buttons.
        [Header("Buttons Config.")]
        public RectTransform startGameRect;
        public Image startGameImage;
        public Canvas continueGameCanvas;
        public Canvas loadGameCanvas;
        public Canvas newGameCanvas;
        public Canvas endGameCanvas;
        public float _startButtonDissolveShowRate = 0.9f;
        public float _startButtonDissolveHideRate = 0.55f;
        public float _menuButtonsShowWaitRate;
        public float _menuButtonsDissovleShowRate;
        public LeanTweenType buttonPingPongFadeEaseType = LeanTweenType.easeOutCubic;
        public float _startButtonPingPongFadeSpeed = 1.3f;
        public float buttonPingPongFadeMinValue = 0.6f;
        #endregion
        
        #region Opening Particles.
        [Header("Opening Particles Config.")]
        public float _openingMaskTweenWaitRate = 1.75f;
        public float _openingBonfireIgniteWaitRate = 3.5f;
        public float _openingBonfireDeactiveWaitRate = 3.3f;
        public GameObject _snowParticles;
        public GameObject _fogParticles;
        public GameObject _titleBonfireIgniteParticle;
        public GameObject _titleBonfireObject;
        #endregion

        #region Opening Title.
        [Header("Game Title Config.")]
        public LeanTweenType _titleDissolveEaseType;
        public float _titleDissolveShowRate = 0.75f;
        public float _titleDissolveHideRate = 0.35f;
        public Image _gameTitleImage;
        public GameTitleSprinkle _1st_title_sprinkle;
        public GameTitleSprinkle _2nd_title_sprinkle;
        public GameTitleSprinkle _3rd_title_sprinkle;
        bool _hasBegunSprinkle;
        #endregion

        #region Main Menu Msg Type.
        [Header("Main Menu Msg Type")]
        public DualButton_MainMenuMsg noPrevSave_MainMenuMsg;
        public SingleButton_MainMenuMsg saveAmtExcd_MainMenuMsg;
        public ManSaveList_MainMenuMsg manSaveList_MainMenuMsg;
        public SingleButton_MainMenuMsg noSaveList_MainMenuMsg;
        [ReadOnlyInspector] public MainMenuMsgTypeEnum _mainMenuMsgType;
        #endregion

        [Header("Message Group Tween.")]
        public CanvasGroup messageGroup;
        public LeanTweenType messageEaseType = LeanTweenType.easeOutCirc;
        public float messageFadeSpeed = 0.125f;

        [Header("Message Buttons Color.")]
        public Color _fullAlphaColor;
        public Color _pressedDownColor;
        
        [Header("Manager (Drops).")]
        public GameManager _gameManager;
        
        [Header("Menu Skybox Config.")]
        public Material _currentSkyBox;
        public float _skyboxRotateSpeed = 0.5f;

        [Header("Status.")]
        [ReadOnlyInspector] public float _delta;

        #region Refs.
        [HideInInspector] public SavableManager _savableManager;
        SessionManager _sessionManager;
        #endregion

        #region Privates.

        #region Buttons.
        Button startGameButton;
        Button continueGameButton;
        Button loadGameButton;
        Button newGameButton;
        Button endGameButton;
        #endregion

        #region Material.
        Material _titleDissolveMat;
        Material _startGameDissolveMat;
        Material _newGameButtonDessolveMat;
        #endregion

        #region Vector3s.
        [HideInInspector] public Vector3 vector3Fwd = Vector3.forward;
        #endregion

        #region IDs.
        int _titleDissolveTweenId;
        int _startGameTweenId;
        int _messageTweenId;
        int _dissolvePropertyId;
        int _skyboxRotPropertyId;
        #endregion

        #endregion

        /// -------------------------------- Methods

        #region Main Menu Button Functions
        public void StartGame()
        {
            OnDissolveHideGameTitle();
            OnDissolveHideStartGame();

            _sessionManager._postProcessManager.Expand_TitleScreen_BlurEffect_AfterStartButton();
        }

        public void LoadGame()
        {
            if (manSaveList_MainMenuMsg._savedProfileCount < 1)
            {
                On_NoSaveListMsg();
            }
            else
            {
                Open_ManSaveListMsg();
            }
        }

        public void ContinueGame()
        {
            if (_savableManager._prev_SubSaveFile.isUsed)
            {
                _savableManager.OnContinueGame();
                _sessionManager.StartGame_UnloadTitleScreen();

                DisableButtonsInteractable();
            }
            else
            {
                On_NoPreviousSaveMsg();
            }
        }

        public void NewGame()
        {
            if (manSaveList_MainMenuMsg.CanStartNewGame())
            {
                _savableManager.isNewGame = true;
                _sessionManager.StartGame_UnloadTitleScreen();

                DisableButtonsInteractable();
            }
            else
            {
                On_SaveAmountExceedMsg();
            }
        }

        public void EndGame()
        {
            _sessionManager.QuitGame_MainMenu();
        }
        #endregion

        #region MOVE IN / OUT CAMERA.
        void CameraMoveInPosition()
        {
            LeanTween.move(menuCamera.gameObject, targetCameraTransitionTransform.position, cameraMoveTweenSpeed).setEase(cameraMoveEaseType).setOnComplete(OnCompleteCameraMoveInPosition);
            LeanTween.rotate(menuCamera.gameObject, targetCameraTransitionTransform.eulerAngles, cameraRotateTweenSpeed).setEase(cameraMoveEaseType);
        }

        void OnCompleteCameraMoveInPosition()
        {
            EnableButtonsInteractable();
        }
        #endregion

        #region SHOW / HIDE BUTTONS.
        void StartShowMenuButtonsWaitCounter()
        {
            LeanTween.value(0, 1, _menuButtonsShowWaitRate).setOnComplete(OnCompleteMenuButtonsWait);
        }

        void OnCompleteMenuButtonsWait()
        {
            ShowButtons();
        }

        void ShowButtons()
        {
            LeanTween.value(0, 1, _menuButtonsDissovleShowRate).setOnUpdate((value) => _newGameButtonDessolveMat.SetFloat(_dissolvePropertyId, value));
        }
        
        void DisableButtonsInteractable()
        {
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
            newGameButton.interactable = false;
            endGameButton.interactable = false;
        }

        public void EnableButtonsInteractable()
        {
            continueGameButton.interactable = true;
            loadGameButton.interactable = true;
            newGameButton.interactable = true;
            endGameButton.interactable = true;
        }

        void DisableButtonsCanvas()
        {
            continueGameCanvas.enabled = false;
            loadGameCanvas.enabled = false;
            newGameCanvas.enabled = false;
            endGameCanvas.enabled = false;
        }

        void EnableButtonsCanvas()
        {
            continueGameCanvas.enabled = true;
            loadGameCanvas.enabled = true;
            newGameCanvas.enabled = true;
            endGameCanvas.enabled = true;
        }

        #region Start Game.
        void OnDissolveShowStartGame()
        {
            startGameButton.interactable = true;

            _startGameTweenId = LeanTween.value(0, 1, _startButtonDissolveShowRate).setOnUpdate((value) => _startGameDissolveMat.SetFloat(_dissolvePropertyId, value)).setOnComplete(OnCompleteShowStartGame).id;
        }
        
        void OnCompleteShowStartGame()
        {
            PingPongLoopFadeOutStartGame();
        }

        void OnDissolveHideStartGame()
        {
            if (LeanTween.isTweening(_startGameTweenId))
                LeanTween.cancel(_startGameTweenId);
            
            startGameButton.interactable = false;

            ResetStartGameColorToFullAlpha();

            LeanTween.value(1, 0, _startButtonDissolveHideRate).setOnUpdate((value) => _startGameDissolveMat.SetFloat(_dissolvePropertyId, value)).setOnComplete(OnCompleteHideStartGame);

            void ResetStartGameColorToFullAlpha()
            {
                Color _color = startGameImage.color;
                _color.a = 100;
                startGameImage.color = _color;
            }
        }

        void OnCompleteHideStartGame()
        {
            startGameImage.gameObject.SetActive(false);
        }

        void PingPongLoopFadeInStartGame()
        {
            _startGameTweenId = LeanTween.alpha(startGameRect, 1, _startButtonPingPongFadeSpeed).setEase(buttonPingPongFadeEaseType).setOnComplete(PingPongLoopFadeOutStartGame).id;
        }

        void PingPongLoopFadeOutStartGame()
        {
            _startGameTweenId = LeanTween.alpha(startGameRect, buttonPingPongFadeMinValue, _startButtonPingPongFadeSpeed).setEase(buttonPingPongFadeEaseType).setOnComplete(PingPongLoopFadeInStartGame).id;
        }
        #endregion

        #endregion

        #region ON / OFF No Previous Save Message.
        public void Off_NoPrevSaveMsg_ByQuit()
        {
            Set_MainMenuMsgType_ToEmpty();

            FadeOut_NoPrevSaveMsg();

            EnableButtonsInteractable();
            
            void FadeOut_NoPrevSaveMsg()
            {
                noPrevSave_MainMenuMsg.OnMessageClose();
                FadeOutMessageGroup_DisableCanvas(MainMenuMsgTypeEnum.NoPrevSave);
            }
        }

        public void Off_NoPrevSaveMsg_ByNewGame()
        {
            Set_MainMenuMsgType_ToEmpty();

            CloseImmedi_NoPrevSaveMsg();

            NewGame();

            void CloseImmedi_NoPrevSaveMsg()
            {
                Hide_MessageGroup_Immediate();
            }
        }

        void On_NoPreviousSaveMsg()
        {
            Set_MainMenuMsgType_ToNoPrevSave();

            FadeIn_NoPrevSaveMsg();

            DisableButtonsInteractable();

            void FadeIn_NoPrevSaveMsg()
            {
                noPrevSave_MainMenuMsg.OnMessageOpen();
                FadeInMessageGroup();
            }
        }
        #endregion

        #region ON / OFF Save Amount Exceed Message.
        public void Off_SaveAmtExcd_byOK()
        {
            Set_MainMenuMsgType_ToEmpty();

            FadeOut_NoPrevSaveMsg();

            EnableButtonsInteractable();

            void FadeOut_NoPrevSaveMsg()
            {
                FadeOutMessageGroup_DisableCanvas(MainMenuMsgTypeEnum.SaveAmtExd);
            }
        }

        void On_SaveAmountExceedMsg()
        {
            Set_MainMenuMsgType_ToSaveAmtExcd();

            FadeIn_SaveAmtExcdMsg();

            DisableButtonsInteractable();

            void FadeIn_SaveAmtExcdMsg()
            {
                saveAmtExcd_MainMenuMsg.OnMessageOpen();
                FadeInMessageGroup();
            }
        }
        #endregion

        #region ON / OFF Manage Save List Message.
        public void Close_ManSaveListMsg_UIButton()
        {
            Set_MainMenuMsgType_ToEmpty();

            MoveOut_ManSaveListMsg();
            
            void MoveOut_ManSaveListMsg()
            {
                manSaveList_MainMenuMsg.OnMessageClose();
            }
        }

        public void OnCompleteHideActive_LG_Profiles()
        {
            FadeOutMessageGroup_NoAction();
            EnableButtonsInteractable();
        }
        
        void Open_ManSaveListMsg()
        {
            Set_MainMenuMsgType_ToManSaveList();

            MoveIn_ManSaveListMsg();

            DisableButtonsInteractable();

            void MoveIn_ManSaveListMsg()
            {
                manSaveList_MainMenuMsg.OnMessageOpen();
                FadeInMessageGroup();
            }
        }
        #endregion

        #region ON / OFF No Save List Message.
        public void Off_NoSaveList_byOK()
        {
            Set_MainMenuMsgType_ToEmpty();

            FadeOut_NoSaveListMsg();

            EnableButtonsInteractable();

            void FadeOut_NoSaveListMsg()
            {
                FadeOutMessageGroup_DisableCanvas(MainMenuMsgTypeEnum.NoSaveList);
            }
        }

        void On_NoSaveListMsg()
        {
            Set_MainMenuMsgType_ToNoSaveList();

            FadeIn_NoSaveListMsg();

            DisableButtonsInteractable();

            void FadeIn_NoSaveListMsg()
            {
                noSaveList_MainMenuMsg.OnMessageOpen();
                FadeInMessageGroup();
            }
        }
        #endregion

        #region OFF No Save Left Message.
        public void Off_NoSaveLeft_ByOK()
        {
            Set_MainMenuMsgType_ToEmpty();

            FadeOutMessageGroup_NoAction();

            EnableButtonsInteractable();
        }
        #endregion

        #region SHOW / HIDE MESSAGE GROUP.
        void Cancel_MessageGroup_Tween()
        {
            if (LeanTween.isTweening(_messageTweenId))
                LeanTween.cancel(_messageTweenId);
        }

        void FadeInMessageGroup()
        {
            Cancel_MessageGroup_Tween();
            _messageTweenId = LeanTween.alphaCanvas(messageGroup, 1, messageFadeSpeed).setEase(messageEaseType).id;
        }

        public void FadeOutMessageGroup_DisableCanvas(MainMenuMsgTypeEnum _mainMenuMsgType)
        {
            Cancel_MessageGroup_Tween();
            _messageTweenId = LeanTween.alphaCanvas(messageGroup, 0, messageFadeSpeed).setEase(messageEaseType).setOnComplete(OnCompleteTween).id;

            void OnCompleteTween()
            {
                switch (_mainMenuMsgType)
                {
                    case MainMenuMsgTypeEnum.NoPrevSave:
                        noPrevSave_MainMenuMsg.DisableCanvas();
                        break;
                    case MainMenuMsgTypeEnum.SaveAmtExd:
                        saveAmtExcd_MainMenuMsg.DisableCanvas();
                        break;
                    case MainMenuMsgTypeEnum.NoSaveList:
                        noSaveList_MainMenuMsg.DisableCanvas();
                        break;
                }
            }
        }

        public void FadeOutMessageGroup_StartGame()
        {
            Cancel_MessageGroup_Tween();
            _messageTweenId = LeanTween.alphaCanvas(messageGroup, 0, messageFadeSpeed).setEase(messageEaseType).setOnComplete(OnCompleteTween).id;

            void OnCompleteTween()
            {
                _sessionManager.StartGame_UnloadTitleScreen();
            }
        }

        void FadeOutMessageGroup_NoAction()
        {
            Cancel_MessageGroup_Tween();
            _messageTweenId = LeanTween.alphaCanvas(messageGroup, 0, messageFadeSpeed).setEase(messageEaseType).id;
        }

        void Hide_MessageGroup_Immediate()
        {
            Cancel_MessageGroup_Tween();
            messageGroup.alpha = 0;
        }
        #endregion

        #region Title Opening.

        void SetupTweenOpeningWaitCounter()
        {
            LeanTween.value(0, 1, _openingMaskTweenWaitRate).setOnComplete(OnCompleteOpeningWaitCounter);
        }

        void OnCompleteOpeningWaitCounter()
        {
            _snowParticles.SetActive(true);
            _fogParticles.SetActive(true);

            _sessionManager._loadingScreenHandler.OnTitleScreenFadeOutOpeningMask();
            _sessionManager._postProcessManager.Expand_TitleScreen_BlurEffect_MaskOpen();
        }

        #region Opening Bonfire.
        void SetupOpeningBonfireIgniteWaitCounter()
        {
            LeanTween.value(0, 1, _openingBonfireIgniteWaitRate).setOnComplete(OnCompleteOpeningBonfireIgniteWait);
        }

        void OnCompleteOpeningBonfireIgniteWait()
        {
            EnableOpeningBonfire();
            BeginBonfireDeactiveWaitCounter();
            OnDissolveShowGameTitle();
            OnDissolveShowStartGame();
        }

        void BeginBonfireDeactiveWaitCounter()
        {
            LeanTween.value(0, 1, _openingBonfireDeactiveWaitRate).setOnComplete(OnCompleteBonfireDeactiveWait);
        }

        void OnCompleteBonfireDeactiveWait()
        {
            if (_titleBonfireIgniteParticle != null)
                _titleBonfireIgniteParticle.SetActive(false);
        }

        void EnableOpeningBonfire()
        {
            _titleBonfireIgniteParticle.SetActive(true);
            _titleBonfireObject.SetActive(true);
        }

        void OnDissolveShowGameTitle()
        {
            _titleDissolveTweenId = LeanTween.value(0, 1, _titleDissolveShowRate).setEase(_titleDissolveEaseType).setOnUpdate((value) => _titleDissolveMat.SetFloat(_dissolvePropertyId, value)).id;
            BeginTitleSprinkles();
        }

        void OnDissolveHideGameTitle()
        {
            if (LeanTween.isTweening(_titleDissolveTweenId))
                LeanTween.cancel(_titleDissolveTweenId);

            LeanTween.value(1, 0, _titleDissolveHideRate).setEase(_titleDissolveEaseType).setOnUpdate((value) => _titleDissolveMat.SetFloat(_dissolvePropertyId, value)).setOnComplete(OnCompleteDissolveHideGameTitle);
            EndTitleSprinkles();
        }
        
        void OnCompleteDissolveHideGameTitle()
        {
            CameraMoveInPosition();

            EnableButtonsCanvas();
            StartShowMenuButtonsWaitCounter();
        }
        
        void BeginTitleSprinkles()
        {
            _1st_title_sprinkle.FadeInSprinkle();
            _1st_title_sprinkle.EnlargeSprinkle();

            _2nd_title_sprinkle.FadeInSprinkle();
            _2nd_title_sprinkle.EnlargeSprinkle();

            _3rd_title_sprinkle.FadeInSprinkle();
            _3rd_title_sprinkle.EnlargeSprinkle();

            _hasBegunSprinkle = true;
        }

        void EndTitleSprinkles()
        {
            _1st_title_sprinkle.CancelSprinkleJobs();
            _2nd_title_sprinkle.CancelSprinkleJobs();
            _3rd_title_sprinkle.CancelSprinkleJobs();

            _hasBegunSprinkle = false;
        }
        #endregion

        #endregion

        #region Set Msg Updatable.
        public void Set_MainMenuMsgType_ToEmpty()
        {
            _mainMenuMsgType = MainMenuMsgTypeEnum.Empty;
        }

        public void Set_MainMenuMsgType_ToNoPrevSave()
        {
            _mainMenuMsgType = MainMenuMsgTypeEnum.NoPrevSave;
        }
        
        public void Set_MainMenuMsgType_ToManSaveList()
        {
            _mainMenuMsgType = MainMenuMsgTypeEnum.ManSaveList;
        }

        public void Set_MainMenuMsgType_ToSaveAmtExcd()
        {
            _mainMenuMsgType = MainMenuMsgTypeEnum.SaveAmtExd;
        }

        public void Set_MainMenuMsgType_ToNoSaveList()
        {
            _mainMenuMsgType = MainMenuMsgTypeEnum.NoSaveList;
        }
        #endregion

        #region Update.
        void Update()
        {
            UpdateDeltas();

            UpdateTitleSprinkles();

            UpdateSkyboxRotation();

            UpdateMainMenuMsgByType();
        }

        void UpdateDeltas()
        {
            _delta = Time.deltaTime;
        }

        void UpdateTitleSprinkles()
        {
            if (_hasBegunSprinkle)
            {
                _1st_title_sprinkle.RotateSprinkle();
                _2nd_title_sprinkle.RotateSprinkle();
                _3rd_title_sprinkle.RotateSprinkle();
            }
        }

        void UpdateSkyboxRotation()
        {
            _currentSkyBox.SetFloat(_skyboxRotPropertyId, _skyboxRotateSpeed * Time.time);
        }

        void UpdateMainMenuMsgByType()
        {
            switch (_mainMenuMsgType)
            {
                case MainMenuMsgTypeEnum.Empty:
                    break;
                case MainMenuMsgTypeEnum.NoPrevSave:
                    noPrevSave_MainMenuMsg.Tick();
                    break;
                case MainMenuMsgTypeEnum.SaveAmtExd:
                    saveAmtExcd_MainMenuMsg.Tick();
                    break;
                case MainMenuMsgTypeEnum.ManSaveList:
                    manSaveList_MainMenuMsg.Tick();
                    break;
                case MainMenuMsgTypeEnum.NoSaveList:
                    noSaveList_MainMenuMsg.Tick();
                    break;
            }
        }
        #endregion

        #region Msg Inputs.
        public void UpdateInputs_SingleButton()
        {
            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
        }

        public void UpdateInputs_DualButton()
        {
            menu_left_input = Input.GetButtonDown("dual_menu_left");
            menu_right_input = Input.GetButtonDown("dual_menu_right");

            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
        }
        
        public void UpdateInputs_ManSaveList()
        {
            menu_up_input = Input.GetButtonDown("dual_menu_up");
            menu_down_input = Input.GetButtonDown("dual_menu_down");

            menu_select_mouse = Input.GetButtonDown("menu_select_mouse");
            menu_select_input = Input.GetButtonDown("menu_select");
            menu_remove_input = Input.GetButtonDown("menu_remove");
        }
        #endregion

        #region Init.
        void Awake()
        {
            InitButtonRefs();
            InitMaterialRefs();

            /// Game Manager.
            _gameManager.Init();
        }
        
        void InitButtonRefs()
        {
            startGameButton = startGameImage.GetComponent<Button>();
            startGameButton.interactable = false;

            continueGameButton = continueGameCanvas.GetComponent<Button>();
            loadGameButton = loadGameCanvas.GetComponent<Button>();
            newGameButton = newGameCanvas.GetComponent<Button>();
            endGameButton = endGameCanvas.GetComponent<Button>();

            DisableButtonsCanvas();
            DisableButtonsInteractable();
        }

        void InitMaterialRefs()
        {
            /// Title Dissolve.
            _titleDissolveMat = _gameTitleImage.material;
            _dissolvePropertyId = Shader.PropertyToID("_DissolveAmount");
            _titleDissolveMat.SetFloat(_dissolvePropertyId, 0);

            /// Start Game Dissolve.
            _startGameDissolveMat = startGameImage.material;
            _startGameDissolveMat.SetFloat(_dissolvePropertyId, 0);

            /// Menu Buttons Dissolve (They all share the same material, so here only get 'New Game' Mat).
            _newGameButtonDessolveMat = newGameCanvas.GetComponent<Image>().material;
            _newGameButtonDessolveMat.SetFloat(_dissolvePropertyId, 0);
        }
        #endregion

        #region Setup.
        void Start()
        {
            SetupRefs();
            SetupMainMenuMsgType();
            SetupMainMenuMsgs();
            
            /// Game Manager.
            _gameManager.Setup();

            /// Setup Tween.
            SetupTweenOpeningWaitCounter();
            SetupOpeningBonfireIgniteWaitCounter();

            /// Setup Sprinkles.
            SetupSprinkles();

            /// Setup Skybox.
            SetupMainMenuSkybox();
        }

        void SetupRefs()
        {
            _savableManager = SavableManager.singleton;
            _sessionManager = SessionManager.singleton;
        }
        
        void SetupMainMenuMsgType()
        {
            Set_MainMenuMsgType_ToEmpty();
        }

        void SetupMainMenuMsgs()
        {
            noPrevSave_MainMenuMsg.Setup(this);
            saveAmtExcd_MainMenuMsg.Setup(this);
            manSaveList_MainMenuMsg.Setup(this);
            noSaveList_MainMenuMsg.Setup(this);
        }

        void SetupSprinkles()
        {
            _1st_title_sprinkle._mainMenuManager = this;
            _2nd_title_sprinkle._mainMenuManager = this;
            _3rd_title_sprinkle._mainMenuManager = this;
        }

        void SetupMainMenuSkybox()
        {
            _skyboxRotPropertyId = Shader.PropertyToID("_Rotation");
        }
        #endregion
    }

    public enum MainMenuMsgTypeEnum
    {
        Empty,
        NoPrevSave,
        SaveAmtExd,
        ManSaveList,
        NoSaveList
    }
}

#region SHOW / HIDE LOAD GAME PROFILES.
//void ShowActiveLoadGameProfiles()
//{
//    float delayAmount = profileMovesDelayTime;
//    for (int i = 0; i < 5; i++)
//    {
//        if (loadGameProfile[i].isActive)
//        {
//            ShowLoadGameProfile(loadGameProfile[i], delayAmount);
//            delayAmount += profileMovesDelayTime;
//        }
//    }
//}

//void HideActiveLoadGameProfiles()
//{
//    float delayAmount = profileMovesDelayTime;

//    for (int i = 0; i < 5; i++)
//    {
//        if (loadGameProfile[i].isActive)
//        {
//            HideLoadGameProfile(loadGameProfile[i], delayAmount);
//            delayAmount += profileMovesDelayTime;
//        }
//    }
//}

//void ShowLoadGameProfile(LoadGameProfile _loadGameProfile, float _delaySec)
//{
//    _loadGameProfile.EnableProfile();
//    LeanTween.moveX(_loadGameProfile.profileRect, profileMovesInXPos, profileMovesSpeed).setDelay(_delaySec);
//}

//void HideLoadGameProfile(LoadGameProfile _loadGameProfile, float _delaySec)
//{
//    LeanTween.moveX(_loadGameProfile.profileRect, profileMovesOutXPos, profileMovesSpeed).setDelay(_delaySec).setOnComplete(_loadGameProfile.DisableProfile);
//}
#endregion

#region Show / Hide Load Menu.
//void ShowLoadMenu()
//{
//    EnableLoadMenu();
//    LeanTween.moveX(loadGameMenu, loadMenuMovesInXPos, loadMenuMovesSpeed).setEase(loadGameMenuEaseType);
//}

//void EnableLoadMenu()
//{
//    loadGameMenuCanvas.enabled = true;
//}

//void HideLoadMenu()
//{
//    LeanTween.moveX(loadGameMenu, loadMenuMovesOutXPos, loadMenuMovesSpeed).setEase(loadGameMenuEaseType).setOnComplete(DisableLoadMenu);
//}

//void DisableLoadMenu()
//{
//    loadGameMenuCanvas.enabled = false;
//}
#endregion

#region Grab SaveFiles.
//void GetSubSavedFiles()
//{
//    List<SaveFile> _savedSaveFile = _savableManager.savedFilesList;
//    _savedProfileCount = _savedSaveFile.Count;
//    for (int i = 0; i < _savedProfileCount; i++)
//    {
//        /// SET UP.
//        loadGameProfile[i].Setup(this);

//        /// TEXT
//        loadGameProfile[i].profile_p_Location.text = _savedSaveFile[i].savedPlayerState.savableSpawnPointName;
//        loadGameProfile[i].profile_p_Name.text = _savedSaveFile[i].savedPlayerStats.savablePlayerName;
//        loadGameProfile[i].profile_p_Lv.text = _savedSaveFile[i].savedPlayerStats.savablePlayerLevel.ToString();
//        loadGameProfile[i].profile_p_Souls.text = _savedSaveFile[i].savedPlayerStats.savableSouls.ToString();

//        /// SPRITE
//        Sprite savedSubSaveSprite = _savableManager.GetAvatarSpriteFromDict(_savedSaveFile[i].saveFileId);
//        if (savedSubSaveSprite != null)
//        {
//            loadGameProfile[i].profile_p_Avatar.enabled = true;
//            loadGameProfile[i].profile_p_Avatar.sprite = savedSubSaveSprite;
//        }
//        else
//        {
//            loadGameProfile[i].profile_p_Avatar.enabled = false;
//            loadGameProfile[i].profile_p_Avatar.sprite = null;
//        }

//        loadGameProfile[i].profileId = _savedSaveFile[i].saveFileId;
//    }
//}
#endregion