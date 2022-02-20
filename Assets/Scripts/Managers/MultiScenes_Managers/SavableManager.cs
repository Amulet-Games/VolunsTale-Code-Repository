using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEditor;

namespace SA
{
    public class SavableManager : MonoBehaviour
    {
        #region Saved Files List.
        [Header("Save Files List")]
        public List<SubSaveFile> _subSavefilesList = new List<SubSaveFile>();
        public Dictionary<int, Sprite> savedAvatarIconDict = new Dictionary<int, Sprite>();
        #endregion

        #region Previous Files.
        [Header("Previous Files.")]
        public MainSaveFile _prev_MainSavedFile;
        public SubSaveFile _prev_SubSaveFile;
        #endregion

        #region Specific Files.
        [Header("specific Files.")]
        public MainSaveFile _spec_MainSavedFile;
        public SubSaveFile _spec_SubSaveFile;
        #endregion

        #region Savable Managers.
        [Header("Savable Managers.")]
        [ReadOnlyInspector] public StateManager _states;
        [ReadOnlyInspector] public AIBossManagable _aIBossManagable;
        public List<AIStateManager> _aIStates = new List<AIStateManager>();
        public List<PlayerInteractable> _playerInteractables = new List<PlayerInteractable>();
        #endregion

        #region Init Player Gears Action.
        public StateActions createStartupGearAction;
        public StateActions loadPlayerGearsFromSaveAction;
        #endregion

        #region Status.
        [Header("Status.")]
        [ReadOnlyInspector] public bool isContinueGame;
        [ReadOnlyInspector] public bool isLoadGame;
        [ReadOnlyInspector] public bool isNewGame;
        #endregion

        #region HideInInspector.
        [ReadOnlyInspector] public int _cur_savefile_id;
        #endregion

        public static SavableManager singleton;
        
        #region Create Savables.
        void Create_PlayerSavable(MainSaveFile _savefile)
        {
            _savefile.savedPlayerState = _states.SaveStateToSave_Player();
            //_states.CreateAvatarIconContainer(_savefile);
            Debug.Log("Savable PlayerState Created");
        }

        void Create_StatusSavable(MainSaveFile _savefile)
        {
            _savefile.savedStatusState = _states.statsHandler.SaveStateToSave();
            Debug.Log("Savable LoadGame State Created");
        }

        void Create_InventorySavable(MainSaveFile _savefile)
        {
            SavableInventory _inventory = _states._savableInventory;
            /// Weapons.
            _savefile.savedWeaponStates = _inventory.SaveWeaponStateToSave();
            /// Armor.
            _savefile.savedHeadStates = _inventory.SaveHeadStateToSave();
            _savefile.savedChestStates = _inventory.SaveChestStateToSave();
            _savefile.savedHandStates = _inventory.SaveHandStateToSave();
            _savefile.savedLegStates = _inventory.SaveLegStateToSave();
            ///Charm.
            _savefile.savedCharmStates = _inventory.SaveCharmStateToSave();
            ///Powerup.
            _savefile.savedPowerupStates = _inventory.SavePowerupStateToSave();
            ///Ring.
            _savefile.savedRingStates = _inventory.SaveRingStateToSave();
            ///Consumable.
            _inventory.SaveConsumableStateToSave(_savefile);
        }

        void Create_EnemySavable(MainSaveFile _savefile)
        {
            _savefile.savedEnemyStates.Clear();

            int savableEnemyCount = _aIStates.Count;
            for (int i = 0; i < savableEnemyCount; i++)
            {
                _savefile.savedEnemyStates.Add(_aIStates[i].SaveEnemyStateToSave());
            }

            Debug.Log("Savable EnemyState Created");
        }

        void Create_InteractableSavable(MainSaveFile _savefile)
        {
            _savefile.savedInteractionStates.Clear();

            int savableInteractionCount = _playerInteractables.Count;
            for (int i = 0; i < savableInteractionCount; i++)
            {
                _savefile.savedInteractionStates.Add(_playerInteractables[i].SaveInteractionStateToSave());
            }

            Debug.Log("Savable InteractionState Created");
        }

        void Create_BossSavable(MainSaveFile _savefile)
        {
            _savefile.savedBossState = _aIBossManagable.SaveBossStateToSave();
        }

        void Create_ProfileSavable(SubSaveFile _savefile)
        {
            _savefile._savedProfileState = _states.SaveStateToSave_Profile();
        }
        #endregion

        #region Overwrite Savables.
        void Overwrite_PlayerSavable()
        {
            _states.OverwriteStateToSave_Player();
        }

        void Overwrite_StatusSavable()
        {
            _states.statsHandler.OverwriteStateToSave();
        }

        void Overwrite_ProfileSavable()
        {
            _states.OverwriteStateToSave_Profile();
        }
        #endregion

        #region Serialize New File.
        public void Serialize_NewFile()
        {
            _cur_savefile_id = _subSavefilesList.Count;

            Serialize_NewFile_Main();

            Serialize_NewFile_Sub();
        }

        void Serialize_NewFile_Main()
        {
            MainSaveFile _newSaveFile = new MainSaveFile();

            Create_PlayerSavable(_newSaveFile);
            Create_StatusSavable(_newSaveFile);
            Create_InventorySavable(_newSaveFile);
            Create_EnemySavable(_newSaveFile);
            Create_InteractableSavable(_newSaveFile);

            Serialization.Main_SaveToFile_NewGame(_newSaveFile);
            _states._current_main_saveFile = _newSaveFile;
        }

        void Serialize_NewFile_Sub()
        {
            SubSaveFile _newSubSaveFile = new SubSaveFile();
            _newSubSaveFile.saveFileId = _cur_savefile_id;
            _newSubSaveFile.isUsed = true;

            Create_ProfileSavable(_newSubSaveFile);

            Serialization.Sub_SaveToFile_NewGame(_newSubSaveFile);
            _states._current_sub_saveFile = _newSubSaveFile;
        }
        #endregion

        #region Serialize Exist File.
        public void Serialize_ExistFile()
        {
            _cur_savefile_id = _states._current_sub_saveFile.saveFileId;

            Serialize_ExistFile_Main();

            Serialize_ExistFile_Sub();

            Serialize_ExistFile_Avatar();
        }
        
        void Serialize_ExistFile_Main()
        {
            MainSaveFile _currentSaveFile = _states._current_main_saveFile;

            Overwrite_PlayerSavable();
            Overwrite_StatusSavable();
            Create_InventorySavable(_currentSaveFile);
            Create_EnemySavable(_currentSaveFile);
            Create_InteractableSavable(_currentSaveFile);
            Create_BossSavable(_currentSaveFile);

            Serialization.Main_SaveToFile_ExistingFile(_currentSaveFile);
        }

        void Serialize_ExistFile_Sub()
        {
            SubSaveFile _cur_SubSaveFile = _states._current_sub_saveFile;

            Overwrite_ProfileSavable();
            
            Serialization.Sub_SaveToFile_ExistingFile(_cur_SubSaveFile);
        }

        void Serialize_ExistFile_Avatar()
        {
            _states.CreateAvatarImageToSave();
        }

        void Serialize_PrevFile_Sub()
        {
            Serialization.Sub_SaveToFile_ExistingFile(_prev_SubSaveFile);
        }

        void Serialize_SpecFile_Sub()
        {
            Serialization.Sub_SaveToFile_ExistingFile(_spec_SubSaveFile);
        }
        #endregion

        #region Delete Exist File.
        public void Delete_ExistFile(int deletingFileId)
        {
            Delete_ExistFile_Main();

            Delete_ExistFile_Sub();

            Delete_ExistFile_Avatar();

            void Delete_ExistFile_Main()
            {
                /// Delete File From Directory
                Serialization.Main_DeleteFile(deletingFileId);
            }

            void Delete_ExistFile_Sub()
            {
                RemoveFileFromList();

                DeleteFileFromDirectory();

                void RemoveFileFromList()
                {
                    SubSaveFile fileToRemove = _subSavefilesList[deletingFileId];

                    _subSavefilesList.Remove(fileToRemove);

                    if (fileToRemove.isUsed)
                    {
                        _prev_SubSaveFile.isUsed = false;
                    }
                }

                void DeleteFileFromDirectory()
                {
                    Serialization.Sub_DeleteFile(deletingFileId);
                }
            }

            void Delete_ExistFile_Avatar()
            {
                /// Delete File From Directory
                Serialization.Avatar_DeleteFile(deletingFileId);
            }
        }
        #endregion

        #region Change File Name IDs.
        public void Change_FileNameIds_SingleLeft()
        {
            SubSaveFile _lastSubSaveFile = _subSavefilesList[0];
            int _cur_renamefile_Id = _lastSubSaveFile.saveFileId;

            Change_FileNameIds_Main_SingleLeft();

            Change_FileNameIds_Sub_SingleLeft();

            Change_FileNameIds_Avatar_SingleLeft();

            void Change_FileNameIds_Main_SingleLeft()
            {
                /// Change File Name In Directory
                Serialization.Main_RenameFile(_cur_renamefile_Id, 0);
            }

            void Change_FileNameIds_Sub_SingleLeft()
            {
                ChangeFileNameInDirectory();

                ChangeFileSaveID();

                SerializeChanges();

                void ChangeFileNameInDirectory()
                {
                    Serialization.Sub_RenameFile(_cur_renamefile_Id, 0);
                }

                void ChangeFileSaveID()
                {
                    _lastSubSaveFile.saveFileId = 0;
                }

                void SerializeChanges()
                {
                    Serialization.Sub_SaveToFile_ExistingFile(_lastSubSaveFile);
                }
            }

            void Change_FileNameIds_Avatar_SingleLeft()
            {
                /// Change File Name In Directory
                Serialization.Avatar_RenameFile(_cur_renamefile_Id, 0);
            }
        }

        public void Change_FileNameIds_MultipleLeft(int oldFileId, int newFildId)
        {
            Change_FileNameIds_Main_MultipleLeft();

            Change_FileNameIds_Sub_MultipleLeft();

            Change_FileNameIds_Avatar_MultipleLeft();

            void Change_FileNameIds_Main_MultipleLeft()
            {
                /// Change File Name In Directory
                Serialization.Main_RenameFile(oldFileId, newFildId);
            }

            void Change_FileNameIds_Sub_MultipleLeft()
            {
                ChangeFileNameInDirectory();

                ChangeFileSaveID();

                SerializeChanges();

                void ChangeFileNameInDirectory()
                {
                    Serialization.Sub_RenameFile(oldFileId, newFildId);
                }

                void ChangeFileSaveID()
                {
                    _subSavefilesList[newFildId].saveFileId = newFildId;
                }

                void SerializeChanges()
                {
                    Serialization.Sub_SaveToFile_ExistingFile(_subSavefilesList[newFildId]);
                }
            }

            void Change_FileNameIds_Avatar_MultipleLeft()
            {
                /// Change File Name In Directory
                Serialization.Avatar_RenameFile(oldFileId, newFildId);
            }
        }
        #endregion

        #region On Continue Game.
        public void OnContinueGame()
        {
            isContinueGame = true;
            _prev_MainSavedFile = Serialization.Main_LoadTargetFile(_prev_SubSaveFile.saveFileId);
        }
        #endregion

        #region On Load Game.
        public void OnLoadGame(int profileIndex)
        {
            isLoadGame = true;
            _spec_SubSaveFile = _subSavefilesList[profileIndex];
            _spec_MainSavedFile = Serialization.Main_LoadTargetFile(profileIndex);
        }
        #endregion

        #region On Quit Level.
        public void OnQuitLevel_ToTitleScreen()
        {
            OnQuitLevel_ClearUpRefs();
            CollectSavedFiles();
        }

        void OnQuitLevel_ClearUpRefs()
        {
            _aIStates.Clear();
            _playerInteractables.Clear();

            isContinueGame = false;
            isLoadGame = false;
            isNewGame = false;
        }
        #endregion

        #region Collect Saved Files.
        void CollectSavedFiles()
        {
            GetSubSavedFiles();
            SetAvatarDictionary();
            SetPreviousSavedFile();
        }

        void GetSubSavedFiles()
        {
            _subSavefilesList = Serialization.Sub_LoadAllFiles();
        }

        void SetAvatarDictionary()
        {
            Serialization.SetupAvatarDictionary();
        }

        void SetPreviousSavedFile()
        {
            int subFilesCount = _subSavefilesList.Count;
            for (int i = 0; i < subFilesCount; i++)
            {
                if (_subSavefilesList[i].isUsed)
                {
                    _prev_SubSaveFile = _subSavefilesList[i];
                }
            }
        }
        #endregion

        #region Set Is Used Status.
        public void If_PrevSaveNotCurrent_SwitchIsUsedStatus()
        {
            if (_prev_SubSaveFile.saveFileId != _spec_SubSaveFile.saveFileId)
            {
                Set_PrevSave_IsUsedToFalse();
                Set_Spec_IsUsedToTrue();
            }
        }

        public void Set_PrevSave_IsUsedToFalse()
        {
            // If the Previous Sub Savefile is Deleted, its "isUsed" Status should be 'false' already
            // So here if it's still true, meaning that the Sub Savefile Existed and we need to set it to false.
            if (_prev_SubSaveFile.isUsed)
            {
                _prev_SubSaveFile.isUsed = false;
                Serialize_PrevFile_Sub();
            }
        }

        void Set_Spec_IsUsedToTrue()
        {
            _spec_SubSaveFile.isUsed = true;

            Serialize_SpecFile_Sub();
        }
        #endregion

        #region Avatar Sprites Dict.
        public Sprite GetAvatarSpriteFromDict(int targetId)
        {
            Sprite retVal = null;
            savedAvatarIconDict.TryGetValue(targetId, out retVal);
            return retVal;
        }
        #endregion

        #region Awake.
        private void Awake()
        {
            if (singleton != null)
            {
                Destroy(gameObject);
            }
            else
            {
                singleton = this;
            }

            InitSerializationClass();
            CollectSavedFiles();
        }

        void InitSerializationClass()
        {
            Serialization.Init();
        }
        #endregion
    }
}