using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA
{
    public class CharacterCommentHandler : MonoBehaviour
    {
        [Header("Fade Session.")]
        public LeanTweenType commentFadeEaseType;
        public float commentFadeSpeed;

        [Header("Display Session.")]
        public TMP_Text _tmp_comment;
        public float commentDisplayRate;
        [ReadOnlyInspector] public bool _isShowingComment;
        [ReadOnlyInspector] public bool _isPauseAcceptingComment;
        [ReadOnlyInspector] public float commentDisplayTimer;
        [ReadOnlyInspector] public float _delta;
        
        [Header("Commentary Types.")]
        [ReadOnlyInspector] public PickupCommentaryTypeEnum _currentPickupCommentType;

        [Header("Camp Comment Pools.")]
        public CharacterCommentTextPool _restInCampCommentPool;
        public CharacterCommentTextPool _unsecuredCamp_G1_CommentPool;
        public CharacterCommentTextPool _unsecuredCamp_G4_CommentPool;
        public CharacterCommentTextPool _securedCampCommentPool;

        [Header("Chest Comment Pools.")]
        public CharacterCommentTextPool _unOpenedChestCommentPool;

        [Header("Door Comment Pools.")]
        public CharacterCommentTextPool _lockedDoorCommentPool;
        public CharacterCommentTextPool _openedDoorCommentPool;

        [Header("Pickups Comment Pools.")]
        public CharacterCommentTextPool _pickupConsumablesCommentPool;
        public CharacterCommentTextPool _pickupWeaponsCommentPool;
        public CharacterCommentTextPool _pickupRingsCommentPool;
        public CharacterCommentTextPool _pickupArmorsCommentPool;
        public CharacterCommentTextPool _pickupPowerupsCommentPool;

        [Header("Time Rate Commentary Moments.")]
        public CommentaryCooldownHandler _pickupItemCommentaryMoment;
        public CommentaryCooldownHandler _restInCampCommentaryMoment;
        public InteruptHandlable_CommentaryCooldownHandler _campAroundCommentaryMoment;
        public InteruptHandlable_CommentaryCooldownHandler _chestAroundCommentaryMoment;

        [Header("Refs.")]
        public Canvas commentCanvas;
        public CanvasGroup commentGroup;
        [ReadOnlyInspector] public StateManager _states;
        [ReadOnlyInspector] public CameraHandler _camHandler;
        [ReadOnlyInspector, SerializeField] Transform _worldUICameraTransform;

        #region Private.
        int commentFadeTweenId;
        List<CommentaryCooldownHandler> coolDownHandlers = new List<CommentaryCooldownHandler>();
        CommentaryCooldownHandler _previousCoolDownHandler;
        int _coolDownablesCount;
        Vector3 _originalLocalPos;
        Vector3 _restInCampLocalPos;
        #endregion

        #region Late Tick.
        public void LateTick()
        {
            UpdateDelta();

            if (_isShowingComment)
            {
                UpdateCommentFacing();
                MonitorCommentTimer();
            }

            MonitorCoolDownZones();
        }

        void UpdateDelta()
        {
            _delta = _states._delta;
        }

        void UpdateCommentFacing()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _worldUICameraTransform.position);
        }

        void MonitorCommentTimer()
        {
            commentDisplayTimer += _states._delta;
            if (commentDisplayTimer > commentDisplayRate)
            {
                commentDisplayTimer = 0;
                UnRegisterComment();
            }
        }

        void MonitorCoolDownZones()
        {
            if (_coolDownablesCount > 0)
            {
                if (_coolDownablesCount == 1)
                {
                    coolDownHandlers[0].MonitorCommentaryCoolDown();
                }
                else
                {
                    for (int i = 0; i < _coolDownablesCount; i++)
                    {
                        coolDownHandlers[i].MonitorCommentaryCoolDown();
                    }
                }
            }
        }
        #endregion

        #region Add / Remove CoolDownables.
        void Add_PickupItem_ToCoolDownables()
        {
            _pickupItemCommentaryMoment._isInCoolDown = true;

            _coolDownablesCount++;
            coolDownHandlers.Add(_pickupItemCommentaryMoment);
        }

        public void Add_RestInCamp_ToCoolDownables()
        {
            _restInCampCommentaryMoment._isInCoolDown = true;

            _coolDownablesCount++;
            coolDownHandlers.Add(_restInCampCommentaryMoment);
        }

        public void Add_CampAround_ToCoolDownables()
        {
            _campAroundCommentaryMoment._isInCoolDown = true;

            _coolDownablesCount++;
            coolDownHandlers.Add(_campAroundCommentaryMoment);
        }

        public void Add_ChestAround_ToCoolDownables()
        {
            _chestAroundCommentaryMoment._isInCoolDown = true;

            _coolDownablesCount++;
            coolDownHandlers.Add(_chestAroundCommentaryMoment);
        }

        public void RemoveFromCoolDownables(CommentaryCooldownHandler _coolDownableToRemove)
        {
            _coolDownablesCount--;
            coolDownHandlers.Remove(_coolDownableToRemove);
        }
        #endregion

        #region Register Comment.
        /// Pickups.
        public void RegisterPickupItemCommentMoment()
        {
            if (_isPauseAcceptingComment)
                return;

            SetIsShowingCommentToTrue();
            SetCommentMomentText();
            Add_PickupItem_ToCoolDownables();
            SetPreviousCooldownHandler();

            void SetCommentMomentText()
            {
                switch (_currentPickupCommentType)
                {
                    case PickupCommentaryTypeEnum.Consumables:
                        _tmp_comment.text = _pickupConsumablesCommentPool.GetRandomCommentFromPool();
                        break;
                    case PickupCommentaryTypeEnum.Weapons:
                        _tmp_comment.text = _pickupWeaponsCommentPool.GetRandomCommentFromPool();
                        break;
                    case PickupCommentaryTypeEnum.Rings:
                        _tmp_comment.text = _pickupRingsCommentPool.GetRandomCommentFromPool();
                        break;
                    case PickupCommentaryTypeEnum.Armors:
                        _tmp_comment.text = _pickupArmorsCommentPool.GetRandomCommentFromPool();
                        break;
                    case PickupCommentaryTypeEnum.Powerups:
                        _tmp_comment.text = _pickupPowerupsCommentPool.GetRandomCommentFromPool();
                        break;
                }
            }

            void SetPreviousCooldownHandler()
            {
                _previousCoolDownHandler = _pickupItemCommentaryMoment;
            }
        }
        
        /// Doors.
        public void RegisterLockedDoorCommentMoment()
        {
            SetIsShowingCommentToTrue();
            SetCommentMomentText();

            void SetCommentMomentText()
            {
                _tmp_comment.text = _lockedDoorCommentPool.GetRandomCommentFromPool();
            }
        }

        public void RegisterOpenedDoorCommentMoment()
        {
            if (_isPauseAcceptingComment)
                return;

            SetIsShowingCommentToTrue();
            SetCommentMomentText();

            void SetCommentMomentText()
            {
                _tmp_comment.text = _openedDoorCommentPool.GetRandomCommentFromPool();
            }
        }

        /// Camps.
        public void RegisterRestInCampCommentMoment()
        {
            SetIsShowingCommentToTrue();
            MoveToCommentToRestPosition();
            SetCommentMomentText();
            Add_RestInCamp_ToCoolDownables();
            SetPreviousCooldownHandler();

            void MoveToCommentToRestPosition()
            {
                _tmp_comment.transform.localPosition = _restInCampLocalPos;
            }

            void SetCommentMomentText()
            {
                _tmp_comment.text = _restInCampCommentPool.GetRandomCommentFromPool();
            }

            void SetPreviousCooldownHandler()
            {
                _previousCoolDownHandler = _restInCampCommentaryMoment;
            }
        }

        public void RegisterUnSecuredCampCommentMoment(UnsecuredCampCommentaryTypeEnum _unsecuredCampCommentType)
        {
            SetIsShowingCommentToTrue();
            SetCommentMomentText();
            Add_CampAround_ToCoolDownables();
            SetPreviousCooldownHandler();

            void SetCommentMomentText()
            {
                switch (_unsecuredCampCommentType)
                {
                    case UnsecuredCampCommentaryTypeEnum.Group1:
                        _tmp_comment.text = _unsecuredCamp_G1_CommentPool.GetRandomCommentFromPool();
                        break;
                    case UnsecuredCampCommentaryTypeEnum.Group4:
                        _tmp_comment.text = _unsecuredCamp_G4_CommentPool.GetRandomCommentFromPool();
                        break;
                }
            }

            void SetPreviousCooldownHandler()
            {
                _previousCoolDownHandler = _campAroundCommentaryMoment;
            }
        }

        public void RegisterSecuredCampCommentMoment()
        {
            SetIsShowingCommentToTrue();
            SetCommentMomentText();
            Add_CampAround_ToCoolDownables();
            SetPreviousCooldownHandler();

            void SetCommentMomentText()
            {
                _tmp_comment.text = _securedCampCommentPool.GetRandomCommentFromPool();
            }

            void SetPreviousCooldownHandler()
            {
                _previousCoolDownHandler = _campAroundCommentaryMoment;
            }
        }

        /// Chests.
        public void RegisterUnOpenedChestCommentMoment()
        {
            SetIsShowingCommentToTrue();
            SetCommentMomentText();
            Add_ChestAround_ToCoolDownables();
            SetPreviousCooldownHandler();

            void SetCommentMomentText()
            {
                _tmp_comment.text = _unOpenedChestCommentPool.GetRandomCommentFromPool();
            }

            void SetPreviousCooldownHandler()
            {
                _previousCoolDownHandler = _chestAroundCommentaryMoment;
            }
        }

        void UnRegisterComment()
        {
            _isShowingComment = false;
            HideComment();
        }
        #endregion

        #region Show / Hide Comment.
        void SetIsShowingCommentToTrue()
        {
            if (!_isShowingComment)
            {
                _isShowingComment = true;
                ShowComment();

                _camHandler.IncreaseWorldUICameraUsageCount();
            }
            else
            {
                commentDisplayTimer = 0;
            }
        }

        void ShowComment()
        {
            if (LeanTween.isTweening(commentFadeTweenId))
                LeanTween.cancel(commentFadeTweenId);

            EnableComment();
            commentFadeTweenId = LeanTween.alphaCanvas(commentGroup, 1, commentFadeSpeed).setEase(commentFadeEaseType).id;
        }

        void HideComment()
        {
            if (LeanTween.isTweening(commentFadeTweenId))
                LeanTween.cancel(commentFadeTweenId);
            
            commentFadeTweenId = LeanTween.alphaCanvas(commentGroup, 0, commentFadeSpeed).setEase(commentFadeEaseType).setOnComplete(DisableComment).id;
        }

        void EnableComment()
        {
            commentCanvas.enabled = true;
        }

        void DisableComment()
        {
            commentCanvas.enabled = false;

            _camHandler.DecreaseWorldUICameraUsageCount();
        }

        public void ResetCommentTextToOriginalPosition()
        {
            _tmp_comment.transform.localPosition = _originalLocalPos;
        }
        #endregion

        #region Pause Accepting Comment.
        public void PauseAcceptingComment()
        {
            if (!_isPauseAcceptingComment)
            {
                SetIsPauseAcceptingCommentToTrue();
                CheckUnRegisterCommentNeeded();
                SetIngoreTriggerZoneLayer();
            }

            void CheckUnRegisterCommentNeeded()
            {
                if (_isShowingComment)
                {
                    UnRegisterComment();
                }
            }
        }

        void SetIsPauseAcceptingCommentToTrue()
        {
            _isPauseAcceptingComment = true;
        }

        void SetIngoreTriggerZoneLayer()
        {
            Physics.IgnoreLayerCollision(LayerManager.singleton.commentaryZoneLayer, _states.gameObject.layer);
        }

        public void ResumeAcceptingComment()
        {
            SetIsPauseAccptingCommentToFalse();
            RecoverTriggerZoneLayer();

            void SetIsPauseAccptingCommentToFalse()
            {
                _isPauseAcceptingComment = false;
            }

            void RecoverTriggerZoneLayer()
            {
                Physics.IgnoreLayerCollision(LayerManager.singleton.commentaryZoneLayer, _states.gameObject.layer, false);
            }
        }
        #endregion

        #region Interrupt Comment.
        public void PauseAcceptingComment_AsInterrupt()
        {
            if (_previousCoolDownHandler != null && !_isPauseAcceptingComment)
            {
                SetIsPauseAcceptingCommentToTrue();
                CheckInInterruptedComment();
                SetIngoreTriggerZoneLayer();
            }

            void CheckInInterruptedComment()
            {
                if (_isShowingComment)
                {
                    UnRegisterComment();
                    _previousCoolDownHandler.HandleInterrupt();
                }
            }
        }
        #endregion

        #region Checkpoint Agent Interaction.
        public void OnCheckpointAgentInteraction()
        {
            if (!_restInCampCommentaryMoment._isInCoolDown)
            {
                LeanTween.value(0, 1, 1).setOnComplete(RegisterRestInCampCommentMoment);
            }
        }
        #endregion

        #region Setup.
        public void SetupByStates(StateManager _states)
        {
            this._states = _states;

            SetupCommentCanvas();
            SetupUseLocalPositions();
            SetupTimeRateCommentMoments();
            ActivateCommentaryZones();
        }

        void SetupCommentCanvas()
        {
            commentCanvas = GetComponent<Canvas>();

            commentCanvas.enabled = false;
            commentGroup.alpha = 0;
        }

        void SetupUseLocalPositions()
        {
            _originalLocalPos = _tmp_comment.transform.localPosition;
            _restInCampLocalPos = new Vector3(-0.51f, -1.13f, -0.47f);
        }

        void SetupTimeRateCommentMoments()
        {
            _pickupItemCommentaryMoment._commentHandler = this;
            _restInCampCommentaryMoment._commentHandler = this;
            _campAroundCommentaryMoment._commentHandler = this;

            /// Chest will start from cooldown state.
            _chestAroundCommentaryMoment._commentHandler = this;
            Add_ChestAround_ToCoolDownables();
        }

        void ActivateCommentaryZones()
        {
            CommentaryZonesActivator._singleton.ActivateCommentaryZones(this);
        }

        public void SetupByCamHandler(CameraHandler _camHandler)
        {
            this._camHandler = _camHandler;
            _worldUICameraTransform = _camHandler.worldUICamera.transform;
            commentCanvas.worldCamera = _camHandler.worldUICamera;
        }
        #endregion
    }
    
    public enum PickupCommentaryTypeEnum
    {
        Consumables,
        Weapons,
        Rings,
        Armors,
        Powerups
    }

    public enum UnsecuredCampCommentaryTypeEnum
    {
        Group1,
        Group4
    }
}