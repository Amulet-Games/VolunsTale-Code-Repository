using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PlayerSpawnAssembly : MonoBehaviour
    {
        [Header("Config.")]
        public string spawnAssemblyId;
        public PlayerSpawnPoint[] spawnPoints;
        
        [Header("Non Serialize.")]
        [ReadOnlyInspector]
        public Dictionary<int, PlayerSpawnPoint> spawnPointsDict = new Dictionary<int, PlayerSpawnPoint>();

        public void Setup()
        {
            InitPlayerSpawnPointsDict();
        }
        
        void InitPlayerSpawnPointsDict()
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (!spawnPointsDict.ContainsKey(spawnPoints[i].spawnId))
                {
                    spawnPointsDict.Add(spawnPoints[i].spawnId, spawnPoints[i]);
                }
                else
                {
                    Debug.LogError("Spawn Point Assembly already contains " + spawnPoints[i].spawnId);
                }
            }
        }

        public PlayerSpawnPoint GetSpawnPointById(int _id)
        {
            if (spawnPointsDict.TryGetValue(_id, out PlayerSpawnPoint _playerSpawn))
            {
                return _playerSpawn;
            }

            return null;
        }
    }
}