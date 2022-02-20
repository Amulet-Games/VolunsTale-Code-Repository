using System.Collections.Generic;
using UnityEngine;

///* Responsible for activate or deactivate the enemies in this group.
///* Responsible to handle over the enemies to AI Session Manager for updates.


namespace SA
{
    public class AI_Group : MonoBehaviour, AI_Updatable
    {
        [Header("Config.")]
        public int _groupId;
        [Tooltip("Note that adjacent group doesn't mean geometrically adjacent to each other, set this value base on the which group you think it stay activate.")]
        public int _1stAdjacentGroupId;
        [Tooltip("Note that adjacent group doesn't mean geometrically adjacent to each other, set this value base on the which group you think it stay activate.")]
        public int _2ndAdjacentGroupId;
        public AIStateManager[] _enemiesInGroup;

        [Header("Status.")]
        [ReadOnlyInspector] public float _disToPlayer;
        [ReadOnlyInspector] public bool _isActiveGroup;
        [ReadOnlyInspector] public bool _isGroupObjDeactivated;
        [ReadOnlyInspector] public bool _isGroupAnimDeactivated;

        [Header("Updatable Enemies.")]
        [ReadOnlyInspector] public List<AIStateManager> _updatables;

        [Header("Refs.")]
        [ReadOnlyInspector] public StateManager _playerStates;
        [ReadOnlyInspector] public AIGroupManagable _aiGroupManagable;
        [ReadOnlyInspector] public Transform _mTransform;

        #region Non Serialized.
        [ReadOnlyInspector, SerializeField] int _enemiesLength;
        [ReadOnlyInspector, SerializeField] int _updatableCount;
        #endregion

        public void FixedTick()
        {
            FixedUpdateEnemiesInGroup();
        }

        public void Tick()
        {
            UpdateEnemiesInGroup();
        }

        public void LateTick()
        {
            LateUpdateEnemiesInGroup();
        }

        #region On / Off Current Active Group.
        public void OnCurrentActiveGroup()
        {
            OnActiveGroupResetBools();
        }

        public void OffCurrentActiveGroup()
        {
            OffActiveGroupResetBools();
        }

        void OnActiveGroupResetBools()
        {
            _isActiveGroup = true;
        }
        
        void OffActiveGroupResetBools()
        {
            _isActiveGroup = false;
        }
        #endregion

        #region Add / Remove Updatables.
        public void AddToUpdatables(AIStateManager _aiStates)
        {
            _updatables.Add(_aiStates);
            _updatableCount++;
        }

        public void RemoveFromUpdatables(AIStateManager _aiStates)
        {
            _updatables.Remove(_aiStates);
            _updatableCount--;
        }
        #endregion
        
        #region Activate / Deactivate.
        public void ActivateGroupObj()
        {
            if (_isGroupObjDeactivated)
            {
                for (int i = 0; i < _enemiesLength; i++)
                {
                    _enemiesInGroup[i].ActivateEnemyObj();
                    _enemiesInGroup[i].OnActiveGroupRefreshEnemy();
                }

                _isGroupObjDeactivated = false;
            }
        }
        
        public void ActivateGroupAnim()
        {
            if (_isGroupAnimDeactivated)
            {
                //Debug.Log("Activated = " + gameObject.name);
                for (int i = 0; i < _enemiesLength; i++)
                {
                    _enemiesInGroup[i].ActivateEnemyAnim();
                }

                _isGroupAnimDeactivated = false;
            }
        }

        public void ActivateGroupDeactivateAnimDelay()
        {
            ActivateGroupObj();
            DeactivateGroupAnimDelay();
        }

        public void ActivateGroupDeactivateAnimInstant()
        {
            ActivateGroupObj();
            DeactivateGroupAnimInstant();
        }

        public void ActivateGroupCompletely()
        {
            ActivateGroupObj();
            ActivateGroupAnim();
        }

        public void DeactivateGroupObj()
        {
            if (!_isGroupObjDeactivated)
            {
                for (int i = 0; i < _enemiesLength; i++)
                {
                    _enemiesInGroup[i].DeactivateEnemyObj();
                }

                _isGroupObjDeactivated = true;
            }
        }

        public void DeactivateGroupAnimInstant()
        {
            //Debug.Log("deactivated = " + gameObject.name);
            if (!_isGroupAnimDeactivated)
            {
                for (int i = 0; i < _enemiesLength; i++)
                {
                    _enemiesInGroup[i].aiManager.DeactivateEnemyAnimInstant();
                }

                _isGroupAnimDeactivated = true;
            }
        }

        public void DeactivateGroupAnimDelay()
        {
            //Debug.Log("deactivated = " + gameObject.name);
            if (!_isGroupAnimDeactivated)
            {
                for (int i = 0; i < _enemiesLength; i++)
                {
                    _enemiesInGroup[i].aiManager.DeactivateEnemyAnimDelay();
                }

                _isGroupAnimDeactivated = true;
            }
        }

        public void DeactivateGroupCompletely_Instant()
        {
            DeactivateGroupObj();
            DeactivateGroupAnimInstant();
        }

        public void DeactivateGroupCompletely_Delay()
        {
            DeactivateGroupObj();
            DeactivateGroupAnimDelay();
        }
        #endregion

        #region Init.
        public void Init()
        {
            InitReferences();
            InitStatus();
            InitEnemiesInGroup();
        }

        void InitReferences()
        {
            _mTransform = transform;
        }

        void InitStatus()
        {
            _isActiveGroup = false;
            _isGroupObjDeactivated = false;
            _isGroupAnimDeactivated = false;
        }

        void InitEnemiesInGroup()
        {
            _enemiesLength = _enemiesInGroup.Length;

            if (SavableManager.singleton.isNewGame)
            {
                for (int i = 0; i < _enemiesLength; i++)
                {
                    AddToUpdatables(_enemiesInGroup[i]);
                    _enemiesInGroup[i]._belongedAIGroup = this;
                    _enemiesInGroup[i].Init();
                }
            }
            else
            {
                for (int i = 0; i < _enemiesLength; i++)
                {
                    _enemiesInGroup[i]._belongedAIGroup = this;
                    _enemiesInGroup[i].Init();
                }
            }
        }
        
        public void InitActivateAdjcentGroup()
        {
            ActivateGroupObj();
        }

        public void InitDeactivateNonRelatedGroup()
        {
            DeactivateGroupObj();
        }
        #endregion

        #region Setup.
        public void Setup()
        {
            SetupEnemiesInGroup();
        }

        void SetupEnemiesInGroup()
        {
            for (int i = 0; i < _enemiesLength; i++)
            {
                _enemiesInGroup[i].Setup();
            }
        }
        #endregion

        #region Fixed Tick.
        public void FixedUpdateEnemiesInGroup()
        {
            for (int i = 0; i < _updatableCount; i++)
            {
                _updatables[i].FixedTick();
            }
        }
        #endregion

        #region Tick.
        public void GetDistanceToPlayer()
        {
            Vector3 _dirToPlayer = _playerStates.mTransform.position - _mTransform.localPosition;
            _dirToPlayer.y = 0;
            _disToPlayer = Vector3.SqrMagnitude(_dirToPlayer);
            _disToPlayer = _isActiveGroup ? _disToPlayer + _aiGroupManagable._activeGroupBuffer : _disToPlayer;
        }

        public void UpdateEnemiesInGroup()
        {
            for (int i = 0; i < _updatableCount; i++)
            {
                _updatables[i].Tick();
            }
        }
        #endregion

        #region Late Tick.
        public void LateUpdateEnemiesInGroup()
        {
            for (int i = 0; i < _updatableCount; i++)
            {
                _updatables[i].LateTick();
            }
        }
        #endregion
    }
}