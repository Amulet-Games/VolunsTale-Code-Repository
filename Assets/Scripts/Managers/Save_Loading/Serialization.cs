using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SA
{
    public static class Serialization
    {
        static StringBuilder saveLocation_strBuilder;

        /// MAIN.

        #region SAVING MAIN
        public static void Main_SaveToFile_NewGame(MainSaveFile new_MainSaveFile)
        {
            ClearLocationBuilder();

            SetFileLocation();

            SerializeSaveFile();

            void SetFileLocation()
            {
                Append_Main_SaveDirectory_Create();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(SavableManager.singleton._cur_savefile_id);
                saveLocation_strBuilder.Append(".data");
            }
            
            void SerializeSaveFile()
            {
                /// Create a file to save to
                Stream stream = new FileStream(saveLocation_strBuilder.ToString(), FileMode.Create, FileAccess.Write, FileShare.None);

                try
                {
                    /// Binary Formmater -- allows us to write data to a file.
                    IFormatter formatter = new BinaryFormatter();

                    /// Serialization method to WRITE to the file
                    formatter.Serialize(stream, new_MainSaveFile);

                }
                catch (SerializationException exception)
                {
                    Debug.LogError("There was an issue serializing this data (New Main File): " + exception.Message);
                }
                finally
                {
                    /// Close the filestream to prevent errors and memory leak.
                    stream.Close();
                }
            }
        }

        public static void Main_SaveToFile_ExistingFile(MainSaveFile cur_MainSaveFile)
        {
            ClearLocationBuilder();

            SetFileLocation();

            SerializeSaveFile();
            
            void SetFileLocation()
            {
                Append_Main_SaveDirectory_Error();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(SavableManager.singleton._cur_savefile_id);
                saveLocation_strBuilder.Append(".data");
            }

            void SerializeSaveFile()
            {
                /// Create a file to save to
                Stream stream = new FileStream(saveLocation_strBuilder.ToString(), FileMode.Create, FileAccess.Write, FileShare.None);

                try
                {
                    /// Binary Formmater -- allows us to write data to a file.
                    IFormatter formatter = new BinaryFormatter();

                    /// Serialization method to WRITE to the file
                    formatter.Serialize(stream, cur_MainSaveFile);

                }
                catch (SerializationException exception)
                {
                    Debug.LogError("There was an issue serializing this data (Existing Main File): " + exception.Message);
                }
                finally
                {
                    /// Close the filestream to prevent errors and memory leak.
                    stream.Close();
                }
            }
        }
        #endregion

        #region LOADING MAIN
        public static List<MainSaveFile> Main_LoadAllFiles()
        {
            #region Create Result List, Binary Formatter.
            List<MainSaveFile> result = new List<MainSaveFile>();
            IFormatter formatter = new BinaryFormatter();
            #endregion

            #region Get All Files From Directory.
            ClearLocationBuilder();
            Append_Main_SaveDirectory();

            string _saveLocationString = saveLocation_strBuilder.ToString();
            if (!Directory.Exists(_saveLocationString))
            {
                return result;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(_saveLocationString);
            FileInfo[] fileInfo = dirInfo.GetFiles();
            #endregion

            #region Deserialize and Add To SaveFile List.
            int fileInfoCount = fileInfo.Length;
            for (int i = 0; i < fileInfoCount; i++)
            {
                FileStream stream = new FileStream(dirInfo + fileInfo[i].Name, FileMode.Open);
                result.Add((MainSaveFile)formatter.Deserialize(stream));
                stream.Close();
            }

            return result;
            #endregion
        }

        public static MainSaveFile Main_LoadTargetFile(int _fileId)
        {
            MainSaveFile result = null;

            ClearLocationBuilder();

            SetFileLocation();

            DeserializeSaveFile();

            return result;

            void SetFileLocation()
            {
                Append_Main_SaveDirectory_Error();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(_fileId);
                saveLocation_strBuilder.Append(".data");
            }

            void DeserializeSaveFile()
            {
                FileStream stream = new FileStream(saveLocation_strBuilder.ToString(), FileMode.Open);

                IFormatter formatter = new BinaryFormatter();

                result = (MainSaveFile)formatter.Deserialize(stream);

                stream.Close();
            }
        }
        #endregion

        #region DELETE MAIN
        public static void Main_DeleteFile(int saveFileId)
        {
            ClearLocationBuilder();

            SetFileLocation();

            DeleteSaveFile();

            void SetFileLocation()
            {
                Append_Main_SaveDirectory_Error();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(saveFileId);
                saveLocation_strBuilder.Append(".data");
            }

            void DeleteSaveFile()
            {
                File.Delete(saveLocation_strBuilder.ToString());
            }
        }
        #endregion

        #region RENAME MAIN
        public static void Main_RenameFile(int oldFileId, int newFildId)
        {
            string _main_directory;
            string _oldFilePath;
            string _newFilePath;
            
            GetDirectoryLocation();

            SetBothFilePaths();

            RenameFile();

            void GetDirectoryLocation()
            {
                _main_directory = Application.persistentDataPath + "/Saves/MainFiles/";

                if (!Directory.Exists(_main_directory))
                {
                    Debug.LogError("Main Save Directory could not be found (Rename).");
                }
            }

            void SetBothFilePaths()
            {
                ClearLocationBuilder();
                _oldFilePath = saveLocation_strBuilder.Append(_main_directory).Append("SaveData_").Append(oldFileId).Append(".data").ToString();

                ClearLocationBuilder();
                _newFilePath = saveLocation_strBuilder.Append(_main_directory).Append("SaveData_").Append(newFildId).Append(".data").ToString();
            }

            void RenameFile()
            {
                if (File.Exists(_newFilePath))
                {
                    Debug.LogErrorFormat("(Main) The file you are attempting to rename already exists! The file path is {0}.", _newFilePath);
                }
                else
                {
                    File.Move(_oldFilePath, _newFilePath);
                }
            }
        }
        #endregion

        #region MAIN LOCATION
        static void Append_Main_SaveDirectory_Create()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/MainFiles/");
            string _directoryLoaction = saveLocation_strBuilder.ToString();

            if (!Directory.Exists(_directoryLoaction))
            {
                Directory.CreateDirectory(_directoryLoaction);
            }
        }

        static void Append_Main_SaveDirectory_Error()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/MainFiles/");

            if (!Directory.Exists(saveLocation_strBuilder.ToString()))
            {
                Debug.LogError("Main Save Directory could not be found (Load or Delete Save).");
            }
        }

        static void Append_Main_SaveDirectory()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/MainFiles/");
        }
        #endregion

        /// SUB.

        #region SAVING SUB
        public static void Sub_SaveToFile_NewGame(SubSaveFile new_SubSaveFile)
        {
            ClearLocationBuilder();

            SetFileLocation();

            SerializeSaveFile();

            void SetFileLocation()
            {
                Append_Sub_SaveDirectory_Create();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(new_SubSaveFile.saveFileId);
                saveLocation_strBuilder.Append(".data");
            }

            void SerializeSaveFile()
            {
                /// Create a file to save to
                Stream stream = new FileStream(saveLocation_strBuilder.ToString(), FileMode.Create, FileAccess.Write, FileShare.None);

                try
                {
                    /// Binary Formmater -- allows us to write data to a file.
                    IFormatter formatter = new BinaryFormatter();

                    /// Serialization method to WRITE to the file
                    formatter.Serialize(stream, new_SubSaveFile);

                }
                catch (SerializationException exception)
                {
                    Debug.LogError("There was an issue serializing this data (New Sub File): " + exception.Message);
                }
                finally
                {
                    /// Close the filestream to prevent errors and memory leak.
                    stream.Close();
                }
            }
        }

        public static void Sub_SaveToFile_ExistingFile(SubSaveFile cur_SubSaveFile)
        {
            ClearLocationBuilder();

            SetFileLocation();

            SerializeSaveFile();

            void SetFileLocation()
            {
                Append_Sub_SaveDirectory_Error();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(cur_SubSaveFile.saveFileId);
                saveLocation_strBuilder.Append(".data");
            }

            void SerializeSaveFile()
            {
                /// Create a file to save to
                Stream stream = new FileStream(saveLocation_strBuilder.ToString(), FileMode.Create, FileAccess.Write, FileShare.None);

                try
                {
                    /// Binary Formmater -- allows us to write data to a file.
                    IFormatter formatter = new BinaryFormatter();

                    /// Serialization method to WRITE to the file
                    formatter.Serialize(stream, cur_SubSaveFile);

                }
                catch (SerializationException exception)
                {
                    Debug.LogError("There was an issue serializing this data (Existing Sub File): " + exception.Message);
                }
                finally
                {
                    /// Close the filestream to prevent errors and memory leak.
                    stream.Close();
                }
            }
        }
        #endregion

        #region LOADING SUB
        public static List<SubSaveFile> Sub_LoadAllFiles()
        {
            #region Create Result List, Binary Formatter.
            List<SubSaveFile> result = new List<SubSaveFile>();
            IFormatter formatter = new BinaryFormatter();
            #endregion

            #region Get All Files From Directory.
            ClearLocationBuilder();
            Append_Sub_SaveDirectory();

            string _saveLocationPath = saveLocation_strBuilder.ToString();
            if (!Directory.Exists(_saveLocationPath))
            {
                return result;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(_saveLocationPath);
            FileInfo[] fileInfo = dirInfo.GetFiles();
            #endregion

            #region Deserialize and Add To SaveFile List.
            int fileInfoCount = fileInfo.Length;
            for (int i = 0; i < fileInfoCount; i++)
            {
                FileStream stream = new FileStream(dirInfo + fileInfo[i].Name, FileMode.Open);
                result.Add((SubSaveFile)formatter.Deserialize(stream));
                stream.Close();
            }

            return result;
            #endregion
        }
        #endregion

        #region DELETE SUB
        public static void Sub_DeleteFile(int saveFileId)
        {
            ClearLocationBuilder();

            SetFileLocation();

            DeleteSaveFile();

            void SetFileLocation()
            {
                Append_Sub_SaveDirectory_Error();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(saveFileId);
                saveLocation_strBuilder.Append(".data");
            }

            void DeleteSaveFile()
            {
                File.Delete(saveLocation_strBuilder.ToString());
            }
        }
        #endregion

        #region RENAME SUB
        public static void Sub_RenameFile(int oldFileId, int newFildId)
        {
            string _sub_directory;
            string _oldFilePath;
            string _newFilePath;

            GetDirectoryLocation();

            SetBothFilePaths();

            RenameFile();

            void GetDirectoryLocation()
            {
                _sub_directory = Application.persistentDataPath + "/Saves/SubFiles/";

                if (!Directory.Exists(_sub_directory))
                {
                    Debug.LogError("Sub Save Directory could not be found (Rename).");
                }
            }

            void SetBothFilePaths()
            {
                ClearLocationBuilder();
                _oldFilePath = saveLocation_strBuilder.Append(_sub_directory).Append("SaveData_").Append(oldFileId).Append(".data").ToString();

                ClearLocationBuilder();
                _newFilePath = saveLocation_strBuilder.Append(_sub_directory).Append("SaveData_").Append(newFildId).Append(".data").ToString();
            }

            void RenameFile()
            {
                if (File.Exists(_newFilePath))
                {
                    Debug.LogErrorFormat("(Sub) The file you are attempting to rename already exists! The file path is {0}.", _newFilePath);
                }
                else
                {
                    File.Move(_oldFilePath, _newFilePath);
                }
            }
        }
        #endregion

        #region SUB LOCATION
        static void Append_Sub_SaveDirectory_Create()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/SubFiles/");
            string _directoryLoaction = saveLocation_strBuilder.ToString();

            if (!Directory.Exists(_directoryLoaction))
            {
                Directory.CreateDirectory(_directoryLoaction);
            }
        }

        static void Append_Sub_SaveDirectory_Error()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/SubFiles/");

            if (!Directory.Exists(saveLocation_strBuilder.ToString()))
            {
                Debug.LogError("Sub Save Directory could not be found (Load or Delete Save).");
            }
        }

        static void Append_Sub_SaveDirectory()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/SubFiles/");
        }
        #endregion

        /// AVATAR.

        #region SAVING AVATAR.
        public static void Avatar_SaveToFile_NewGame(byte[] imgBytes)
        {
            ClearLocationBuilder();

            SetFileLocation();

            SerializeSaveFile();

            void SetFileLocation()
            {
                Append_Avatar_SaveDirectory_Create();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(SavableManager.singleton._cur_savefile_id);
                saveLocation_strBuilder.Append(".png");
            }

            void SerializeSaveFile()
            {
                File.WriteAllBytes(saveLocation_strBuilder.ToString(), imgBytes);
            }
        }
        #endregion

        #region LOADING AVATAR.
        public static void SetupAvatarDictionary()
        {
            #region Create File Separator, Result List.
            char[] fileNameSeparter = new char[] { '_', '.' };

            List<Sprite> result = new List<Sprite>();
            #endregion

            #region Get All Files From Directory.
            ClearLocationBuilder();
            Append_Avatar_SaveDirectory();
            
            string _saveLocationPath = saveLocation_strBuilder.ToString();
            if (!Directory.Exists(_saveLocationPath))
                return;

            DirectoryInfo dirInfo = new DirectoryInfo(_saveLocationPath);
            FileInfo[] fileInfo = dirInfo.GetFiles();
            #endregion

            #region Create Sprites, Add to Dictionary.
            Dictionary<int, Sprite> _avatarIconDict = SavableManager.singleton.savedAvatarIconDict;
            int fileInfoLength = fileInfo.Length;

            for (int i = 0; i < fileInfoLength; i++)
            {
                /// Split File Name, Get Dict Key.
                string[] readName = fileInfo[i].Name.Split(fileNameSeparter, StringSplitOptions.RemoveEmptyEntries);
                int dictKey_i = int.Parse(readName[1]);

                /// Check is Dictionary contains this sprite already.
                if (!_avatarIconDict.ContainsKey(dictKey_i))
                {
                    /// Create Sprite.
                    Texture2D spriteTexture = LoadTexture(fileInfo[i].FullName);
                    Sprite newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), 100f, 0, SpriteMeshType.Tight);
                    newSprite.name = readName[1];

                    _avatarIconDict.Add(dictKey_i, newSprite);
                }
            }
            #endregion
        }

        static Texture2D LoadTexture(string FilePath)
        {
            Texture2D newTexture;
            byte[] fileData;

            if (File.Exists(FilePath))
            {
                fileData = File.ReadAllBytes(FilePath);
                newTexture = new Texture2D(2, 2);
                if (newTexture.LoadImage(fileData))
                    return newTexture;
            }

            return null;
        }
        #endregion

        #region DELETE AVATAR
        public static void Avatar_DeleteFile(int saveFileId)
        {
            ClearLocationBuilder();

            SetFileLocation();

            DeleteSaveFile();

            void SetFileLocation()
            {
                Append_Avatar_SaveDirectory_Error();
                saveLocation_strBuilder.Append("SaveData_");
                saveLocation_strBuilder.Append(saveFileId);
                saveLocation_strBuilder.Append(".png");
            }

            void DeleteSaveFile()
            {
                File.Delete(saveLocation_strBuilder.ToString());
            }
        }
        #endregion

        #region RENAME AVATAR
        public static void Avatar_RenameFile(int oldFileId, int newFildId)
        {
            string _avatar_directory;
            string _oldFilePath;
            string _newFilePath;

            GetDirectoryLocation();

            SetBothFilePaths();

            RenameFile();

            void GetDirectoryLocation()
            {
                _avatar_directory = Application.persistentDataPath + "/Saves/AvatarIcons/";

                if (!Directory.Exists(_avatar_directory))
                {
                    Debug.LogError("Avatar Icon Directory could not be found (Rename).");
                }
            }

            void SetBothFilePaths()
            {
                ClearLocationBuilder();
                _oldFilePath = saveLocation_strBuilder.Append(_avatar_directory).Append("SaveData_").Append(oldFileId).Append(".png").ToString();

                ClearLocationBuilder();
                _newFilePath = saveLocation_strBuilder.Append(_avatar_directory).Append("SaveData_").Append(newFildId).Append(".png").ToString();
            }

            void RenameFile()
            {
                if (File.Exists(_newFilePath))
                {
                    Debug.LogErrorFormat("(Avatar) The file you are attempting to rename already exists! The file path is {0}.", _newFilePath);
                }
                else
                {
                    File.Move(_oldFilePath, _newFilePath);
                }
            }
        }
        #endregion

        #region AVATAR LOCATION.
        public static void Append_Avatar_SaveDirectory_Create()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/AvatarIcons/");
            string _directoryLoaction = saveLocation_strBuilder.ToString();

            if (!Directory.Exists(_directoryLoaction))
            {
                Directory.CreateDirectory(_directoryLoaction);
            }
        }

        public static void Append_Avatar_SaveDirectory_Error()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/AvatarIcons/");
            string _directoryLoaction = saveLocation_strBuilder.ToString();

            if (!Directory.Exists(_directoryLoaction))
            {
                Debug.LogError("Avatar Icon could not be found (Load or Delete Save).");
            }
        }

        static void Append_Avatar_SaveDirectory()
        {
            saveLocation_strBuilder.Append(Application.persistentDataPath + "/Saves/AvatarIcons/");
        }
        #endregion

        #region Location StrBuilder.
        static void ClearLocationBuilder()
        {
            saveLocation_strBuilder.Length = 0;
        }
        #endregion

        #region Init.
        public static void Init()
        {
            InitRefs();

            void InitRefs()
            {
                saveLocation_strBuilder = new StringBuilder();
            }
        }
        #endregion
    }

    [Serializable]
    public class MainSaveFile
    {
        /// Player.
        public SavablePlayerState savedPlayerState;
        public SavableStatusState savedStatusState;
        /// Weapon.
        public List<SavableWeaponState> savedWeaponStates = new List<SavableWeaponState>();
        /// Armor.
        public List<SavableHeadState> savedHeadStates = new List<SavableHeadState>();
        public List<SavableChestState> savedChestStates = new List<SavableChestState>();
        public List<SavableHandState> savedHandStates = new List<SavableHandState>();
        public List<SavableLegState> savedLegStates = new List<SavableLegState>();
        /// Charm.
        public List<SavableCharmState> savedCharmStates = new List<SavableCharmState>();
        /// Powerup.
        public List<SavablePowerupState> savedPowerupStates = new List<SavablePowerupState>();
        /// Ring.
        public List<SavableRingState> savedRingStates = new List<SavableRingState>();
        /// Consumable.
        public List<SavableStatsEffectConsumableState> savedStatesEffectConsumableStates = new List<SavableStatsEffectConsumableState>();
        public List<SavableThrowableConsumableState> savedThrowableConsumableStates = new List<SavableThrowableConsumableState>();
        /// Enemy.
        public List<SavableEnemyState> savedEnemyStates = new List<SavableEnemyState>();
        /// Interactions.
        public List<SavableInteractionState> savedInteractionStates = new List<SavableInteractionState>();
        /// Boss.
        public SavableBossState savedBossState;
    }

    [Serializable]
    public class SubSaveFile
    {
        public int saveFileId;
        public bool isUsed;
        /// Profile.
        public SavableProfileState _savedProfileState;
    }

    #region Savable States

    #region Player.
    [Serializable]
    public class SavablePlayerState
    {
        public SavableVector3 savablePosition;
        public SavableVector3 savableEulers;
        public int savableSpawnPointId;
    }

    [Serializable]
    public class SavableStatusState
    {
        /// General.
        public string savablePlayerName;
        public int savablePlayerLevel;
        public int savableVolun;

        /// Attributes.
        public int savableVigor;
        public int savableAdaptation;
        public int savableEndurance;
        public int savableVitality;
        public int savableStrength;
        public int savableHexes;
        public int savableIntelligence;
        public int savableDivinity;
        public int savableFortune;

        /// Stats.
        public float savable_hp;
        public float savable_fp;

        /// Vessels.
        public int savable_volunVessel_amount;
        public int savable_volunFragment_amount;
        public int savable_shatteredAmuletPiece_amount;
        public int savable_sodurVessel_amount;
    }

    [Serializable]
    public class SavableInteractionState
    {
        public string interactionId;
        public bool isInteracted;
    }
    #endregion

    #region Equipments.
    [Serializable]
    public class SavableWeaponState
    {
        /// Runtime General.
        public string savableWeaponName;
        public int savableWeaponUniqueId;
        public int savableWeaponSlotNumber;

        /// Weapon General.
        public int savableWeaponSlotSide;
        public bool savableIsCurrentRhWeapon;
        public bool savableIsCurrentLhWeapon;

        /// Weapon Modifiable Stats.
        public int savableInfusedElementType;
        public int savableWeaponFortifiedLevel;
        public float savableWeaponDurability;

        /// Weapon Attack Powers.
        public int savableMainAttackPower;
        public double savableMainAttackBonus;
        public int savableCriticalAttPower;
        public int savableRange;
        public int savableSpellBuff;
        
        /// Weapon Additional Effects.
        public int savableBleedEffect;
        public int savablePoisonEffect;
        public int savableFrostEffect;
    }

    [Serializable]
    public class SavableArrowState
    {

    }

    [Serializable]
    public class SavableHeadState
    {
        /// Runtime General.
        public string savableHeadName;
        public int savableHeadUniqueId;

        /// Armor General.
        public int savableHeadSlotSide;
    }

    [Serializable]
    public class SavableChestState
    {
        /// Runtime General.
        public string savableChestName;
        public int savableChestUniqueId;

        /// Armor General.
        public int savableChestSlotSide;
    }

    [Serializable]
    public class SavableHandState
    {
        /// Runtime General.
        public string savableHandName;
        public int savableHandUniqueId;

        /// Armor General.
        public int savableHandSlotSide;
    }

    [Serializable]
    public class SavableLegState
    {
        /// Runtime General.
        public string savableLegName;
        public int savableLegUniqueId;

        /// Armor General.
        public int savableLegSlotSide;
    }

    [Serializable]
    public class SavableCharmState
    {
        /// Runtime General.
        public string savableCharmName;
        public int savableCharmUniqueId;

        /// Charm General.
        public int savableCharmSlotSide;
    }

    [Serializable]
    public class SavablePowerupState
    {
        /// Runtime General.
        public string savablePowerupName;
        public int savablePowerupUniqueId;

        /// Powerup General.
        public int savablePowerupSlotSide;
    }

    [Serializable]
    public class SavableRingState
    {
        /// Runtime General.
        public string savableRingName;
        public int savableRingUniqueId;

        /// Ring General.
        public int savableRingSlotSide;

        /// Ring Modifiable Stats.
        public int savableRingFortifiedLevel;
    }

    [Serializable]
    public class SavableStatsEffectConsumableState
    {
        /// Runtime General.
        public string savableStatsEffectName;
        public int savableStatsEffectUniqueId;
        public int savableStatsEffectSlotNumber;

        /// Consumable General.
        public int savableStatsEffectSlotSide;
        public bool savableStatsEffectIsCurrent;

        /// Consumable Modifiable Stats.
        public int savableStatsEffectFortifiedLevel;
        public int savableStatsEffectCurrentCarryingAmount;
        public int savableStatsEffectCurrentStoredAmount;
        public bool savableStatsEffectIsCarryingEmpty;
        public bool savableStatsEffectIsVessel;
        public bool savableStatsEffectIsVolun;

        /// Runtime Dictionary TKey.
        public int savableStatsEffectStackId;
    }

    [Serializable]
    public class SavableThrowableConsumableState
    {
        /// Runtime General.
        public string savableThrowableName;
        public int savableThrowableUniqueId;
        public int savableThrowableSlotNumber;

        /// Consumable General.
        public int savableThrowableSlotSide;
        public bool savableThrowableIsCurrent;

        /// Consumable Modifiable Stats.
        public int savableThrowableCurrentCarryingAmount;
        public int savableThrowableCurrentStoredAmount;
        public bool savableThrowableIsCarryingEmpty;

        /// Runtime Dictionary TKey.
        public int savableThrowableStackId;
    }

    [Serializable]
    public class SavableSpellState
    {
        public string savableSpellName;
        public int savableSpellUniqueId;
        public int savableSpellSlotNumber;
    }
    #endregion

    #region Enemy.
    [Serializable]
    public class SavableEnemyState
    {
        public string savableId;
        public bool isDeadFlag;
    }

    [Serializable]
    public class SavableBossState
    {
        public bool savableIsBossSequenceTriggered;
        public bool savableIsBossKilled;
    }
    #endregion

    #region Profile.
    [Serializable]
    public class SavableProfileState
    {
        public string savableProfName;
        public string savableProfDate;
        public int savableProfLevel;
        public int savableProfVolun;
    }
    #endregion

    #endregion

    #region Savable Struct.
    [Serializable]
    public struct SavableVector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3 GetValues()
        {
            return new Vector3(x, y, z);
        }

        public SavableVector3(Vector3 p)
        {
            x = p.x;
            y = p.y;
            z = p.z;
        }
    }
    #endregion
}

/*
public static List<Sprite> GetAllAvatarIcons()
    {
        List<Sprite> result = new List<Sprite>();
        DirectoryInfo dirInfo = new DirectoryInfo(AvatarSaveLocation());
        FileInfo[] fileInfo = dirInfo.GetFiles();

        int fileInfoCount = fileInfo.Length;
        for (int i = 0; i < fileInfoCount; i++)
        {
            string[] readName = fileInfo[i].Name.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (readName.Length == 2)
            {
                if (string.Equals(("png"), readName[1]))
                {
                    //Debug.Log("FileFullPath: " + fileInfo[i].FullName);
                    Texture2D spriteTexture = LoadTexture(fileInfo[i].FullName);
                    Sprite newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), 100f, 0, SpriteMeshType.Tight);
                    newSprite.name = readName[0];
                    result.Add(newSprite);
                }
            }
        }

        return result;
    }
*/

/*
#region LOADING SUB
        public static List<SubSaveFile> Sub_LoadAllFiles()
        {
            #region Set File Name Separter.
            string[] fileNameSeparter = new string[1];
            fileNameSeparter[0] = ".";
            #endregion

            #region Create Result List, Binary Formatter.
            List<SubSaveFile> result = new List<SubSaveFile>();
            IFormatter formatter = new BinaryFormatter();
            #endregion

            #region Get All Files From Directory.
            ClearLocationBuilder();
            Append_Sub_SaveDirectory();

            string _saveLocationPath = saveLocation_strBuilder.ToString();
            if (!Directory.Exists(_saveLocationPath))
            {
                return result;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(_saveLocationPath);
            FileInfo[] fileInfo = dirInfo.GetFiles();
            #endregion

            #region Deserialize and Add To SaveFile List.
            int fileInfoCount = fileInfo.Length;
            for (int i = 0; i < fileInfoCount; i++)
            {
                string[] readName = fileInfo[i].Name.Split(fileNameSeparter, StringSplitOptions.RemoveEmptyEntries);
                if (readName.Length == 2)
                {
                    if (string.Equals("data", readName[1]))
                    {
                        FileStream stream = new FileStream(dirInfo + fileInfo[i].Name, FileMode.Open);
                        result.Add((SubSaveFile)formatter.Deserialize(stream));
                        stream.Close();
                    }
                }
            }

            return result;
            #endregion
        }
        #endregion
*/
