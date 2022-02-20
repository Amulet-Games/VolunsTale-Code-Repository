using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    [CreateAssetMenu(menuName = "AI/AI Weapons/Sidearm")]
    public class EnemySidearm : ScriptableObject
    {
        [Header("Sidearm Prefab")]
        public EnemyRuntimeSidearm modelPrefab;

        [Header("Shealth transform")]
        public AISheathTransform weaponSheathTransform;

        public EnemyRuntimeSidearm GetRuntimeSidearm()
        {
            EnemyRuntimeSidearm runtimeSidearm = Instantiate(modelPrefab);
            runtimeSidearm.gameObject.SetActive(true);
            runtimeSidearm.referedEnemySidearm = this;
            return runtimeSidearm;
        }
    }
}