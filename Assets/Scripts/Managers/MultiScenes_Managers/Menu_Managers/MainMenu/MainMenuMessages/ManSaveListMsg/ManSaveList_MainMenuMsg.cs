using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class ManSaveList_MainMenuMsg : MonoBehaviour
    {
        [Header("Sub Message (Drops).")]
        public CanvasGroup subMessageGroup;

        [Header("List (Drops).")]
        public RectTransform listBgRect;
        public Canvas listBgCanvas;

        [Header("Profiles (Drops).")]
        public LoadGameProfile[] default_LgProfiles;

        [Header("Hint (Drops.)")]
        public GameObject _hintObj;

        [Header("Sub Message Group Tween.")]
        public LeanTweenType subMessageEaseType = LeanTweenType.easeOutCirc;
        public float subMessageFadeSpeed = 0.125f;

        [Header("List Bg Tween.")]
        public LeanTweenType listBgEaseType = LeanTweenType.easeOutExpo;
        public float listBgMovesSpeed = 0.4f;
        public int listBgMovesInXPos = -1000;
        public int listBgMovesOutXPos = -4130;

        [Header("Profiles Tween.")]
        public float profileMovesDelayTime = 0.07f;
        public float profileMovesSpeed = 0.15f;
        public int profileMovesInXPos = -1000;
        public int profileMovesOutXPos = -4130;

        [Header("Profile Highlighter Tween.")]
        public LeanTweenType highEaseType = LeanTweenType.easeOutCirc;
        public float highTweenSpeed;

        [Space(10)]
        public Color highMaxValueColor;
        public Color highMidValueColor;
        public Color highNoAlphaColor;

        [Header("Sub Msg Type.")]
        public DualButton_MainMenuMsg confirmDeleteMsg;
        public SingleButton_MainMenuMsg noSaveLeftMsg;
        [ReadOnlyInspector] public ManSaveListSubMsgTypeEnum subMsgType;

        [Header("Status.")]
        [ReadOnlyInspector] public int _profileIndex;

        [Header("Save Limit.")]
        public int savedProfileLimit;
        [ReadOnlyInspector] public int _savedProfileCount;

        [Header("Refs.")]
        [ReadOnlyInspector] public LoadGameProfile currentProfile;
        [ReadOnlyInspector] public MainMenuManager _mainMenuManager;
        [ReadOnlyInspector] public SavableManager _savableManager;

        [Space(10)]
        [ReadOnlyInspector] public List<LoadGameProfile> runtime_LgProfiles;

        #region IDs.
        int _subMsgTweenID;
        int _listBgTweenID;
        int _profileTweenID;
        int _profileHighTweenId;
        #endregion

        #region Tick.
        public void Tick()
        {
            UpdateMsgBySubType();
        }

        void UpdateMsgBySubType()
        {
            switch (subMsgType)
            {
                case ManSaveListSubMsgTypeEnum.Empty:
                    break;
                case ManSaveListSubMsgTypeEnum.SaveList:
                    SaveListTick();
                    break;
                case ManSaveListSubMsgTypeEnum.ConfirmDelete:
                    confirmDeleteMsg.Tick();
                    break;
                case ManSaveListSubMsgTypeEnum.NoSaveLeft:
                    noSaveLeftMsg.Tick();
                    break;
            }
        }

        #region Save List.
        void SaveListTick()
        {
            _mainMenuManager.UpdateInputs_ManSaveList();

            GetProfileByInput();

            LoadCurProfileByInput();

            RemoveCurProfileByInput();
        }

        void GetProfileByInput()
        {
            if (_mainMenuManager.menu_up_input)
            {
                _profileIndex--;
                _profileIndex = _profileIndex < 0 ? _savedProfileCount - 1 : _profileIndex;

                SetCurProfile_ByInput();
            }
            else if (_mainMenuManager.menu_down_input)
            {
                _profileIndex++;
                _profileIndex = _profileIndex == _savedProfileCount ? 0 : _profileIndex;

                SetCurProfile_ByInput();
            }

            void SetCurProfile_ByInput()
            {
                currentProfile.OffProfile();
                currentProfile = runtime_LgProfiles[_profileIndex];
                currentProfile.OnProfile_ByInput();
            }
        }

        void LoadCurProfileByInput()
        {
            if (_mainMenuManager.menu_select_input)
            {
                LoadProfile_UIButton();
            }
        }

        void RemoveCurProfileByInput()
        {
            if (_mainMenuManager.menu_remove_input)
            {
                DeleteProfile_UIButton();
            }
        }
        #endregion
        
        #endregion

        #region Pointer Event.
        public void SetCurProfile_ByPointer(LoadGameProfile _profile)
        {
            currentProfile.OffProfile();
            currentProfile = _profile;
            _profileIndex = _profile.profileId;
        }
        #endregion

        #region Load Profile.
        public void LoadProfile_UIButton()
        {
            HideHint();

            LoadProfile_FadeOutSubMessageGroup();

            SetSubMsgType_ToEmpty();

            _savableManager.OnLoadGame(_profileIndex);

            _mainMenuManager.FadeOutMessageGroup_StartGame();
        }

        void LoadProfile_FadeOutSubMessageGroup()
        {
            Cancel_SubMessageTween();
            _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 0, subMessageFadeSpeed).setEase(subMessageEaseType).id;
        }
        #endregion

        #region Delete Profile.
        public void DeleteProfile_UIButton()
        {
            SetSubMsgType_ToEmpty();
            FadeOutTo_ConfirmDelete();
        }

        public void Confirm_RemoveProfile()
        {
            SetSubMsgType_ToEmpty();
            FadeBackFrom_ConfirmDelete_Confirm();
        }

        public void Quit_RemoveProfile()
        {
            SetSubMsgType_ToEmpty();
            FadeBackFrom_ConfirmDelete_Quit();
        }

        void DeleteCurrentProfile()
        {
            LoadGameProfile _deletingProfile = currentProfile;
            int deletingProfileId = _deletingProfile.profileId;
            int newProfileCount = _savedProfileCount - 1;

            if (newProfileCount > 0)
            {
                if (newProfileCount > 1)
                {
                    /// if there's still more than 1 profiles left..
                    SetNewCurrentProfile_MultipleLeft();

                    RemoveProfileFromList();

                    DeleteFileFromManager();

                    ResetProfileIDs_MultipleLeft();
                }
                else
                {
                    /// if there's only 1 profile left..
                    SetNewCurrentProfile_SingleLeft();

                    RemoveProfileFromList();

                    DeleteFileFromManager();

                    ResetProfileIDs_SingleLeft();
                }

                HideDeletingProfile_Multi_Single_Left();
                OnNewCurrentProfile();
            }
            else
            {
                /// Show there's no more profiles left...
                SetSubMsgType_ToEmpty();

                SetEmptyCurrentProfile();

                RemoveProfileFromList();

                DeleteFileFromManager();

                HideDeletingProfile_ZeroLeft();
            }

            #region Multiple Left.
            void SetNewCurrentProfile_MultipleLeft()
            {
                int tempIndex = currentProfile.profileId;
                tempIndex++;
                tempIndex = (tempIndex == _savedProfileCount) ? 0 : tempIndex;

                if (runtime_LgProfiles[tempIndex] == null)
                {
                    for (int i = 0; i < _savedProfileCount - 1; i++)
                    {
                        tempIndex++;
                        tempIndex = (tempIndex == _savedProfileCount) ? 0 : tempIndex;

                        if (runtime_LgProfiles[tempIndex] != null)
                        {
                            currentProfile = runtime_LgProfiles[tempIndex];
                        }
                    }
                }
                else
                {
                    currentProfile = runtime_LgProfiles[tempIndex];
                }
            }

            void ResetProfileIDs_MultipleLeft()
            {
                int i_deletingId;

                for (int i = 0; i < _savedProfileCount - deletingProfileId; i++)
                {
                    i_deletingId = deletingProfileId + i;

                    _savableManager.Change_FileNameIds_MultipleLeft(i_deletingId + 1, i_deletingId);

                    runtime_LgProfiles[i_deletingId].profileId = i_deletingId;
                }
            }
            #endregion

            #region Single Left.
            void SetNewCurrentProfile_SingleLeft()
            {
                currentProfile = _deletingProfile == runtime_LgProfiles[0] ? runtime_LgProfiles[1] : runtime_LgProfiles[0];
            }

            void ResetProfileIDs_SingleLeft()
            {
                if (runtime_LgProfiles[0].profileId != 0)
                {
                    _savableManager.Change_FileNameIds_SingleLeft();
                    currentProfile.profileId = 0;
                }
            }
            #endregion

            #region Multiple / Single Left.
            void HideDeletingProfile_Multi_Single_Left()
            {
                LeanTween.moveX(_deletingProfile.profileRect, profileMovesOutXPos, profileMovesSpeed).setOnComplete(OnCompleteHidden);

                void OnCompleteHidden()
                {
                    _deletingProfile.OffProfile();
                    _deletingProfile.DisableProfile();
                }
            }

            void OnNewCurrentProfile()
            {
                _profileIndex = currentProfile.profileId;
                currentProfile.OnProfile_ByInput();
            }
            #endregion

            #region Zero Left.
            void SetEmptyCurrentProfile()
            {
                currentProfile = null;
            }

            void HideDeletingProfile_ZeroLeft()
            {
                LeanTween.moveX(_deletingProfile.profileRect, profileMovesOutXPos, profileMovesSpeed).setOnComplete(OnCompleteHidden);

                void OnCompleteHidden()
                {
                    _deletingProfile.OffProfile();
                    _deletingProfile.DisableProfile();

                    FadeOutTo_NoSaveLeft();
                }
            }
            #endregion

            void RemoveProfileFromList()
            {
                runtime_LgProfiles.Remove(_deletingProfile);
                _savedProfileCount = newProfileCount;
            }

            void DeleteFileFromManager()
            {
                _savableManager.Delete_ExistFile(deletingProfileId);
            }
        }
        #endregion

        #region OK No Save Left.
        public void OK_NoSaveLeftQuitList()
        {
            SetSubMsgType_ToEmpty();
            FadeBackFrom_NoSaveLeft();
        }
        #endregion

        #region On Message Open.
        public void OnMessageOpen()
        {
            ShowHint();
            OnMenuOpen_ShowListBg();
            OnMenuOpen_LoadRuntimeLists();
            OnMenuOpen_SetFirstProfile();
            OnMenuOpen_SetSubMsgType();
        }

        void OnMenuOpen_ShowListBg()
        {
            ShowListBg();
        }
        
        void OnMenuOpen_LoadRuntimeLists()
        {
            ShowActive_LgProfiles();
        }

        void OnMenuOpen_SetFirstProfile()
        {
            _profileIndex = 0;
            currentProfile = runtime_LgProfiles[0];
            currentProfile.OnProfile_ByInput();
        }

        void OnMenuOpen_SetSubMsgType()
        {
            SetSubMsgType_ToSaveList();
        }
        #endregion

        #region On Message Close.
        public void OnMessageClose()
        {
            HideHint();
            OnMenuClose_HideProfiles();
            OnMenuClose_OffCurrentSlot();
            OnMenuClose_SetSubMsgType();
        }

        void OnMenuClose_HideProfiles()
        {
            /// Hide profiles first before List Background.
            HideActive_LgProfiles();
        }

        void OnMenuClose_OffCurrentSlot()
        {
            currentProfile.OffProfile();
        }

        void OnMenuClose_SetSubMsgType()
        {
            SetSubMsgType_ToEmpty();
        }
        #endregion

        #region Show / Hide Save List Bg.
        void CancelListBgTween()
        {
            if (LeanTween.isTweening(_listBgTweenID))
                LeanTween.cancel(_listBgTweenID);
        }

        void ShowListBg()
        {
            CancelListBgTween();

            EnableListCanvas();
            _listBgTweenID = LeanTween.moveX(listBgRect, listBgMovesInXPos, listBgMovesSpeed).setEase(listBgEaseType).id;
        }

        void HideListBg()
        {
            CancelListBgTween();
            
            _listBgTweenID = LeanTween.moveX(listBgRect, listBgMovesOutXPos, listBgMovesSpeed).setEase(listBgEaseType).setOnComplete(OnCompleteTween).id;

            void OnCompleteTween()
            {
                DisableListCanvas();
            }
        }

        void EnableListCanvas()
        {
            listBgCanvas.enabled = true;
        }

        void DisableListCanvas()
        {
            listBgCanvas.enabled = false;
        }

        void HideListBgImmediate()
        {
            Vector2 _tempPos = listBgRect.anchoredPosition;
            _tempPos.x = listBgMovesOutXPos;
            listBgRect.anchoredPosition = _tempPos;
        }
        #endregion

        #region Show / Hide Lg Profiles.
        void Cancel_LgProfilesTween()
        {
            if (LeanTween.isTweening(_profileTweenID))
                LeanTween.cancel(_profileTweenID);
        }

        void ShowActive_LgProfiles()
        {
            Cancel_LgProfilesTween();

            float delayAmount = profileMovesDelayTime;
            LoadGameProfile _temp_profile;

            for (int i = 0; i < _savedProfileCount; i++)
            {
                _temp_profile = runtime_LgProfiles[i];

                Show_LgProfile();

                IncrementDelay();
            }

            void Show_LgProfile()
            {
                _temp_profile.EnableProfile();
                LeanTween.moveX(_temp_profile.profileRect, profileMovesInXPos, profileMovesSpeed).setDelay(delayAmount);
            }
            
            void IncrementDelay()
            {
                delayAmount += profileMovesDelayTime;
            }
        }

        void HideActive_LgProfiles()
        {
            Cancel_LgProfilesTween();

            float delayAmount = profileMovesDelayTime;
            
            for (int i = _savedProfileCount - 1; i > -1; i--)
            {
                LeanTween.moveX(runtime_LgProfiles[i].profileRect, profileMovesOutXPos, profileMovesSpeed).setDelay(delayAmount).setOnComplete(runtime_LgProfiles[i].DisableProfile);

                delayAmount += profileMovesDelayTime;

                if (i < 1)
                {
                    LeanTween.value(0, 1, delayAmount).setOnComplete(OnCompleteWait);
                    
                    void OnCompleteWait()
                    {
                        /// Hide List Background if this is the last element
                        HideListBg();
                        _mainMenuManager.OnCompleteHideActive_LG_Profiles();
                    }
                }
            }
        }
        #endregion

        #region Switch To Sub Message Group.

        #region Confirm Delete.
        void FadeOutTo_ConfirmDelete()
        {
            FadeOutSubMessageGroup();

            void FadeOutSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 0, subMessageFadeSpeed).setEase(subMessageEaseType).setOnComplete(OnCompleteFadeOut).id;
            }

            void OnCompleteFadeOut()
            {
                DisableListCanvas();

                confirmDeleteMsg.OnMessageOpen();
                FadeInSubMessageGroup();

                SetSubMsgType_ToConfirmDelete();
            }

            void FadeInSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 1, subMessageFadeSpeed).setEase(subMessageEaseType).id;
            }
        }

        void FadeBackFrom_ConfirmDelete_Quit()
        {
            FadeOutSubMessageGroup();

            void FadeOutSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 0, subMessageFadeSpeed).setEase(subMessageEaseType).setOnComplete(OnCompleteFadeOut).id;
            }

            void OnCompleteFadeOut()
            {
                confirmDeleteMsg.OnSubMessageClose();

                EnableListCanvas();
                FadeInSubMessageGroup();

                SetSubMsgType_ToSaveList();
            }

            void FadeInSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 1, subMessageFadeSpeed).setEase(subMessageEaseType).id;
            }
        }

        void FadeBackFrom_ConfirmDelete_Confirm()
        {
            FadeOutSubMessageGroup();

            void FadeOutSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 0, subMessageFadeSpeed).setEase(subMessageEaseType).setOnComplete(OnCompleteFadeOut).id;
            }

            void OnCompleteFadeOut()
            {
                confirmDeleteMsg.OnSubMessageClose();

                EnableListCanvas();
                FadeInSubMessageGroup();

                SetSubMsgType_ToSaveList();
            }

            void FadeInSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 1, subMessageFadeSpeed).setEase(subMessageEaseType).setOnComplete(DeleteCurrentProfile).id;
            }
        }
        #endregion

        #region No Save Left.
        void FadeOutTo_NoSaveLeft()
        {
            FadeOutSubMessageGroup();

            void FadeOutSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 0, subMessageFadeSpeed).setEase(subMessageEaseType).setOnComplete(OnCompleteFadeOut).id;
            }

            void OnCompleteFadeOut()
            {
                DisableListCanvas();
                HideListBgImmediate();

                noSaveLeftMsg.OnMessageOpen();
                FadeInSubMessageGroup();

                SetSubMsgType_ToNoSaveLeft();
            }
            
            void FadeInSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 1, subMessageFadeSpeed).setEase(subMessageEaseType).id;
            }
        }

        void FadeBackFrom_NoSaveLeft()
        {
            FadeOutSubMessageGroup();

            void FadeOutSubMessageGroup()
            {
                Cancel_SubMessageTween();
                _subMsgTweenID = LeanTween.alphaCanvas(subMessageGroup, 0, subMessageFadeSpeed).setEase(subMessageEaseType).setOnComplete(OnCompleteFadeOut).id;
            }

            void OnCompleteFadeOut()
            {
                noSaveLeftMsg.DisableCanvas();

                _mainMenuManager.Off_NoSaveLeft_ByOK();
            }
        }
        #endregion
        
        void Cancel_SubMessageTween()
        {
            if (LeanTween.isTweening(_subMsgTweenID))
                LeanTween.cancel(_subMsgTweenID);
        }
        #endregion

        #region Show / Hide Hint.
        void ShowHint()
        {
            _hintObj.SetActive(true);
        }

        void HideHint()
        {
            _hintObj.SetActive(false);
        }
        #endregion

        #region Ping Pong Tween Profile Highlighter.
        public void TweenHighlighter_FullAlpha()
        {
            _profileHighTweenId = LeanTween.value(0, 1, highTweenSpeed).setEase(highEaseType).setOnUpdate
                (
                    (value) =>
                    {
                        currentProfile.highlighterImage.color = Color.Lerp(currentProfile.highlighterImage.color, highMaxValueColor, value);
                    }
                )
                .setOnComplete(TweenHighlighter_MinAlpha).id;
        }

        void TweenHighlighter_MinAlpha()
        {
            _profileHighTweenId = LeanTween.value(0, 1, highTweenSpeed).setEase(highEaseType).setOnUpdate
                (
                    (value) =>
                    {
                        currentProfile.highlighterImage.color = Color.Lerp(currentProfile.highlighterImage.color, highMidValueColor, value);
                    }
                )
                .id;
        }

        public void CancelHighlighterTween()
        {
            if (LeanTween.isTweening(_profileHighTweenId))
                LeanTween.cancel(_profileHighTweenId);
        }
        #endregion

        #region Set Sub Message Type.
        void SetSubMsgType_ToEmpty()
        {
            subMsgType = ManSaveListSubMsgTypeEnum.Empty;
        }

        void SetSubMsgType_ToSaveList()
        {
            subMsgType = ManSaveListSubMsgTypeEnum.SaveList;
        }

        void SetSubMsgType_ToConfirmDelete()
        {
            subMsgType = ManSaveListSubMsgTypeEnum.ConfirmDelete;
        }

        void SetSubMsgType_ToNoSaveLeft()
        {
            subMsgType = ManSaveListSubMsgTypeEnum.NoSaveLeft;
        }
        #endregion

        #region New Game Check.
        public bool CanStartNewGame()
        {
            return _savedProfileCount < savedProfileLimit;
        }
        #endregion

        #region Setup.
        public void Setup(MainMenuManager mainMenuManager)
        {
            _mainMenuManager = mainMenuManager;

            SetupRefs();

            SetupClearRuntimeList();

            GetSubSavedFiles();

            SetupSubMessages();
        }

        void SetupRefs()
        {
            _savableManager = _mainMenuManager._savableManager;
        }

        void SetupClearRuntimeList()
        {
            runtime_LgProfiles.Clear();
        }

        void GetSubSavedFiles()
        {
            List<SubSaveFile> _savedSaveFile = _savableManager._subSavefilesList;
            _savedProfileCount = _savedSaveFile.Count;
            for (int i = 0; i < _savedProfileCount; i++)
            {
                /// SET UP.
                default_LgProfiles[i].Setup(this);

                /// TEXT
                default_LgProfiles[i].profile_p_Name.text = _savedSaveFile[i]._savedProfileState.savableProfName;
                default_LgProfiles[i].profile_p_Date.text = _savedSaveFile[i]._savedProfileState.savableProfDate;
                default_LgProfiles[i].profile_p_Lv.text = _savedSaveFile[i]._savedProfileState.savableProfLevel.ToString();
                default_LgProfiles[i].profile_p_Voluns.text = _savedSaveFile[i]._savedProfileState.savableProfVolun.ToString();

                /// SPRITE
                Sprite savedSubSaveSprite = _savableManager.GetAvatarSpriteFromDict(_savedSaveFile[i].saveFileId);
                if (savedSubSaveSprite != null)
                {
                    default_LgProfiles[i].profile_p_Avatar.enabled = true;
                    default_LgProfiles[i].profile_p_Avatar.sprite = savedSubSaveSprite;
                }
                else
                {
                    default_LgProfiles[i].profile_p_Avatar.enabled = false;
                    default_LgProfiles[i].profile_p_Avatar.sprite = null;
                }

                default_LgProfiles[i].profileId = _savedSaveFile[i].saveFileId;

                /// ADD TO RUNTIME LIST
                runtime_LgProfiles.Add(default_LgProfiles[i]);
            }
        }

        void SetupSubMessages()
        {
            confirmDeleteMsg.Setup(_mainMenuManager);
            noSaveLeftMsg.Setup(_mainMenuManager);
        }
        #endregion
    }

    public enum ManSaveListSubMsgTypeEnum
    {
        Empty,
        SaveList,
        ConfirmDelete,
        NoSaveLeft
    }
}