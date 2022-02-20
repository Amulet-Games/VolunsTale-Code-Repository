using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Start Game Spawn Point.")]
        public PlayerSpawnPoint defualtStartGameSpawnPoint;
        public PlayerSpawnPoint _debugGameSpawnPoint;

        [Header("Spawn Assemblys.")]
        public PlayerSpawnAssembly _WinterFieldSpawnAssembly;
        
        public static LevelManager singleton;
        void Awake()
        {
            if (singleton != null)
                Destroy(this);
            else
                singleton = this;
        }

        public void Setup()
        {
            _WinterFieldSpawnAssembly.Setup();
        }
        
        public PlayerSpawnPoint Get_WF_SpawnPointFromDict(int _targetSpawnPointId)
        {
            return _WinterFieldSpawnAssembly.GetSpawnPointById(_targetSpawnPointId);
        }
    }
}