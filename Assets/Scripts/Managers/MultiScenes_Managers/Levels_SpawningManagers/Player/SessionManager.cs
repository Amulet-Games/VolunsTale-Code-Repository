using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA
{
    public class SessionManager : MonoBehaviour
    {
        [Header("Config.")]
        public float _destroyImmanentObjectWaitRate;
        public bool _isDebugMode;

        [Header("Immanents.")]
        [ReadOnlyInspector] public GameObject immanent_GameManager;
        [ReadOnlyInspector] public GameObject immanent_WI_Effects_Bp;
        [ReadOnlyInspector] public GameObject immanent_WA_Effects_Bp;
        [ReadOnlyInspector] public GameObject immanent_BloodFx_Bp;

        [Header("Core Prefabs (Drops).")]
        public StateManager _playerPrefab;
        public CameraHandler _mainCameraPrefab;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isCurrentAsyncOperationReady;
        [ReadOnlyInspector] public bool isCurrentAsyncOperationFinished;
        [ReadOnlyInspector] public bool isInLevel;

        [Header("Core Refs.")]
        [ReadOnlyInspector] public StateManager _states;
        [ReadOnlyInspector] public CameraHandler _camHandler;

        [Header("Private Refs.")]
        [ReadOnlyInspector, SerializeField] LevelManager _levelManager;
        [ReadOnlyInspector, SerializeField] SavableManager _savableManager;
        [ReadOnlyInspector] public LoadingScreenHandler _loadingScreenHandler;
        [ReadOnlyInspector] public PostProcessManager _postProcessManager;
        [ReadOnlyInspector] public AsyncOperation _currentAsyncOperation;

        public event Action _onAsyncOperationReadyEvent;
        public event Action _onAsyncOperationFullyFinishEvent;

        public static SessionManager singleton;
        void Awake()
        {
            if(singleton == null)
            {
                singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame_UnloadTitleScreen()
        {
            _loadingScreenHandler.DisableOpeningMask();
            _loadingScreenHandler.ShowLoadingScreen(2, "Unload Title Screen.", TargetAsyncAction);

            void TargetAsyncAction()
            {
                /// Load Title Screen Scene.
                isCurrentAsyncOperationReady = true;
                isCurrentAsyncOperationFinished = false;

                _onAsyncOperationFullyFinishEvent = AsyncOperationFinish_StartGame_UnLoadTitleScreen;

                UnLoadSceneWithProgress((int)SceneIndexes.TITLE_SCREEN);
            }
        }

        void StartGame_ChainOps_LoadLevel()
        {
            /// Load Level Scene.
            isCurrentAsyncOperationReady = false;
            isCurrentAsyncOperationFinished = false;

            _onAsyncOperationReadyEvent = ChainOperationReady_StartGame_LoadLevel;
            _onAsyncOperationFullyFinishEvent = ChainOperationFinish_StartGame_LoadLevel_EndOps;

            if (_isDebugMode)
            {
                LoadSceneWithProgress((int)SceneIndexes.AI_TEST, false);
            }
            else
            {
                LoadSceneWithProgress((int)SceneIndexes.MAIN_LEVEL, false);
            }
        }

        public void QuitGame_UnLoadLevel()
        {
            /// Serialize.
            AutoSerializaion_SaveFromCurrentSaveFile();

            /// Set Status.
            isInLevel = false;

            /// Begin Show Loading Screen.
            _loadingScreenHandler.ShowLoadingScreen(3, "Exiting Level.", TargetAsyncAction);

            void TargetAsyncAction()
            {
                /// UnLoad Level Scene.
                isCurrentAsyncOperationReady = true;
                isCurrentAsyncOperationFinished = false;

                _onAsyncOperationFullyFinishEvent = AsyncOperationFinish_QuitGame_UnLoadLevel;

                UnLoadSceneWithProgress((int)SceneIndexes.MAIN_LEVEL);
            }
        }

        void QuitGame_ChainOps_ReloadTitleScreen()
        {
            _loadingScreenHandler.RefreshLoadingScreen("Loading Title Screen.");

            /// Load Title Screen Scene.
            isCurrentAsyncOperationReady = false;
            isCurrentAsyncOperationFinished = false;

            _onAsyncOperationReadyEvent = ChainOperationReady_QuitGame_ReloadTitleScreen;
            _onAsyncOperationFullyFinishEvent = ChainOperationFinish_QuitGame_ReloadTitleScreen_EndOps;

            LoadSceneWithProgress((int)SceneIndexes.TITLE_SCREEN , false);
        }

        #region Create / Move Player.
        void LoadPlayer()
        {
            PlayerSpawnPoint _loadGameSpawnPoint;
            if (_isDebugMode)
            {
                _loadGameSpawnPoint = _levelManager._debugGameSpawnPoint;
            }
            else
            {
                _loadGameSpawnPoint = _levelManager.defualtStartGameSpawnPoint;
            }

            InstantiateCoreReferences();
            MovePlayerToSpawnPoint();

            void InstantiateCoreReferences()
            {
                _states = Instantiate(_playerPrefab);
                _camHandler = Instantiate(_mainCameraPrefab);
            }

            void MovePlayerToSpawnPoint()
            {
                _states._currentSpawnPoint = _loadGameSpawnPoint;
                _states.transform.position = _loadGameSpawnPoint.transform.position;
                _states.transform.rotation = _loadGameSpawnPoint.transform.rotation;
            }
        }

        void ActivateCoreReferences()
        {
            _states.gameObject.SetActive(true);
            _camHandler.gameObject.SetActive(true);
        }
        #endregion

        #region Load / UnLoad Scene With Progress.
        void LoadSceneWithProgress(int sceneId, bool _isAutoActivateLoadedScene)
        {
            _currentAsyncOperation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
            _currentAsyncOperation.allowSceneActivation = _isAutoActivateLoadedScene;
            StartCoroutine(MonitorAsyncOperationProgress());
        }

        void UnLoadSceneWithProgress(int sceneId)
        {
            _currentAsyncOperation = SceneManager.UnloadSceneAsync(sceneId);
            StartCoroutine(MonitorAsyncOperationProgress());
        }

        IEnumerator MonitorAsyncOperationProgress()
        {
            while (!_currentAsyncOperation.isDone)
            {
                float _progress = _currentAsyncOperation.progress;

                _loadingScreenHandler.OnUpdatingLoadingSlider(_progress);
                if (_progress == 0.9f)
                {
                    OnCurrentAsyncOperationReady();
                }

                yield return null;
            }

            OnCurrentAsyncOperationFinish();
        }

        void OnCurrentAsyncOperationReady()
        {
            if (!isCurrentAsyncOperationReady)
            {
                _onAsyncOperationReadyEvent.Invoke();
            }
        }

        void OnCurrentAsyncOperationFinish()
        {
            isCurrentAsyncOperationFinished = true;
            _onAsyncOperationFullyFinishEvent.Invoke();
        }

        #region Async Operation Ready Varients.
        void ChainOperationReady_StartGame_LoadLevel()
        {
            _loadingScreenHandler.RefreshLoadingScreen(1, "Prepare Activation.");

            isCurrentAsyncOperationReady = true;
            _currentAsyncOperation.allowSceneActivation = true;
            _onAsyncOperationReadyEvent = null;
        }

        void ChainOperationReady_QuitGame_ReloadTitleScreen()
        {
            _loadingScreenHandler.RefreshLoadingScreen(1, "Prepare Activation.");

            isCurrentAsyncOperationReady = true;
            _currentAsyncOperation.allowSceneActivation = true;
            _onAsyncOperationReadyEvent = null;
        }
        #endregion

        #region Async Operation Finish Varients.
        void AsyncOperationFinish_StartGame_UnLoadTitleScreen()
        {
            _loadingScreenHandler.RefreshLoadingScreen("Initialize Level.");
            StartGame_ChainOps_LoadLevel();
        }

        void ChainOperationFinish_StartGame_LoadLevel_EndOps()
        {
            if (_isDebugMode)
            {
                SetActiveScene((int)SceneIndexes.AI_TEST);
            }
            else
            {
                SetActiveScene((int)SceneIndexes.MAIN_LEVEL);
            }

            /// Load Core Refs.
            LoadPlayer();
            ActivateCoreReferences();

            /// Switch Post Process Profile.
            _postProcessManager.SwitchVolumeToWinterField();

            /// Set Status.
            isInLevel = true;

            _onAsyncOperationFullyFinishEvent = null;
            _currentAsyncOperation = null;

            _loadingScreenHandler.HideLoadingScreen(true);
        }

        void AsyncOperationFinish_QuitGame_UnLoadLevel()
        {
            /// Destroy Immanent Object.
            DestroyImmanentObjs();

            /// Collect Saved Files.
            _savableManager.OnQuitLevel_ToTitleScreen();
            
            /// Change Loading Screen.
            _loadingScreenHandler.RefreshLoadingScreen(0.6f, "Collecting Saved Files.");

            /// Tween Wait For 5 sec.
            TweenStartDestroyImmanentObjectWait();

            void DestroyImmanentObjs()
            {
                Destroy(immanent_GameManager);
                Destroy(immanent_WI_Effects_Bp);
                Destroy(immanent_WA_Effects_Bp);
                Destroy(immanent_BloodFx_Bp);
            }

            void TweenStartDestroyImmanentObjectWait()
            {
                LeanTween.value(0, 1, _destroyImmanentObjectWaitRate).setOnComplete(OnCompleteDestroyImmanentObjectWait);
            }

            void OnCompleteDestroyImmanentObjectWait()
            {
                QuitGame_ChainOps_ReloadTitleScreen();
            }
        }

        void ChainOperationFinish_QuitGame_ReloadTitleScreen_EndOps()
        {
            SetActiveScene((int)SceneIndexes.TITLE_SCREEN);
            _postProcessManager.SwitchVolumeToTitleScreen();

            _onAsyncOperationFullyFinishEvent = null;
            _currentAsyncOperation = null;

            _loadingScreenHandler.HideLoadingScreen(false);
        }
        #endregion

        #endregion
        
        #region Load / UnLoad Scene Direct.
        void LoadSceneDirect(int sceneId)
        {
            _currentAsyncOperation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);
            StartCoroutine(MonitorLoadSceneDirectFinished());

            IEnumerator MonitorLoadSceneDirectFinished()
            {
                while (!_currentAsyncOperation.isDone)
                {
                    yield return null;
                }

                SetActiveScene(sceneId);
            }
        }

        void UnLoadSceneDirect(int sceneId)
        {
            SceneManager.UnloadSceneAsync(sceneId);
        }
        #endregion

        #region Quit Application.
        public void QuitGame_MainMenu()
        {
            Debug.Log("Has Started Quit Game From Menu.");
            Application.Quit();
        }
        
        void OnApplicationQuit()
        {  
            if (isInLevel)
            {
                AutoSerializaion_SaveFromCurrentSaveFile();
            }
        }

        void AutoSerializaion_SaveFromCurrentSaveFile()
        {
            _savableManager.Serialize_ExistFile();
            Debug.Log("Application Quit, SaveFile Created.");
        }
        #endregion

        #region Setup.
        public void Start()
        {
            SetupManagerRefs();
            Setup_LoadTitleScreen();
        }

        void SetupManagerRefs()
        {
            _savableManager = SavableManager.singleton;
            _postProcessManager = PostProcessManager.singleton;

            _levelManager = LevelManager.singleton;
            _levelManager.Setup();
        }
        
        void Setup_LoadTitleScreen()
        {
            LoadSceneDirect((int)SceneIndexes.TITLE_SCREEN);
        }
        #endregion

        void SetActiveScene(int _targetActiveSceneIndex)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_targetActiveSceneIndex));
        }
    }

    public enum SceneIndexes
    {
        PERSISTENT = 0,
        TITLE_SCREEN = 1,
        MAIN_LEVEL = 2,
        AI_TEST = 3
    }
}