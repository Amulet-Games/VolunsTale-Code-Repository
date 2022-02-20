using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(LancerAIManager))]
    [CanEditMultipleObjects]
    public class LancerAIManagerInspector : AIManagerInspector
    {
        // Indicators.
        SerializedProperty rh_ParryIndicator;
        SerializedProperty canPlayIndicator;

        // Private ref.
        LancerAIManager ai;

        private void OnEnable()
        {
            ai = (LancerAIManager)target;
            
            // General

            OnInspectorGUI_Init();

            InitModHeaderGUIStyle();

            InitModSubHeaderGUIStyle();

            InitClassFoldoutHeaderGUIStyle();

            InitFoldoutHeaderGUIStyle();

            InitSubFoldoutHeaderGUIStyle();

            // Class

            Lancer_OnInspectorGUI_Init();

            // Mods

            PerilousAttackMod_Init(serializedObject);

            RollIntervalMod_Init(serializedObject);

            EnemyEvolveMod_Init(serializedObject);

            DamageParticleAttackMod_Init(serializedObject);

            LeftLegDamageColliderMod_Init(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General

            OnInspectorGUI_Tick();

            // Class

            Lancer_OnInspectorGUI_Tick();

            // Mods

            Lancer_Mods_OnInspectorGUI_Tick();

            serializedObject.ApplyModifiedProperties();
        }

        void Lancer_OnInspectorGUI_Init()
        {
            // Indicators.

            rh_ParryIndicator = serializedObject.FindProperty("rh_ParryIndicator");
            canPlayIndicator = serializedObject.FindProperty("canPlayIndicator");

            showIndicators = serializedObject.FindProperty("showIndicators");
            openIndicators = showIndicators.boolValue;
        }

        void Lancer_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            #region  Indicators.
            openIndicators = EditorGUILayout.BeginFoldoutHeaderGroup(openIndicators, "AI Indicators", classFoldoutHeaderGUIStyle);
            if (openIndicators)
            {
                showIndicators.boolValue = true;
                EditorGUILayout.PropertyField(rh_ParryIndicator);
                EditorGUILayout.PropertyField(canPlayIndicator);
            }
            else
            {
                showIndicators.boolValue = false;
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            #endregion

            DrawUILine1();

            EditorGUILayout.EndVertical();
        }

        void Lancer_Mods_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            PerilousAttackMod_Tick();

            RollIntervalMod_Tick();

            EnemyEvolveMod_Tick();

            DamageParticleAttackMod_Tick();

            LeftLegDamageColliderMod_Tick();

            EditorGUILayout.EndVertical();
        }
    }
}