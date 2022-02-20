using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class CameraHandler : MonoBehaviour
    {
        #region Rotate Actions.
        [Header("ICameraRotate Managables.")]
        public CamFreemodeRotateManagable freemodeRotateManagable;
        public CamLockonRotateManagable lockonRotateManagable;
        public ICameraRotateManagable _iCamRotationManagable;
        #endregion

        #region Collision Actions.
        [Header("ICameraCollision Managables.")]
        public FreeCamCollisionManagable freeCamCollisionManagable;
        public PauseCamCollisionManagable pauseCamCollisionManagable;
        public ICameraCollisionManagable _iCamCollisionManagable;
        #endregion

        #region Camera Actions.
        [Header("Camera Actions.")]
        public CameraAction[] tick_Actions;
        public CameraAction[] fixedTick_Actions;
        #endregion

        #region Axis Input.
        [Header("Axis Input.")]
        [ReadOnlyInspector] public float mouseX;
        [ReadOnlyInspector] public float mouseY;
        #endregion

        #region Mouse Lock Mode.
        CursorLockMode currentCursorLockMode;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public float _currentXAngle;
        [ReadOnlyInspector] public float _currentYAngle;
        [ReadOnlyInspector] public float _fixedDelta;
        [ReadOnlyInspector] public float _delta;
        #endregion

        #region Collision Handling.
        [Header("Collision Handling.")]
        public bool _drawDebugLine = false;
        public float _cancelCollisionSphereRadius = 0.75f;
        public float _collisionCameraMoveYValue = -0.4f;
        public float _collisionMoveSmoothTime = 0.07f;
        public float _slowedCollisionMoveSmoothTime = 0.3f;
        public float _switchSmoothTimeDistanceThershold = 0.25f;
        [ReadOnlyInspector] public bool _isColliding;
        [ReadOnlyInspector] public float _currentCollisionMoveSmoothTime;
        [ReadOnlyInspector] public Vector3[] desiredCameraClipPoints;
        [ReadOnlyInspector] public Vector3 _cameraSmoothDampVelocity;
        [ReadOnlyInspector] public Vector3 _collisionDetectionPivot;
        [ReadOnlyInspector] public Collider[] _hitCollider = new Collider[1];
        [ReadOnlyInspector] public Transform _playerSpineTransform;
        #endregion

        #region World UI Camera.
        [Header("World UI Camera.")]
        public Camera worldUICamera;
        [ReadOnlyInspector] public int _worldUIElementsCount;
        [ReadOnlyInspector] public bool _isActivatingWorldUICamera;
        #endregion

        #region Refs.
        [Header("Refs.")]
        [ReadOnlyInspector] public StateManager states = null;
        [ReadOnlyInspector] public LayerManager _layerManager;
        [ReadOnlyInspector] public Camera mainCamera = null;
        public static CameraHandler singleton;
        #endregion

        #region Linked Transforms.
        [Header("Linked Transforms.")]
        [ReadOnlyInspector] public Transform _mTransform;
        [ReadOnlyInspector] public Transform _pivotTransform;
        [ReadOnlyInspector] public Transform _mainCameraTransform;
        [ReadOnlyInspector] public Transform _collisionCenterTransform;
        #endregion

        #region Camera Components Init Transform.
        [Header("Init Transforms.")]
        public CamComponentInitTransform _camHandler_initTransform;
        public CamComponentInitTransform _camPivot_initTransform;
        public CamComponentInitTransform _mainCam_initTransform;
        #endregion

        #region Private.
        [Header("Camera Actions Length.")]
        int tickActionsLength;
        int fixedTickActionsLength;
        #endregion
        
        public void Setup(StateManager _states)
        {
            states = _states;

            SetupSingleton();

            SetupLinkedTransform();

            SetupRefs();
            
            SetupCameraSmoothDampVelocity();

            SwitchToFreemodeRotateManagable();

            SwitchToFreeCamCollisionManagable();
            
            SetupPlayerWorldCanvas();

            SetupWorldUICamera();

            SetupCameraCollisionClipPoints();

            GetActionsLength();
        }

        public void Tick()
        {
            ExecuteTickActions();

            _iCamCollisionManagable.Execute();
        }

        public void FixedTick()
        {
            _iCamRotationManagable.Execute(this);

            ExecuteFixedTickActions();
        }

        #region Rest Inputs.
        public void Off_MainHud_SetInputStatus()
        {
            LockScreenCursor();
        }

        #region Selection Menu.
        public void On_SelectionMenu_SetInputStatus()
        {
            mouseX = 0;
            mouseY = 0;

            UnlockScreenCursor();
        }
        #endregion

        #region Checkpoint.
        public void On_CheckpointMenu_SetInputStatus()
        {
            UnlockScreenCursor();
        }

        public void Off_CheckpointMenu_SetInputStatus()
        {
            mouseX = 0;
        }
        #endregion

        #region Leveling Menu.
        public void On_LevelingMenu_SetInputStatus()
        {
            mouseX = 0;
        }
        #endregion

        public void PausePlayerInput_CampStart()
        {
            mouseX = 0;
            mouseY = 0;
        }

        public void PausePlayerInput_OnDeath()
        {
            mouseX = 0;
            mouseY = 0;
        }

        public void ResetCurrentAngles_OnDeath()
        {
            _currentXAngle = 0;
            _currentYAngle = 0;
        }

        #region Screen Cursor.
        public void UnlockScreenCursor()
        {
            currentCursorLockMode = CursorLockMode.None;
            Cursor.lockState = currentCursorLockMode;
            Cursor.visible = true;
        }

        void LockScreenCursor()
        {
            currentCursorLockMode = CursorLockMode.Locked;
            Cursor.lockState = currentCursorLockMode;
            Cursor.visible = false;
        }
        #endregion

        #endregion

        #region Setup.
        void SetupSingleton()
        {
            if (singleton != null)
                Destroy(this);
            else
                singleton = this;
        }

        void SetupLinkedTransform()
        {
            _mTransform = transform;
            _pivotTransform = transform.GetChild(0);
            _mainCameraTransform = transform.GetChild(0).GetChild(0);
            _collisionCenterTransform = transform.GetChild(0).GetChild(1);
        }
        
        void SetupRefs()
        {
            states._camHandler = this;

            _playerSpineTransform = states.spineTransform;

            mainCamera = _mainCameraTransform.GetComponent<Camera>();

            _layerManager = LayerManager.singleton;
        }

        void SetupCameraSmoothDampVelocity()
        {
            _cameraSmoothDampVelocity = states.vector3Zero;
        }
        
        void SetupPlayerWorldCanvas()
        {
            states._commentHandler.SetupByCamHandler(this);
        }

        void SetupWorldUICamera()
        {
            _worldUIElementsCount = 0;
            DeactivateWorldUICamera();
        }

        void SetupCameraCollisionClipPoints()
        {
            desiredCameraClipPoints = new Vector3[5];
        }

        void GetActionsLength()
        {
            tickActionsLength = tick_Actions.Length;
            fixedTickActionsLength = fixedTick_Actions.Length;
        }
        #endregion

        #region Tick.
        public void UpdateInputs_Main()
        {
            mouseX = Input.GetAxis("mouseX");
            mouseY = Input.GetAxis("mouseY");
        }

        public void UpdateInputs_CheckpointMenu()
        {
            mouseX = Input.GetAxis("menu_camHandler_horizontal");
        }

        public void ExecuteTickActions()
        {
            for (int i = 0; i < tickActionsLength; i++)
            {
                tick_Actions[i].Execute(this);
            }
        }
        #endregion

        #region Fixed Tick.
        public void ExecuteFixedTickActions()
        {
            for (int i = 0; i < fixedTickActionsLength; i++)
            {
                fixedTick_Actions[i].Execute(this);
            }
        }
        #endregion

        #region Camera Collision.
        public void HandleCameraCollision()
        {
            UpdateCameraClipPoints();

            if (_drawDebugLine)
            {
                for (int i = 0; i < 5; i++)
                {
                    Debug.DrawLine(desiredCameraClipPoints[i], _playerSpineTransform.position, Color.magenta);
                }
            }

            CheckColliding();
            MoveCameraWhenCollided();
        }

        /// Updating the position of each corner on the clip plane in order to shoot a ray from it. 
        void UpdateCameraClipPoints()
        {
            /// Calculate near clip plane position.
            _collisionDetectionPivot = _collisionCenterTransform.position;
            
            float z = mainCamera.nearClipPlane;
            float x = Mathf.Tan(mainCamera.fieldOfView / 3.12f) * z;
            float y = x / mainCamera.aspect;

            /// top left
            desiredCameraClipPoints[0] = (_collisionCenterTransform.rotation * new Vector3(-x, y, 0)) + _collisionCenterTransform.position;
            /// top right
            desiredCameraClipPoints[1] = (_collisionCenterTransform.rotation * new Vector3(x, y, 0)) + _collisionCenterTransform.position;
            /// bottom left
            desiredCameraClipPoints[2] = (_collisionCenterTransform.rotation * new Vector3(-x, -y, 0)) + _collisionCenterTransform.position;
            /// bottom right
            desiredCameraClipPoints[3] = (_collisionCenterTransform.rotation * new Vector3(x, -y, 0)) + _collisionCenterTransform.position;
            /// Camera's position
            desiredCameraClipPoints[4] = _collisionDetectionPivot;
        }

        /// Constantly checking if camera is colliding or not.
        void CheckColliding()
        {
            if (CollisionDetectedAtClipPoints())
            {
                _isColliding = true;
            }
            else
            {
                if (_isColliding)
                {
                    int i = Physics.OverlapSphereNonAlloc(_collisionDetectionPivot, _cancelCollisionSphereRadius, _hitCollider, _layerManager._cameraCollisionIncludedMask);
                    if (i > 0)
                    {
                        return;
                    }
                    
                    _isColliding = false;
                }
            }

            /// Detect if there are collisions happen in between ray start from near clip plane to player.
            bool CollisionDetectedAtClipPoints()
            {
                for (int i = 0; i < 5; i++)
                {
                    Ray ray = new Ray(_playerSpineTransform.position, desiredCameraClipPoints[i] - _playerSpineTransform.position);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, -_collisionCenterTransform.localPosition.z, _layerManager._cameraCollisionIncludedMask))
                    {
                        //Debug.Log("hit Object = " + hit.transform.gameObject.name);
                        return true;
                    }
                }

                return false;
            }
        }
        
        void MoveCameraWhenCollided()
        {
            if (_isColliding)
            {
                #region Determine is to switch smooth time.
                float adjustedDistance = GetAdjustedDistanceWithRayFrom(); //Debug.Log("adjustedDistance = " + adjustedDistance);
                
                if ((adjustedDistance + _mainCameraTransform.localPosition.z * -1) > _switchSmoothTimeDistanceThershold)
                {
                    _currentCollisionMoveSmoothTime = _collisionMoveSmoothTime;
                }
                else
                {
                    _currentCollisionMoveSmoothTime = _slowedCollisionMoveSmoothTime;
                }
                #endregion

                Vector3 _collidedNewPos = new Vector3(0, _collisionCameraMoveYValue, adjustedDistance);

                /// Smooth Damp.
                _mainCameraTransform.localPosition = Vector3.SmoothDamp(_mainCameraTransform.localPosition, _collidedNewPos, ref _cameraSmoothDampVelocity, _currentCollisionMoveSmoothTime);
            }
            else
            {
                /// Smooth Damp.
                _mainCameraTransform.localPosition = Vector3.SmoothDamp(_mainCameraTransform.localPosition, _mainCam_initTransform._componentLocalPos, ref _cameraSmoothDampVelocity, _collisionMoveSmoothTime);
            }

            /// Determine how far camera needs to move forward when collision happened.
            float GetAdjustedDistanceWithRayFrom()
            {
                float distance = 150;
                RaycastHit retVal = new RaycastHit();

                for (int i = 0; i < desiredCameraClipPoints.Length; i++)
                {
                    Ray ray = new Ray(_playerSpineTransform.position, desiredCameraClipPoints[i] - _playerSpineTransform.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.Log("hit Object = " + hit.transform.gameObject.name);
                        if (hit.distance < distance)
                        {
                            distance = hit.distance;
                            retVal = hit;
                        }
                    }

                    //Debug.Log("distance1 = " + distance);
                }

                return transform.InverseTransformPoint(retVal.point).z;
            }
        }

        //private void OnDrawGizmos()
        //{
        //    Gizmos.DrawWireSphere(_collisionDetectionPivot, _cancelCollisionSphereRadius);
        //}
        #endregion

        #region Get Dot Product To Enemy.
        public float GetDotProductToEnemy(AIStateManager _aiStates)
        {
            Vector3 _mainCameraForwardDir = _mainCameraTransform.forward;
            _mainCameraForwardDir.y = 0;

            Vector3 dirFromCamToEnemy = (_aiStates.mTransform.position - _mainCameraTransform.position).normalized;
            dirFromCamToEnemy.y = 0;

            return Vector3.Dot(_mainCameraForwardDir, dirFromCamToEnemy);
        }
        #endregion

        #region Switch ICameraRotate Managable.
        public void SwitchToLockonRotateManagable()
        {
            _iCamRotationManagable = lockonRotateManagable;
        }

        public void SwitchToFreemodeRotateManagable()
        {
            _iCamRotationManagable = freemodeRotateManagable;
        }
        #endregion

        #region Switch ICameraCollision Managable.
        public void SwitchToFreeCamCollisionManagable()
        {
            _iCamCollisionManagable = freeCamCollisionManagable;
        }

        public void SwitchToPauseCamCollisionManagable()
        {
            _iCamCollisionManagable = pauseCamCollisionManagable;
        }
        #endregion

        #region World UI Camera.
        public void IncreaseWorldUICameraUsageCount()
        {
            _worldUIElementsCount++;

            if (!_isActivatingWorldUICamera)
            {
                ActivateWorldUICamera();
            }
        }

        public void DecreaseWorldUICameraUsageCount()
        {
            _worldUIElementsCount--;

            if (_worldUIElementsCount == 0)
            {
                DeactivateWorldUICamera();
            }
        }

        void DeactivateWorldUICamera()
        {
            worldUICamera.gameObject.SetActive(false);
            _isActivatingWorldUICamera = false;
        }

        void ActivateWorldUICamera()
        {
            worldUICamera.gameObject.SetActive(true);
            _isActivatingWorldUICamera = true;
        }
        #endregion

        #region Reset To Init Transform.
        public void MoveComponentsToInitTransform()
        {
            _mTransform.localPosition = _camHandler_initTransform._componentLocalPos;
            _mTransform.localEulerAngles = _camHandler_initTransform._componentLocalEulers;

            _pivotTransform.localPosition = _camPivot_initTransform._componentLocalPos;
            _pivotTransform.localEulerAngles = _camPivot_initTransform._componentLocalEulers;

            _mainCameraTransform.localPosition = _mainCam_initTransform._componentLocalPos;
            _mainCameraTransform.localEulerAngles = _mainCam_initTransform._componentLocalEulers;
        }
        #endregion

        public enum CameraRotationTypeEnum
        {
            Freemode,
            Lockon
        }
    }

    public interface ICameraRotateManagable
    {
        void Execute(CameraHandler _camHandler);
    }

    public interface ICameraCollisionManagable
    {
        void Execute();
    }
}

//public void UpdateCameraClipPoints(Vector3 _cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
//{
//    /// clear the contents of intoArray
//    intoArray = new Vector3[5];

//    float z = mainCamera.nearClipPlane;
//    float x = Mathf.Tan(mainCamera.fieldOfView / 3.12f) * z;
//    float y = x / mainCamera.aspect;

//    /// top left
//    intoArray[0] = (atRotation * new Vector3(-x, y, z)) + _cameraPosition;
//    /// top right
//    intoArray[1] = (atRotation * new Vector3(x, y, z)) + _cameraPosition;
//    /// bottom left
//    intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + _cameraPosition;
//    /// bottom right
//    intoArray[3] = (atRotation * new Vector3(x, -y, z)) + _cameraPosition;
//    /// Camera's position
//    intoArray[4] = _cameraPosition;
//}