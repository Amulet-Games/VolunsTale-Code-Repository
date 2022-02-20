using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(SwordsmanAIManager))]
    [CanEditMultipleObjects]
    public class SwordsmanAIManagerInspector : AIManagerInspector
    {
        // Indicators.
        SerializedProperty rh_ParryIndicator;
        SerializedProperty canPlayIndicator;

        // Private ref.
        SwordsmanAIManager ai;

        private void OnEnable()
        {
            ai = (SwordsmanAIManager)target;
            
            // General

            OnInspectorGUI_Init();

            InitModHeaderGUIStyle();

            InitModSubHeaderGUIStyle();

            InitClassFoldoutHeaderGUIStyle();

            InitFoldoutHeaderGUIStyle();

            InitSubFoldoutHeaderGUIStyle();

            // Class

            Swordsman_OnInspectorGUI_Init();

            // Mods

            LimitEnemyTurning_Init(serializedObject);
            
            RollIntervalMod_Init(serializedObject);

            TwoStanceCombatMod_Init(serializedObject);

            EnemyTauntMod_Init(serializedObject);

            ParryPlayerMod_Init(serializedObject);

            PerilousAttackMod_Init(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General

            OnInspectorGUI_Tick();

            // Class

            Swordsman_OnInspectorGUI_Tick();

            // Mods

            Swordsman_Mods_OnInspectorGUI_Tick();

            serializedObject.ApplyModifiedProperties();
        }

        void Swordsman_OnInspectorGUI_Init()
        {
            // Indicators.

            rh_ParryIndicator = serializedObject.FindProperty("rh_ParryIndicator");
            canPlayIndicator = serializedObject.FindProperty("canPlayIndicator");

            showIndicators = serializedObject.FindProperty("showIndicators");
            openIndicators = showIndicators.boolValue;
        }

        void Swordsman_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            #region Indicators.
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

        void Swordsman_Mods_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            LimitEnemyTurning_Tick();

            RollIntervalMod_Tick();

            TwoStanceCombatMod_Tick();

            EnemyTauntMod_Tick();

            ParryPlayerMod_Tick();

            PerilousAttackMod_Tick();

            EditorGUILayout.EndVertical();
        }
    }
}