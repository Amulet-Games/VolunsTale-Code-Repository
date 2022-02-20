using UnityEditor;
using UnityEngine;

namespace SA
{
    [CustomEditor(typeof(MarksmanAIManager))]
    [CanEditMultipleObjects]
    public class MarksmanAIManagerInspector : AIManagerInspector
    {
        // Indicators.
        SerializedProperty rh_ParryIndicator;
        SerializedProperty lh_ParryIndicator;
        SerializedProperty canPlayIndicator;

        // Private ref.
        MarksmanAIManager ai;

        private void OnEnable()
        {
            ai = (MarksmanAIManager)target;
            
            // General

            OnInspectorGUI_Init();

            InitModHeaderGUIStyle();

            InitModSubHeaderGUIStyle();

            InitClassFoldoutHeaderGUIStyle();

            InitFoldoutHeaderGUIStyle();

            InitSubFoldoutHeaderGUIStyle();

            // Class

            Marksman_OnInspectorGUI_Init();

            // Mods

            RollIntervalMod_Init(serializedObject);

            AimingPlayerMod_Init(serializedObject);

            FullBodyDamageColliderMod_Init(serializedObject);

            EnemyInteractableMod_Init(serializedObject);

            LimitEnemyTurning_Init(serializedObject);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General

            OnInspectorGUI_Tick();

            // Class

            Marksman_OnInspectorGUI_Tick();

            // Mods

            Marksman_Mods_OnInspectorGUI_Tick();

            serializedObject.ApplyModifiedProperties();
        }

        void Marksman_OnInspectorGUI_Init()
        {
            // Indicators.

            rh_ParryIndicator = serializedObject.FindProperty("rh_ParryIndicator");
            lh_ParryIndicator = serializedObject.FindProperty("lh_ParryIndicator");
            canPlayIndicator = serializedObject.FindProperty("canPlayIndicator");

            showIndicators = serializedObject.FindProperty("showIndicators");
            openIndicators = showIndicators.boolValue;
        }

        void Marksman_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            #region Indicators.
            openIndicators = EditorGUILayout.BeginFoldoutHeaderGroup(openIndicators, "AI Indicators", classFoldoutHeaderGUIStyle);
            if (openIndicators)
            {
                showIndicators.boolValue = true;
                EditorGUILayout.PropertyField(rh_ParryIndicator);
                EditorGUILayout.PropertyField(lh_ParryIndicator);
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

        void Marksman_Mods_OnInspectorGUI_Tick()
        {
            EditorGUILayout.BeginVertical();

            RollIntervalMod_Tick();

            AimingPlayerMod_Tick();

            FullBodyDamageColliderMod_Tick();

            EnemyInteractableMod_Tick();

            LimitEnemyTurning_Tick();

            EditorGUILayout.EndVertical();
        }
    }
}