using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class DefinitionHub : MonoBehaviour
    {
        [Header("Definition Refs.")]
        [ReadOnlyInspector, SerializeField] Canvas _currentDefineDetail;

        #region On / Off Define Detail.
        /// Call this function each time when cursor pointing different attributes/stats.
        public void OnDefineDetail(Canvas _targetDetail)
        {
            DisableDefineDetail();
            SetCurrentDefineDetail(_targetDetail);
            EnableDefineDetail();
        }
        #endregion

        #region Set Current Define Detail.
        void SetCurrentDefineDetail(Canvas _targetDetail)
        {
            _currentDefineDetail = _targetDetail;
        }
        #endregion

        #region Show / Hide Define Detail.
        void EnableDefineDetail()
        {
            _currentDefineDetail.enabled = true;
        }

        void DisableDefineDetail()
        {
            _currentDefineDetail.enabled = false;
        }
        #endregion

        #region Setup.
        public void SetupDefineDetail(Canvas _targetDetail)
        {
            SetCurrentDefineDetail(_targetDetail);
        }
        #endregion
    }
}