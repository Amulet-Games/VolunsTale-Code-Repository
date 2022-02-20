using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AI_Connector : MonoBehaviour
    {
        [Header("Config.")]
        public Collider _connectorCollider;

        [Header("Refs.")]
        [ReadOnlyInspector] public AIGroupManagable _aiGroupManagable;
        [ReadOnlyInspector] public LayerManager _layerManager;

        #region Callbacks.
        private void OnTriggerEnter(Collider other)
        {
            if (!_aiGroupManagable._isWithinBlender)
                return;

            //Debug.Log("On Connector Enter");
            OnConnectorEnter();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_aiGroupManagable._isWithinBlender)
                return;

            //Debug.Log("On Connector Exit");
            OnConnectorExit();
        }
        #endregion

        #region On Enter.
        void OnConnectorEnter()
        {
            _aiGroupManagable._isWithinConnector = true;
        }
        #endregion

        #region On Exit.
        void OnConnectorExit()
        {
            _aiGroupManagable._isWithinConnector = false;
        }
        #endregion

        #region Setup.
        public void Setup(AIGroupManagable _aiGroupManagable)
        {
            this._aiGroupManagable = _aiGroupManagable;

            _layerManager = LayerManager.singleton;
            gameObject.layer = _layerManager.enemyBlenderLayer;

            _connectorCollider.enabled = true;
        }
        #endregion
    }
}