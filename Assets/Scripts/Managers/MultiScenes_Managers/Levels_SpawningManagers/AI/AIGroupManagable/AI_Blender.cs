using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_Blender : MonoBehaviour, AI_Updatable
    {
        [Header("Config.")]
        public AI_Group _1stGroupToBlend;
        public AI_Group _2ndGroupToBlend;
        public Collider _blenderCollider;
        
        [Header("Refs.")]
        [ReadOnlyInspector] public AIGroupManagable _aiGroupManagable;

        List<AI_Group> _tempAIGroupList = new List<AI_Group>();

        public void Setup(AIGroupManagable _aiGroupManagable)
        {
            this._aiGroupManagable = _aiGroupManagable;
            
            gameObject.layer = LayerManager.singleton.enemyBlenderLayer;

            _blenderCollider.enabled = true;
        }

        public void FixedTick()
        {
            _1stGroupToBlend.FixedUpdateEnemiesInGroup();
            _2ndGroupToBlend.FixedUpdateEnemiesInGroup();
        }
        
        public void Tick()
        {
            //Debug.Log("UpdateEnemiesInBlender 1");
            _1stGroupToBlend.UpdateEnemiesInGroup();
            _2ndGroupToBlend.UpdateEnemiesInGroup();
        }
        
        public void LateTick()
        {
            _1stGroupToBlend.LateUpdateEnemiesInGroup();
            _2ndGroupToBlend.LateUpdateEnemiesInGroup();
        }

        #region Callbacks.
        private void OnTriggerEnter(Collider other)
        {
            if (_aiGroupManagable._isWithinConnector)
            {
                _aiGroupManagable.SetCurrentActiveBlender(this);
                return;
            }
            
            OnBlenderEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if (_aiGroupManagable._isWithinConnector || !_aiGroupManagable._isWithinBlender)
                return;
            
            /// If player has entered another blender before leaving previous one.
            if (_aiGroupManagable._currentActiveBlender != this)
                return;

            //Debug.Log("On Blender Exit");
            OnBlenderExit();
        }
        #endregion

        #region Enter / Exit Blender.
        void OnBlenderEnter()
        {
            OnEnterSetStatus();
        }

        void OnBlenderExit()
        {
            OnExitSetStatus();
        }
        #endregion

        #region On Enter.
        void OnEnterSetStatus()
        {
            /// Assume player entered blender from active group.
            if (!_aiGroupManagable._isWithinBlender)
            {
                OnActiveGroupEnterRefreshGroups();
            }
            /// Assume player entered blender from another active blender without AI_Connector.
            else
            {
                OnActiveBlenderEnterRefreshGroups();
            }

            _aiGroupManagable.OnEnterBlender();
            _aiGroupManagable.SetCurrentActiveBlender(this);
        }
        
        void OnActiveGroupEnterRefreshGroups()
        {
            /// Off Current Active Group.
            _aiGroupManagable._currentActiveGroup.OffCurrentActiveGroup();

            RefillTempAIGroupList();

            /// Activate the two current groups.
            _1stGroupToBlend.ActivateGroupCompletely();
            _1stGroupToBlend.OnCurrentActiveGroup();
            _tempAIGroupList.Remove(_1stGroupToBlend);

            _2ndGroupToBlend.ActivateGroupCompletely();
            _2ndGroupToBlend.OnCurrentActiveGroup();
            _tempAIGroupList.Remove(_2ndGroupToBlend);

            int _listCount = _tempAIGroupList.Count;
            for (int i = 0; i < _listCount; i++)
            {
                _tempAIGroupList[i].DeactivateGroupCompletely_Instant();
            }
        }

        void OnActiveBlenderEnterRefreshGroups()
        {
            AI_Blender _previousActiveBlender = _aiGroupManagable._currentActiveBlender;

            OnEnterCheckPreviousBlenderGroup(_previousActiveBlender._1stGroupToBlend);
            OnEnterCheckPreviousBlenderGroup(_previousActiveBlender._2ndGroupToBlend);

            _1stGroupToBlend.ActivateGroupCompletely();
            _2ndGroupToBlend.ActivateGroupCompletely();

            _1stGroupToBlend.OnCurrentActiveGroup();
            _2ndGroupToBlend.OnCurrentActiveGroup();

            void OnEnterCheckPreviousBlenderGroup(AI_Group _previousBlenderGroup)
            {
                /// If previous blender group Not the Same as 1st group to blender and 2nd group to blender...
                if (_previousBlenderGroup != _1stGroupToBlend && _previousBlenderGroup != _2ndGroupToBlend)
                {
                    _previousBlenderGroup.DeactivateGroupCompletely_Instant();
                    _previousBlenderGroup.OffCurrentActiveGroup();
                }
            }
        }
        
        void RefillTempAIGroupList()
        {
            _tempAIGroupList.Clear();
            for (int i = 0; i < _aiGroupManagable._groupsLength; i++)
            {
                _tempAIGroupList.Add(_aiGroupManagable.groupsInSession[i]);
            }
        }
        #endregion

        #region On Exit.
        void OnExitSetStatus()
        {
            _aiGroupManagable.OnExitBlender();
            OnExitRefreshClosetGroup();
        }
        
        void OnExitRefreshClosetGroup()
        {
            AI_Group _closetGroup = _aiGroupManagable.GetClosetGroup();
            /// 1st group is the closet.
            if (_closetGroup._groupId == _1stGroupToBlend._groupId)
            {
                _2ndGroupToBlend.OffCurrentActiveGroup();
                _aiGroupManagable.SetCurrentActiveGroup(_1stGroupToBlend);
            }
            /// 2nd group is the closet.
            else if (_closetGroup._groupId == _2ndGroupToBlend._groupId)
            {
                _1stGroupToBlend.OffCurrentActiveGroup();
                _aiGroupManagable.SetCurrentActiveGroup(_2ndGroupToBlend);
            }
            /// none of them is close.
            else
            {
                _1stGroupToBlend.OffCurrentActiveGroup();
                _2ndGroupToBlend.OffCurrentActiveGroup();
                _aiGroupManagable.SetCurrentActiveGroup(_closetGroup);
            }
            
            _aiGroupManagable.RefreshActivateGroups();
        }
        #endregion
    }
}