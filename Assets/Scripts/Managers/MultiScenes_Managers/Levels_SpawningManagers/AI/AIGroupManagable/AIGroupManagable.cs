using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///* Handles Updates for normal enemies in current group / blender,
///  Also Handles Pools Type or Singleton Type of Damage Particles for normal enemies.
///  Contain the Patrol Routes of all normal enemies.
///  NOTE: Fixed Tick, Tick, Late Tick. will be called in 'AI Session Manager'.

namespace SA
{
    public class AIGroupManagable : MonoBehaviour, AI_Managable
    {
        [Header("AI Session Manager.")]
        [ReadOnlyInspector] public AISessionManager _aiSessionManager;

        [Header("AI Boss Entrance.")]
        public AIBossEntrance _aiBossEntrance;

        [Header("AI Groups.")]
        public AI_Group[] groupsInSession;

        [Header("AI PatrolRoutes.")]
        public AI_PatrolRoute[] patrolRoutesInSession;

        [Header("AI Blenders.")]
        public AI_Blender[] blendersInSession;

        [Header("AI Connectors.")]
        public AI_Connector[] connectorsInSession;

        [Header("AI Updatable.")]
        public AI_Updatable _currentUpdatable;

        [Header("Active Group.")]
        public int _checkActiveGroupInterval = 10;
        public float _activeGroupBuffer = 2f;

        [Header("Status.")]
        [ReadOnlyInspector] public bool isForbiddenToFoundPlayer;
        [ReadOnlyInspector] public bool _isWithinBlender;
        [ReadOnlyInspector] public bool _isWithinConnector;
        [ReadOnlyInspector] public AI_Blender _currentActiveBlender;
        [ReadOnlyInspector] public AI_Group _currentActiveGroup;

        [Header("Aggros Amount.")]
        [ReadOnlyInspector] public int _currentAggroAmount;
        [ReadOnlyInspector] public bool _isAggroEmpty;
        [ReadOnlyInspector] public List<AIStateManager> aggrosInSession;

        [Header("Remnant Amount.")]
        [ReadOnlyInspector] public int _currentRemnantAmount;
        [ReadOnlyInspector] public bool _isRemnantEmpty;
        [ReadOnlyInspector] public List<AIStateManager> remnantInSession;
        
        #region Non Serialzied.
        [ReadOnlyInspector] public int _groupsLength;

        Dictionary<int, AI_PatrolRoute> _patrolRoutesDict = new Dictionary<int, AI_PatrolRoute>();
        public Dictionary<int, AI_Group> _groupDict = new Dictionary<int, AI_Group>();
        List<AI_Group> _tempAIGroupList = new List<AI_Group>();
        #endregion

        #region Awake.
        public void Init(AISessionManager _aiSessionManager)
        {
            //Debug.Log("AI Group Managable Init.");
            this._aiSessionManager = _aiSessionManager;

            InitAIGroups();
            InitGetFirstActiveGroup();
            InitDicts();
        }

        void InitAIGroups()
        {
            _groupsLength = groupsInSession.Length;
            for (int i = 0; i < _groupsLength; i++)
            {
                groupsInSession[i]._aiGroupManagable = this;
                groupsInSession[i]._playerStates = _aiSessionManager._playerState;
                groupsInSession[i].Init();
                
                /// Add Group to Dictionary.
                _groupDict.Add(groupsInSession[i]._groupId, groupsInSession[i]);
            }
        }

        void InitGetFirstActiveGroup()
        {
            SetCurrentActiveGroup(GetClosetGroup());
            _currentActiveGroup.OnCurrentActiveGroup();

            InitActivateGroups();

            void InitActivateGroups()
            {
                RefillTempAIGroupList();

                /// For active group..
                _currentActiveGroup.ActivateGroupCompletely();
                _tempAIGroupList.Remove(_currentActiveGroup);

                /// For groups that are adjacent to current Active Group.
                if (_currentActiveGroup._1stAdjacentGroupId != -1)
                {
                    AI_Group _1stAdjacentGroup = GetAIGroupById(_currentActiveGroup._1stAdjacentGroupId);
                    _1stAdjacentGroup.InitActivateAdjcentGroup();
                    _tempAIGroupList.Remove(_1stAdjacentGroup);
                }

                if (_currentActiveGroup._2ndAdjacentGroupId != -1)
                {
                    AI_Group _2ndAdjacentGroup = GetAIGroupById(_currentActiveGroup._2ndAdjacentGroupId);
                    _2ndAdjacentGroup.InitActivateAdjcentGroup();
                    _tempAIGroupList.Remove(_2ndAdjacentGroup);
                }

                /// For Others...
                int _listCount = _tempAIGroupList.Count;
                for (int i = 0; i < _listCount; i++)
                {
                    _tempAIGroupList[i].InitDeactivateNonRelatedGroup();
                }
            }
        }

        void InitDicts()
        {
            #region AI_Patrol Routes.
            _patrolRoutesDict.Clear();

            for (int i = 0; i < patrolRoutesInSession.Length; i++)
            {
                if (!_patrolRoutesDict.ContainsKey(patrolRoutesInSession[i].patrolListId))
                {
                    _patrolRoutesDict.Add(patrolRoutesInSession[i].patrolListId, patrolRoutesInSession[i]);
                }
            }
            #endregion
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            //Debug.Log("AI Group Managable Setup.");
           
            SetupRemnantAggroEmptyStatus();
            SetupAIGroups();
            SetupAIBlenders();
            SetupAIConnectors();
            SetupCheckEnemySaveFile();
            SetupAIBossEntrance();
        }

        void SetupRemnantAggroEmptyStatus()
        {
            _isRemnantEmpty = true;
            _isAggroEmpty = true;
        }

        void SetupAIGroups()
        {
            for (int i = 0; i < _groupsLength; i++)
            {
                groupsInSession[i].Setup();
            }
        }

        void SetupAIBlenders()
        {
            for (int i = 0; i < blendersInSession.Length; i++)
            {
                blendersInSession[i].Setup(this);
            }
        }

        void SetupAIConnectors()
        {
            for (int i = 0; i < connectorsInSession.Length; i++)
            {
                connectorsInSession[i].Setup(this);
            }
        }

        void SetupCheckEnemySaveFile()
        {
            SavableManager _savableManager = _aiSessionManager._savableManager;
            MainSaveFile _fileToLoad = null;

            if (_savableManager.isContinueGame)
            {
                _fileToLoad = _savableManager._prev_MainSavedFile;
            }
            else
            {
                _fileToLoad = _savableManager._spec_MainSavedFile;
            }

            LoadStatesFromSave();

            void LoadStatesFromSave()
            {
                List<SavableEnemyState> savedEnemyStates = _fileToLoad.savedEnemyStates;
                List<AIStateManager> savedAIStates = _savableManager._aIStates;

                Dictionary<string, SavableEnemyState> savedSavableDict = new Dictionary<string, SavableEnemyState>();

                int savedEnemyStatesCount = savedEnemyStates.Count;
                for (int i = 0; i < savedEnemyStatesCount; i++)
                {
                    savedSavableDict.Add(savedEnemyStates[i].savableId, savedEnemyStates[i]);
                }

                int savedAIStatesCount = savedAIStates.Count;
                for (int i = 0; i < savedAIStatesCount; i++)
                {
                    savedSavableDict.TryGetValue(savedAIStates[i].savableId, out SavableEnemyState _savedState);
                    if (_savedState != null)
                    {
                        savedAIStates[i].LoadEnemyStateFromSave(_savedState);
                    }
                }
            }
        }

        void SetupAIBossEntrance()
        {
            if (!_aiSessionManager._aiBossManagable._isBossKilled)
            {
                SetupActivateBossEntrance();
                _aiBossEntrance.Setup(this);
            }
        }
        #endregion

        #region Fixed Tick.
        public void FixedTick()
        {
            UpdatableFixedTick();
            AggrosUpdatableFixedTick();
            RemnantsUpdatableFixedTick();
        }
        
        void UpdatableFixedTick()
        {
            _currentUpdatable.FixedTick();
        }

        void AggrosUpdatableFixedTick()
        {
            if (!_isAggroEmpty)
            {
                for (int i = 0; i < _currentAggroAmount; i++)
                {
                    aggrosInSession[i].FixedTick();
                }
            }
        }

        void RemnantsUpdatableFixedTick()
        {
            if (!_isRemnantEmpty)
            {
                for (int i = 0; i < _currentRemnantAmount; i++)
                {
                    remnantInSession[i].FixedTick();
                }
            }
        }
        #endregion

        #region Tick.
        public void Tick()
        {
            RefreshActiveGroupOnInterval();

            UpdatableTick();
            AggrosUpdatableTick();

            MonitorRemnants();
        }

        void UpdatableTick()
        {
            _currentUpdatable.Tick();
        }
        
        void AggrosUpdatableTick()
        {
            if (!_isAggroEmpty)
            {
                for (int i = 0; i < _currentAggroAmount; i++)
                {
                    aggrosInSession[i].Tick();
                }
            }
        }

        void MonitorRemnants()
        {
            if (!_isRemnantEmpty)
            {
                for (int i = 0; i < _currentRemnantAmount; i++)
                {
                    // If remnants separate from player far enough
                    remnantInSession[i].RemoveRemnantAfterDistance();
                }
            }
        }
        #endregion

        #region Late Tick.
        public void LateTick()
        {
            UpdatableLateTick();
            AggrosUpdatableLateTick();
            RemnantsUpdatableLateTick();
        }

        void UpdatableLateTick()
        {
            _currentUpdatable.LateTick();
        }
        
        void AggrosUpdatableLateTick()
        {
            if (!_isAggroEmpty)
            {
                for (int i = 0; i < _currentAggroAmount; i++)
                {
                    aggrosInSession[i].LateTick();
                }
            }
        }

        void RemnantsUpdatableLateTick()
        {
            if (!_isRemnantEmpty)
            {
                for (int i = 0; i < _currentRemnantAmount; i++)
                {
                    remnantInSession[i].LateTick();
                }
            }
        }
        #endregion

        #region On Boss Sequence Start.
        public void OnBossSequenceStart()
        {
            /// Create Smoke Wall.
            OnSequenceStart_DeactivateBossEntrance();
            OnSequenceStart_DeactivateGroups();
        }

        void OnSequenceStart_DeactivateGroups()
        {
            _currentActiveGroup.DeactivateGroupCompletely_Instant();

            if (_currentActiveGroup._1stAdjacentGroupId != -1)
                GetAIGroupById(_currentActiveGroup._1stAdjacentGroupId).DeactivateGroupCompletely_Instant();

            if (_currentActiveGroup._2ndAdjacentGroupId != -1)
                GetAIGroupById(_currentActiveGroup._2ndAdjacentGroupId).DeactivateGroupCompletely_Instant();

            //for (int i = 0; i < _groupsLength; i++)
            //{
            //    if (groupsInSession[i] != _currentActiveGroup)
            //    {
            //        if (groupsInSession[i]._groupId == _currentActiveGroup._1stAdjacentGroupId || groupsInSession[i]._groupId == _currentActiveGroup._2ndAdjacentGroupId)
            //        {
            //            groupsInSession[i].DeactivateGroupCompletely_Instant();
            //        }
            //    }
            //}
        }
        #endregion

        #region On Boss Fight Ended.
        public void OnBossFightEnded_ActivateGroups()
        {
            _currentActiveGroup.ActivateGroupCompletely();

            if (_currentActiveGroup._1stAdjacentGroupId != -1)
                GetAIGroupById(_currentActiveGroup._1stAdjacentGroupId).ActivateGroupCompletely();

            if (_currentActiveGroup._2ndAdjacentGroupId != -1)
                GetAIGroupById(_currentActiveGroup._2ndAdjacentGroupId).ActivateGroupCompletely();
        }
        #endregion
        
        #region Refresh.
        void RefreshActiveGroupOnInterval()
        {
            if (!_isWithinBlender)
            {
                if (_aiSessionManager._frameCount % _checkActiveGroupInterval == 0)
                {
                    RefreshCurrentActiveGroup();
                }
            }
        }

        void RefreshCurrentActiveGroup()
        {
            AI_Group _closestGroup = GetClosetGroup();
            if (!_closestGroup._isActiveGroup)
            {
                _currentActiveGroup.OffCurrentActiveGroup();
                SetCurrentActiveGroup(_closestGroup);
                _currentActiveGroup.OnCurrentActiveGroup();
                
                RefreshActivateGroups();
            }
        }

        public void RefreshActivateGroups()
        {
            RefillTempAIGroupList();

            /// For active group..
            _currentActiveGroup.ActivateGroupCompletely();
            _tempAIGroupList.Remove(_currentActiveGroup);

            /// For groups that are adjacent to current Active Group.
            int _1stAdjacentGroupId = _currentActiveGroup._1stAdjacentGroupId;
            int _2ndAdjacentGroupId = _currentActiveGroup._2ndAdjacentGroupId;

            if (_1stAdjacentGroupId != -1)
            {
                GetAIGroupById(_1stAdjacentGroupId).ActivateGroupDeactivateAnimDelay();
                _tempAIGroupList.Remove(groupsInSession[_1stAdjacentGroupId - 1]);
            }

            if (_2ndAdjacentGroupId != -1)
            {
                GetAIGroupById(_2ndAdjacentGroupId).ActivateGroupDeactivateAnimDelay();
                _tempAIGroupList.Remove(groupsInSession[_2ndAdjacentGroupId - 1]);
            }

            /// For Others...
            int _listCount = _tempAIGroupList.Count;
            for (int i = 0; i < _listCount; i++)
            {
                _tempAIGroupList[i].DeactivateGroupCompletely_Instant();
            }
        }

        public void SetCurrentActiveGroup(AI_Group _aiGroup)
        {
            _currentActiveGroup = _aiGroup;
            _currentUpdatable = _aiGroup;
        }

        public void SetCurrentActiveBlender(AI_Blender _aiBlender)
        {
            _currentActiveBlender = _aiBlender;
            _currentUpdatable = _aiBlender;
        }

        void RefillTempAIGroupList()
        {
            _tempAIGroupList.Clear();
            for (int i = 0; i < _groupsLength; i++)
            {
                _tempAIGroupList.Add(groupsInSession[i]);
            }
        }
        #endregion
        
        #region Add / Remove Aggros.
        public void DecrementAggrosCount(AIStateManager _aiState)
        {
            aggrosInSession.Remove(_aiState);
            _currentAggroAmount--;

            if (_currentAggroAmount == 0)
            {
                _isAggroEmpty = true;
                _aiSessionManager._playerState._commentHandler.ResumeAcceptingComment();
            }
        }

        public void IncrementAggrosCount(AIStateManager _aiState)
        {
            aggrosInSession.Add(_aiState);
            _currentAggroAmount++;

            if (_isAggroEmpty)
            {
                _isAggroEmpty = false;
                _aiSessionManager._playerState._commentHandler.PauseAcceptingComment();
            }
        }
        #endregion

        #region Add / Remove Remnants.
        public void RemoveFromRemnants(AIStateManager _aiStates)
        {
            remnantInSession.Remove(_aiStates);

            _currentRemnantAmount--;

            if (_currentRemnantAmount == 0)
            {
                _isRemnantEmpty = true;
            }
        }

        public void AddToRemnants(AIStateManager _aiStates)
        {
            remnantInSession.Add(_aiStates);

            _currentRemnantAmount++;

            _isRemnantEmpty = false;
        }
        #endregion

        #region Enter / Exit Blender.
        public void OnEnterBlender()
        {
            _isWithinBlender = true;
        }

        public void OnExitBlender()
        {
            _isWithinBlender = false;
            _currentActiveBlender = null;
        }
        #endregion

        #region AI Boss Entrances.

        #region Activate / Deactivate Entrances.
        public void SetupActivateBossEntrance()
        {
            _aiBossEntrance.gameObject.SetActive(true);
        }

        public void OnSequenceStart_DeactivateBossEntrance()
        {
            _aiBossEntrance.gameObject.SetActive(false);
        }
        #endregion

        #region Forced Aggros Return To Patrol.
        public void ForcedAggrosReturnToPatrol()
        {
            isForbiddenToFoundPlayer = true;

            for (int i = _currentAggroAmount; i > 0; i--)
            {
                aggrosInSession[i - 1].aiManager.ForceExitAggroToPatrol();
            }
        }

        public void AllowedToTurnAggroAgain()
        {
            isForbiddenToFoundPlayer = false;
        }
        #endregion

        #endregion

        public AI_Group GetClosetGroup()
        {
            float _leastDistance = 50000;
            AI_Group _closestGroup = null;

            for (int i = 0; i < _groupsLength; i++)
            {
                groupsInSession[i].GetDistanceToPlayer();
                if (groupsInSession[i]._disToPlayer < _leastDistance)
                {
                    _leastDistance = groupsInSession[i]._disToPlayer;
                    _closestGroup = groupsInSession[i];
                }
            }

            return _closestGroup;
        }

        public bool GetIsGroupAdjcentToActiveGroup(int _groupId)
        {
            return _groupId == _currentActiveGroup._1stAdjacentGroupId || _groupId == _currentActiveGroup._2ndAdjacentGroupId;
        }

        public bool GetIsGroupWithinActiveBlender(AI_Group _group)
        {
            return _group == _currentActiveBlender._1stGroupToBlend || _group == _currentActiveBlender._2ndGroupToBlend;
        }

        public AI_PatrolRoute GetAIPatrolRouteById(int _id)
        {
            _patrolRoutesDict.TryGetValue(_id, out AI_PatrolRoute _aiPatrolRoute);
            return _aiPatrolRoute;
        }

        public AI_Group GetAIGroupById(int _id)
        {
            _groupDict.TryGetValue(_id, out AI_Group _aiGroup);
            return _aiGroup;
        }
    }

    public interface AI_Updatable
    {
        void FixedTick();

        void Tick();

        void LateTick();
    }
}